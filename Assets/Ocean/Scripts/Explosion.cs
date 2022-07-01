using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject ExplosionParticle;
    public GameObject nullobject;
    public Player Player;

    void Start()
    {
        nullobject = Instantiate(ExplosionParticle, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        StartCoroutine(Player.Shake(0.15f, 0.4f));
    }

    float a;
    void Update()
    {
        if (a >= 2f)
        {
            Destroy(nullobject);
            Destroy(this.gameObject);
        }
        a += Time.deltaTime;
        transform.GetComponent<SphereCollider>().radius += Time.deltaTime * 10;
    }
}
