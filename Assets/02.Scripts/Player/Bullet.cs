using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
namespace Player
{
    public class Bullet : MonoBehaviour
    {
        private Vector3 mousePos;
        private Camera mainCam;

        Rigidbody2D rb;
        float force = 15;

        // Start is called before the first frame update
        void Start()
        {
            mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            rb = GetComponent<Rigidbody2D>();
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

            Vector3 direction = mousePos - transform.position;
            Vector3 rotation = transform.position - mousePos;

            rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
            float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, rot + 90);
        }

        // Update is called once per frame
        void Update()
        {
            //rb.velocity = new Vector2(speed * transform.localScale.x, 0);
        }


        //화면밖으로 나가면 destory
        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            //print(collision.gameObject.name);

            if (collision.gameObject.layer == 8)
            {
                //print("layer는 ground");

                Destroy(gameObject);
            }
            if (collision.gameObject.CompareTag("Enemy"))
            {
                EnemyStatus enemyStatus = collision.gameObject.GetComponent<EnemyStatus>();
                if (enemyStatus != null)
                {
                    enemyStatus.TakeDamage(5);
                }
                Destroy(gameObject);
            }
        }
        
    }
}

