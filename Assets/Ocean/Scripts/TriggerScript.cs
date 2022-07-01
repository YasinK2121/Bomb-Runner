using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    [SerializeField] GetScripts m_GetScripts;

    public enum Types
    {
        Loop_Trap,
        Ground_Trap,
        Hammer_Trap,
        Pole_Trap,
        GroundBomb,
        Finish
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Trap":
                m_GetScripts.InGameCanvasManager.BombPowerRedBar.fillAmount -= 0.1f;
                m_GetScripts.Player.DisableSpeedBoost();
                if (m_GetScripts.InGameCanvasManager.BombPowerRedBar.fillAmount == 0)
                {
                    m_GetScripts.GameManager.GameOver();
                }
                else
                {
                    m_GetScripts.Player.TakeDamageCheck = true;
                    if (null != collision.gameObject.GetComponent<Traps>())
                    {
                        collision.gameObject.GetComponent<Traps>().DisableCollider();
                    }
                }
                break;

            case "Finish":
                m_GetScripts.GameManager.FinishLevel();
                break;
        }
    }
}
