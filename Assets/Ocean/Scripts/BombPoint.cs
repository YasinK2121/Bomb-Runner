using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPoint : MonoBehaviour
{
    [SerializeField] GetScripts m_GetScripts;
    [SerializeField] GameObject Particle;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {   
            m_GetScripts.Player.WindSpeed = true;
            m_GetScripts.Player.WindBombCount++;
            m_GetScripts.InGameCanvasManager.BombPowerRedBar.fillAmount += 1f / m_GetScripts.GameManager.PowerBombCount;
            Instantiate(Particle, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
