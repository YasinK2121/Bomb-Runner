using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPointParticleTimer : MonoBehaviour
{
    void Start()
    {

    }

    float a = 0;
    void Update()
    {
        a += Time.deltaTime;
        if (a > 1f)
        {
            Destroy(this.gameObject);
        }
    }
}
