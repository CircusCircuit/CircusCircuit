using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Enemy
{
    public class EnemyBulletBounce : MonoBehaviour
    {
        Rigidbody2D rigid;
        EnemyAttack enemyAttack;
        float cooldownTimer = 3;
        public float destroyTime = 2f; // 총알이 생성된 후 파괴될 시간

        public float speed = 1.0f; // 이동 속도
        public float startTime = 0.0f; // 시작 시간
        public float duration = 5.0f; // 이동하는 시간

        private float inclination;
        private Vector3 initialPosition; // 초기 위치
        // Start is called before the first frame update
        void Start()
        {
            enemyAttack = GetComponent<EnemyAttack>();

            initialPosition = transform.position;

            inclination = initialPosition.y / math.pow(initialPosition.x, 2);

            rigid = GetComponent<Rigidbody2D>();

            // 일정 시간 후에 총알을 파괴하는 Invoke 함수 호출
            // Invoke("DestroyBullet", destroyTime);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
        }

        // 총알이 충돌하면 호출되는 함수
        // void OnTriggerEnter2D(Collider2D other)
        // {
        //     // 충돌한 객체가 플랫폼이면 총알을 파괴합니다.
        //     if (other.CompareTag("Ground") || !other.isTrigger)
        //     {

        //         if (!other.CompareTag("Enemy") && !other.CompareTag("EnemyBullet"))
        //         {
        //             DestroyBullet();
        //         }
        //     }
        // }

        void OnTriggerEnter2D(Collider2D other)
        {
            
            if (other.CompareTag("Ground") || !other.isTrigger)
            {
                Vector2 reflection = Vector2.Reflect(rigid.velocity.normalized, other.ClosestPoint(transform.position) - (Vector2)transform.position).normalized;
                rigid.velocity = reflection * speed;
            }
        }
        // 총알을 파괴하는 함수
        void DestroyBullet()
        {
            Destroy(gameObject);
        }
    }
}
