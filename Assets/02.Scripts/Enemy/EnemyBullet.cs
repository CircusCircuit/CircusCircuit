using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Enemy{
    public class EnemyBullet : MonoBehaviour
    {
        Rigidbody2D rigid;

        float cooldownTimer = 3;
        public float speed;
        public float destroyTime = 2f; // 총알이 생성된 후 파괴될 시간

        // Start is called before the first frame update
        void Start()
        {
            rigid = GetComponent<Rigidbody2D>();
            // 일정 시간 후에 총알을 파괴하는 Invoke 함수 호출
            // Invoke("DestroyBullet", destroyTime);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(cooldownTimer>0){
                cooldownTimer -= Time.fixedDeltaTime;
            }
            if(cooldownTimer <= 0){
                MoveObjectToOrigin();
                // cooldownTimer = 2f;
        
            }
        }

        // 총알을 이동시키는 함수
        public void MoveObjectToOrigin()
        {
            // 타겟 오브젝트의 현재 위치를 얻어옵니다.
            Vector2 currentPosition = transform.position;
            
            // 타겟 오브젝트를 원점으로 이동시킵니다.
            rigid.velocity = new Vector2(-currentPosition.x, -currentPosition.y);
          
            // 이동한 만큼 이동 거리를 출력합니다.
            // Debug.Log(targetObject.name + " 이 " + currentPosition + " 위치에서 (0, 0)으로 이동되었습니다.");
        }
        // 총알이 충돌하면 호출되는 함수
         void OnTriggerEnter2D(Collider2D other)
        {
            // 충돌한 객체가 플랫폼이면 총알을 파괴합니다.
            if (other.CompareTag("Ground") || !other.isTrigger)
            {
                if(!other.CompareTag("Enemy")&&!other.CompareTag("EnemyBullet")){
                    DestroyBullet();
                }
            }
        }

        // 총알을 파괴하는 함수
        void DestroyBullet()
        {
            Destroy(gameObject);
        }
    }
}
