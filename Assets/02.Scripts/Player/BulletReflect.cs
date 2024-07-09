using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletReflect : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    Vector3 lastVelocity;

    int reflectCount = 0;
    //bool isCollision = false;
    bool firstCollision = false;
    float time;

    bool doReflect = true;

    const int MAXREFLECT = 2;
    const int INTIME = 2;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        lastVelocity = rb.velocity;
        transform.position = transform.parent.transform.position;
    }

    private void Update()
    {
        if (firstCollision)
        {
            time += Time.deltaTime;

            if (time >= INTIME)
            {
                doReflect = false;
            }
        }

        if (reflectCount >= MAXREFLECT)
        {
            doReflect = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (doReflect)
            {
                var speed = lastVelocity.magnitude;
                var dir = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

                rb.velocity = dir * Mathf.Max(speed, 0f);

                firstCollision = true;

                ++reflectCount;
                //isCollision = true;
                time = 0;

            }
        }
    }

}
