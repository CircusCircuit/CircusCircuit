using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Enemy
{
    public class BoseBullet : MonoBehaviour
    {
        Rigidbody2D rigid;
        EnemyAttack enemyAttack;
        float cooldownTimer = 3;
        public float destroyTime = 2f; // 총알이 생성된 후 파괴될 시간
        bool isAttack = false;
        public float speed = 1.0f; // 이동 속도
        public float startTime = 0.0f; // 시작 시간
        public float duration = 5.0f; // 이동하는 시간
        public float amount = 5.0f; // 이동하는 시간


        public float distance = 20.0f; // d 값
        private LineRenderer lineRenderer;
        public int resolution = 1000; // 해상도
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

            transform.position = Vector2.zero;
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = resolution;
            // 일정 시간 후에 총알을 파괴하는 Invoke 함수 호출
            // Invoke("DestroyBullet", destroyTime);
        }

        void DrawGraph()
        {
            float step = 2 * Mathf.PI / resolution;
            for (int i = 0; i < resolution; i++)
            {
                float t = i * step;
                float x = amount * Mathf.Sin(t) - distance * Mathf.Sin(amount * t);
                float y = amount * Mathf.Cos(t) + distance * Mathf.Cos(amount * t);
                lineRenderer.SetPosition(i, new Vector3(x, y, 0));
            }
        }
        // Update is called once per frame
        void FixedUpdate()
        {
            
            DrawGraph();

            // 시간 증가
            t += Time.deltaTime * speed;

            // 주어진 수식에 따라 x, y 좌표 계산
            float x = 3 * Mathf.Sin(t) - distance * Mathf.Sin(2/3 * t);
            float y = 3 * Mathf.Cos(t) + distance * Mathf.Cos(2/3 * t);

            // 오브젝트 이동
            // transform.position = new Vector2(x, y);

            // if (cooldownTimer > 0)
            // {
            //     cooldownTimer -= Time.fixedDeltaTime;
            // }

            // if (transform.position.x <= 0.1f && transform.position.y <= 0.1f)
            // {
            //     rigid.position = Vector2.zero;
            //     if (!isAttack)
            //     {
            //         Invoke("DestroyBullet", 1f);
            //         isAttack = true;
            //     }
            // }
            // else
            // {
            //     MoveObjectToOrigin();
            // }
        }

        // 총알을 이동시키는 함수
        public void MoveObjectToOrigin()
        {
            float y_position = inclination * (math.pow(transform.position.x, 2));
            rigid.position = new Vector2(transform.position.x - 0.1f, y_position);
        }

        public void MoveObject()
        {
            float y_position = inclination * (math.pow(transform.position.x, 2));
            rigid.position = new Vector2(transform.position.x - 0.1f, y_position);
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
            // enemyAttack.FireBulletSpiral();
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
