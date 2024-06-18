using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Enemy
{
    public class BoseBulletCopy : MonoBehaviour
    {
        Rigidbody2D rigid;
        float cooldownTimer = 3;
        public float destroyTime = 2f; // 총알이 생성된 후 파괴될 시간
        bool isAttack = false;
        public float speed = 1.0f; // 이동 속도
        public float startTime = 0.0f; // 시작 시간
        public float duration = 5.0f; // 이동하는 시간

        private float inclination;
        private Vector3 initialPosition; // 초기 위치
        // Start is called before the first frame update
        void Start()
        {

            initialPosition = transform.position;

            inclination = initialPosition.y/math.pow(initialPosition.x, 2);

            rigid = GetComponent<Rigidbody2D>();

            // 일정 시간 후에 총알을 파괴하는 Invoke 함수 호출
            // Invoke("DestroyBullet", destroyTime);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.fixedDeltaTime;
            }
            
            if(transform.position.x <= 0.1f && transform.position.y <= 0.1f){
                rigid.position = Vector2.zero;
                if(!isAttack){
                    Invoke("DestroyBullet",1f);
                    isAttack = true;
                }
            }
            else{
                MoveObjectToOrigin();
            }
        }

        // 총알을 이동시키는 함수
        public void MoveObjectToOrigin()
        {
            float y_position = inclination*(math.pow(transform.position.x, 2));
            rigid.position = new Vector2(transform.position.x - 0.1f, y_position);
            // Debug.Log(rigid.position);

        
        }
        // 총알이 충돌하면 호출되는 함수
        void OnTriggerEnter2D(Collider2D other)
        {
            // 충돌한 객체가 플랫폼이면 총알을 파괴합니다.
            if (other.CompareTag("Ground") || !other.isTrigger)
            {
                if (!other.CompareTag("Enemy") && !other.CompareTag("EnemyBullet"))
                {
                    DestroyBullet();
                }
            }
        }

        // 총알을 파괴하는 함수
        void DestroyBullet()
        {
            // enemyAttack.FireBulletSpiral();
            // Destroy(gameObject);
        }

        //패턴 1 저장
        // void DestroyBullet()
        // {
        //     enemyAttack.FireBullet_8();
        //     Destroy(gameObject);
        // }
    }
}
