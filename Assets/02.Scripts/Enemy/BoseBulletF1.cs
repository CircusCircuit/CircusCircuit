using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Enemy
{
    public class BoseBulletF1 : MonoBehaviour
    {
        Rigidbody2D rigid;
        EnemyAttack enemyAttack;
        float cooldownTimer = 0.5f;
        public float destroyTime = 20f; // 총알이 생성된 후 파괴될 시간
        bool isAttack = false;
        public float speed = 1.0f; // 이동 속도
        public float startTime = 0.0f; // 시작 시간
        public float duration = 5.0f; // 이동하는 시간
        public float amount = 3.0f; // 이동하는 시간

        public float distance = 2.0f; // d 값
        private float t = 0.0f; // 시간 변수
        private float inclination;
        
        private Vector3 initialPosition; // 초기 위치
        // Start is called before the first frame update
        void Start()
        {
            enemyAttack = GetComponent<EnemyAttack>();
            initialPosition = transform.position;
            inclination = initialPosition.y / math.pow(initialPosition.x, 2);
            rigid = GetComponent<Rigidbody2D>();
            // transform.position = Vector2.zero;
            // 일정 시간 후에 총알을 파괴하는 Invoke 함수 호출
            Invoke("DestroyBullet", destroyTime);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Drow();
        }

        public void Drow(){
            t = t+Time.deltaTime;
            float x = amount * Mathf.Sin(t) - distance * Mathf.Sin(amount * t);
            float y = amount * Mathf.Cos(t) + distance * Mathf.Cos(amount * t);

            // 오브젝트 이동
            transform.position = new Vector2(x/1f, y/1.5f);
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

        // 총알을 파괴하는 함수
        void DestroyBullet()
        {
            Destroy(gameObject);
        }

        //패턴 1 저장
        // void DestroyBullet()
        // {
        //     enemyAttack.FireBullet_8();
        //     Destroy(gameObject);
        // }
    }
}
