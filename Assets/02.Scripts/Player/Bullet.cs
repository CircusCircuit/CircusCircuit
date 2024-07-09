using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
namespace Player
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] Sprite[] bulletSkin;
        private Vector3 mousePos;
        private Camera mainCam;

        Rigidbody2D rb;
        float force = 15;

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<SpriteRenderer>().sortingOrder = 5;

            mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            rb = GetComponent<Rigidbody2D>();
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

            Vector3 direction = mousePos - transform.position;
            Vector3 rotation = transform.position - mousePos;

            rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
            float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, rot + 90);

            GetComponent<SpriteRenderer>().sprite = bulletSkin[Random.Range(0, 4)];
        }

        // Update is called once per frame
        void Update()
        {

        }


        //화면밖으로 나가면 destory
        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                EnemyBase enemybase = collision.gameObject.GetComponent<EnemyBase>();
                Status enemystatus = collision.gameObject.GetComponent<Status>();
                if (enemybase != null)
                {
                    enemystatus.TakeDamage(GameManager.Instance.M_AttackDamage);
                    print(GameManager.Instance.M_AttackDamage);
                }
                Destroy(gameObject);
            }
        }
    }
}

