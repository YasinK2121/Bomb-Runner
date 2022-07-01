using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] GetScripts m_GetScripts;
    [SerializeField] GameObject ExplosionBombParticleEffect;
    private bool BombTimerCheck;
    public bool MoveBomb;
    public bool FallowBombCheck;
    private float Timer = 0;

    void Start()
    {
        transform.GetComponent<SphereCollider>().radius = 0.3f;
        MoveBomb = false;
        gameObject.GetComponent<Collider>().enabled = false;
        FallowBombCheck = true;
    }

    void Update()
    {
        BombTimer(BombTimerCheck);

        if (MoveBomb)
        {
            MoveBombFinish();
        }
    }

    public void MoveBombFinish()
    {
        transform.parent = null;
        this.GetComponent<Collider>().enabled = true;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, transform.position.y, transform.position.z), Time.deltaTime * 10);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, m_GetScripts.GameManager.BombMovePos.z), Time.deltaTime * 30);
        transform.GetComponent<SphereCollider>().radius = 1.5f;
        if (transform.position.z == m_GetScripts.GameManager.BombMovePos.z)
        {
            m_GetScripts.Player.BombLastPos = transform.position;
            FallowBombCheck = false;
            BombActivated();
            transform.GetComponent<SphereCollider>().radius = 0.3f;
        }
    }

    public void BombActivated()
    {
        transform.parent = null;
        BombTimerCheck = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<Collider>().enabled = true;
    }

    public void BombTimer(bool active)
    {
        if (active)
        {
            Timer += Time.deltaTime;
            if (Timer >= 1f)
            {
                Instantiate(ExplosionBombParticleEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);               
                Destroy(this.gameObject);

                if (m_GetScripts.GameManager.FinishGame)
                {
                    m_GetScripts.InGameCanvasManager.CanvasFinishGame();
                }
            }
        }
    }
}
