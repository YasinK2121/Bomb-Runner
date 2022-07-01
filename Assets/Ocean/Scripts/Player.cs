using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    static string[] AnimNames = new string[] { "Idle", "Run", "Bomb", "Damage" };

    [SerializeField] GetScripts m_GetScripts;
    [SerializeField] Transform[] TransformLimbs;
    [SerializeField] Animator p_Animator;
    [SerializeField] Rigidbody p_Rb;
    [SerializeField] GameObject Bomb;
    [SerializeField] GameObject WindParticle;

    [SerializeField] Vector3 c_IdleOffest;
    [SerializeField] Vector3 c_IdleRot;
    [SerializeField] Vector3 c_RunOffest;
    [SerializeField] Vector3 c_RunRot;

    private Touch m_Touch;
    private Vector3 refPos;
    public bool TakeDamageCheck;
    public int WindBombCount;

    [SerializeField] private float forwardSpeed;
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float smoothness;
    [SerializeField] float c_Speed;
    public bool WindSpeed;
    float p_NormalSpeed;
    float p_SpeedBoost;

    public Vector3 BombLastPos;

    public void Initialize()
    {
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        transform.eulerAngles = new Vector3(0, 158, 0);

        WindParticle.SetActive(false);
        WindSpeed = false;
        p_NormalSpeed = forwardSpeed;
        p_SpeedBoost = forwardSpeed + 10;

        Camera.main.transform.position = c_IdleOffest;
        Camera.main.transform.rotation = Quaternion.Euler(c_IdleRot);

        SetAnimStatus("Idle");
        OnOffCollider(false);
        OnOffRigidbody(false);
    }


    private void LateUpdate()
    {
        if (a != 0 && m_GetScripts.GameManager.HasTheGameStarted && !TakeDamageCheck)
        {
            FallowCamera();
        }
    }


    float t = 0;
    private void Update()
    {
        if (TakeDamageCheck)
        {
            t += Time.deltaTime;
            SetAnimStatus("Damage");
            if (t > 0.4f)
            {
                TakeDamageCheck = false;
                t = 0;
            }
        }

        if (m_GetScripts.GameManager.HasTheGameStarted)
        {
            PlayerMovement();
            PlayerWindSpeed();
        }

        FallowBomb();
    }

    int a = 0;
    private void PlayerMovement()
    {
        if (a == 0 && m_GetScripts.GameManager.HasTheGameStarted)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(transform.position.x + c_RunOffest.x, c_RunOffest.y, transform.position.z + c_RunOffest.z), Time.deltaTime * 3);
            Camera.main.transform.rotation = Quaternion.Euler(new Vector3(c_RunRot.x, 0, 0));

            if (Vector3.Distance(Camera.main.transform.position, new Vector3(transform.position.x + c_RunOffest.x, c_RunOffest.y, transform.position.z + c_RunOffest.z)) <= 0.2f)
            {
                a++;
            }
        }
        else
        {
            if (!TakeDamageCheck)
            {
                SetAnimStatus("Run");
                if (Input.touchCount > 0)
                {
                    m_Touch = Input.GetTouch(0);
                    if (m_Touch.phase == TouchPhase.Moved)
                    {
                        Vector2 touchPos = m_Touch.deltaPosition;
                        transform.Translate(new Vector3(touchPos.x * horizontalSpeed / 100 * Time.deltaTime, 0, 0));
                        float clampX = Mathf.Clamp(transform.position.x, -2.6f, 2.6f);
                        transform.position = new Vector3(clampX, transform.position.y, transform.position.z);
                    }
                }
                transform.Translate(new Vector3(0, 0, forwardSpeed * Time.deltaTime / 10));
            }
        }
    }

    float windTimer;
    private void PlayerWindSpeed()
    {
        if (WindSpeed)
        {
            windTimer -= Time.deltaTime;
            if (windTimer > 0)
            {
                if (WindBombCount == 3)
                {
                    m_GetScripts.InGameCanvasManager.InGameTextEnlarge.gameObject.SetActive(true);
                    p_Animator.speed = 1.3f;
                    if (!WindParticle.activeInHierarchy)
                    {
                        StartCoroutine(PlayerWindSpeedTimer());
                        StartCoroutine(m_GetScripts.InGameCanvasManager.WindSpeedText());
                    }
                    WindParticle.SetActive(true);
                    forwardSpeed = p_SpeedBoost;
                }
            }
            else
            {
                WindSpeed = false;
            }
        }
        else
        {
            windTimer = 3;
            WindBombCount = 0;
            //m_GetScripts.InGameCanvasManager.InGameTextEnlarge.gameObject.SetActive(false);
        }
    }

    public IEnumerator PlayerWindSpeedTimer()
    {
        yield return new WaitForSeconds(3f);
        DisableSpeedBoost();
        StopAllCoroutines();
    }

    public void DisableSpeedBoost()
    {
        p_Animator.speed = 1f;
        WindParticle.SetActive(false);
        WindSpeed = false;
        windTimer = 3;
        WindBombCount = 0;
        forwardSpeed = p_NormalSpeed;
        m_GetScripts.InGameCanvasManager.InGameTextEnlarge.gameObject.SetActive(false);
    }

    private void OnOffRigidbody(bool check)
    {
        foreach (var limbs in TransformLimbs)
        {
            if (!check)
            {
                limbs.GetComponent<Rigidbody>().isKinematic = true;
            }
            else if (check)
            {
                limbs.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    private void OnOffCollider(bool check)
    {
        foreach (var limbs in TransformLimbs)
        {
            if (check)
            {
                limbs.GetComponent<Collider>().enabled = true;
            }
            else if (!check)
            {
                limbs.GetComponent<Collider>().enabled = false;
            }
        }
    }

    public void PlayerGameOver()
    {
        OnOffCollider(true);
        OnOffRigidbody(true);
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        transform.GetComponent<Collider>().enabled = false;
        p_Animator.enabled = false;
    }

    public IEnumerator PlayerFinishGame()
    {
        SetAnimStatus("Bomb");
        yield return new WaitForSeconds(0.9f);
        m_GetScripts.Bomb.MoveBomb = true;
        m_GetScripts.GameManager.FinishGame = true;
    }

    Vector3 desiredPosition;
    Vector3 smoothedPosition;
    private void FallowCamera()
    {
        desiredPosition = transform.position + new Vector3(0, c_RunOffest.y, c_RunOffest.z);
        smoothedPosition = Vector3.SmoothDamp(Camera.main.transform.position, desiredPosition, ref refPos, smoothness);
        Camera.main.transform.position = new Vector3(smoothedPosition.x, c_RunOffest.y, desiredPosition.z);
    }

    private void FallowBomb()
    {
        if (m_GetScripts.GameManager.FinishGame)
        {
            if (Bomb != null && Bomb.GetComponent<Bomb>().FallowBombCheck)
            {
                desiredPosition = Bomb.transform.position + new Vector3(0f, 7f, -8);
                smoothedPosition = Vector3.Lerp(Camera.main.transform.position, desiredPosition, c_Speed);
                Camera.main.transform.position = new Vector3(c_RunOffest.x, smoothedPosition.y, smoothedPosition.z);
            }
            else
            {
                desiredPosition = BombLastPos + new Vector3(0f, 7f, -10);
                smoothedPosition = Vector3.Lerp(Camera.main.transform.position, desiredPosition, c_Speed);
                Camera.main.transform.position = new Vector3(c_RunOffest.x, smoothedPosition.y, smoothedPosition.z);
            }
        }
    }

    public void SetAnimStatus(string animName)
    {
        for (int i = 0; i < AnimNames.Length; i++)
        {
            if (AnimNames[i] == animName)
            {
                p_Animator.SetBool(AnimNames[i], true);
            }
            else
            {
                p_Animator.SetBool(AnimNames[i], false);
            }
        }
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 orginalpos = Camera.main.transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.localPosition = new Vector3(orginalpos.x + x, orginalpos.y + y, orginalpos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        Camera.main.transform.localPosition = orginalpos;
    }
}
