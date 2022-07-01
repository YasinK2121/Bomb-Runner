using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    [SerializeField] MyScript m_MyScript;
    [SerializeField] GetScripts m_GetScripts;
    public Collider[] m_Collider;

    private bool m_LoopTrap;
    private bool m_GroundTrap;
    private bool m_HammerTrap;
    private bool m_PoleTrap;
    private bool m_GoRight;
    private bool m_GoLeft;
    private bool m_YasinIf;
    private bool m_ColliderCheck;

    [Range(0, 10)]
    [SerializeField] float m_TrapSpeed;

    [Range(0, 20)]
    [SerializeField] float m_TrapSpeedTwo;

    [SerializeField] bool m_PoleTrapCheck;
    [SerializeField] float m_PoleTimer;

    [SerializeField] GameObject OneBox;
    [SerializeField] GameObject SecondBox;

    public GameObject Wall;
    public List<Transform> BrokeWall;
    float g;

    [System.Serializable]
    public class PoleTrap
    {
        public Transform Trap;
        public bool StartMovement;
        public bool Down;
        public bool Up;
    }
    [Range(-50, 50)]
    [SerializeField] private float m_PoleTrapMin;
    [Range(-50, 50)]
    [SerializeField] private float m_PoleTrapMax;

    [SerializeField] private PoleTrap[] m_PoleTraps = new PoleTrap[2];

    void Start()
    {
        m_ColliderCheck = false;
        m_LoopTrap = false;
        m_GroundTrap = false;
        m_HammerTrap = false;
        m_PoleTrap = false;
        m_GoRight = false;
        m_GoLeft = false;
        m_YasinIf = true;

        switch (m_MyScript.MyType)
        {
            case TriggerScript.Types.Loop_Trap:
                m_LoopTrap = true;
                break;

            case TriggerScript.Types.Ground_Trap:
                m_GroundTrap = true;
                break;

            case TriggerScript.Types.Hammer_Trap:
                m_HammerTrap = true;
                break;

            case TriggerScript.Types.Pole_Trap:
                m_PoleTrap = true;
                break;
        }

        int RandomDirection = Random.Range(0, 2);
        if (RandomDirection == 0) { m_GoRight = true; } else { m_GoLeft = true; };
    }

    float t = 0;
    void Update()
    {
        if (m_ColliderCheck)
        {
            t += Time.deltaTime;
            if (t > 1.2f)
            {
                t = 0;
                EnableCollider();
            }
        }

        if (m_YasinIf)
        {
            AllTrapsSettings();
        }
    }

    public void EnableCollider()
    {
        foreach (var item in m_Collider)
        {
            item.enabled = true;
        }
        m_ColliderCheck = false;
    }

    public void DisableCollider()
    {
        m_YasinIf = false;
        foreach (var item in m_Collider)
        {
            item.enabled = false;
        }
        m_ColliderCheck = true;
    }

    private void AllTrapsSettings()
    {
        if (m_LoopTrap)
        {
            TrapLoop(m_TrapSpeed);
        }
        else if (m_GroundTrap && m_YasinIf)
        {
            TrapMoveY();
        }
        else if (m_HammerTrap)
        {
            TrapMoveX(m_TrapSpeed, m_TrapSpeedTwo);
        }
        else if (m_PoleTrap)
        {
            PoleTrap_TrapMove();
        }
    }

    private void TrapLoop(float Speed)
    {
        transform.Rotate(new Vector3(0, 1 * Speed, 0));
    }

    private void TrapMoveY()
    {
        if (m_GoLeft)
        {
            g += Time.deltaTime;
            if (g > 1f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * m_TrapSpeed, transform.position.z);
            }
        }
        else if (m_GoRight)
        {
            g += Time.deltaTime;
            if (g > 1f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * m_TrapSpeed, transform.position.z);
            }
        }
    }

    private void TrapMoveX(float Speed, float SpeedTwo)
    {
        if (transform.eulerAngles.y == 0)
        {
            if (m_GoLeft)
            {
                transform.position = new Vector3(transform.position.x - Time.deltaTime * SpeedTwo, transform.position.y, transform.position.z);
            }
            else if (m_GoRight)
            {
                transform.position = new Vector3(transform.position.x + Time.deltaTime * Speed, transform.position.y, transform.position.z);
            }
        }
        else
        {
            if (m_GoLeft)
            {
                transform.position = new Vector3(transform.position.x + Time.deltaTime * SpeedTwo, transform.position.y, transform.position.z);
            }
            else if (m_GoRight)
            {
                transform.position = new Vector3(transform.position.x - Time.deltaTime * Speed, transform.position.y, transform.position.z);
            }
        }
    }

    private void PoleTrap_TrapMove()
    {
        Vector3 target = Vector3.zero;
        PoleMovement(0, ref target);
        PoleMovement(1, ref target);
    }

    private void PoleMovement(int index, ref Vector3 target)
    {
        if (m_PoleTraps[index].StartMovement)
        {
            if (m_PoleTraps[index].Down)
            {

                if (m_PoleTraps[index].Trap.position.y > m_PoleTrapMin)
                {
                    target = new Vector3(m_PoleTraps[index].Trap.position.x, m_PoleTrapMin, m_PoleTraps[index].Trap.position.z);
                    m_PoleTraps[index].Trap.position = Vector3.MoveTowards(m_PoleTraps[index].Trap.position, target, m_TrapSpeed * Time.deltaTime);
                }
                else
                {
                    m_PoleTraps[index].Down = false;
                    m_PoleTraps[index].Up = true;
                }
            }
            else if (m_PoleTraps[index].Up)
            {
                if (m_PoleTraps[index].Trap.position.y < m_PoleTrapMax)
                {
                    target = new Vector3(m_PoleTraps[index].Trap.position.x, m_PoleTrapMax, m_PoleTraps[index].Trap.position.z);
                    m_PoleTraps[index].Trap.position = Vector3.MoveTowards(m_PoleTraps[index].Trap.position, target, m_TrapSpeed * Time.deltaTime);
                }
                else
                {
                    m_PoleTraps[index].Down = false;
                    m_PoleTraps[index].Up = false;
                    m_PoleTraps[index].StartMovement = false;
                    int secondIndex = index == 0 ? 1 : 0;
                    target = Vector3.zero;
                    m_PoleTraps[secondIndex].StartMovement = true;
                    m_PoleTraps[secondIndex].Down = true;

                }
            }
        }
    }

    private void TrapGameOver()
    {
        m_YasinIf = false;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        if (Wall != null)
        {
            Wall.GetComponent<Collider>().enabled = true;
            Wall.GetComponent<Rigidbody>().isKinematic = false;
            foreach (var item in m_Collider)
            {
                if (item.GetComponent<Rigidbody>() != null)
                {
                    item.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Explosion"))
        {
            TrapGameOver();
        }

        if (collision.gameObject == OneBox)
        {
            m_GoRight = true;
            m_GoLeft = false;
            g = 0;
        }

        if (collision.gameObject == SecondBox)
        {
            m_GoRight = false;
            m_GoLeft = true;
            g = 0;
        }
    }

    private void OnCollisionExit(Collision collision)
    {

    }
}
