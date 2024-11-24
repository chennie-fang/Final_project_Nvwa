using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 15f;

    private Rigidbody rb;
    public AttackType attackType;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (speed != 0)
        {
            rb.velocity = transform.forward * speed;
            //transform.position += transform.forward * (speed * Time.deltaTime);         
        }
    }
    void OnCollisionEnter(Collision collision)
    {
       if(collision.gameObject.GetComponent<MonsterController>().AcceptAttackType == attackType)
        {
            collision.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
