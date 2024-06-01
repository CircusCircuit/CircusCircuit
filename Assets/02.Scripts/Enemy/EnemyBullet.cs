using Unity.Mathematics;
using UnityEngine;

namespace Enemy
{
    public class EnemyBullet : MonoBehaviour
    {
        Rigidbody2D rigid;
        public float destroyTime = 2f; // 총알이 생성된 후 파괴될 시간
        private float inclination;
        private Vector3 initialPosition; // 초기 위치
        
        void Start()
        {
            initialPosition = transform.position;

            inclination = initialPosition.y/math.pow(initialPosition.x, 2);

            rigid = GetComponent<Rigidbody2D>();

            // 일정 시간 후에 총알을 파괴하는 Invoke 함수 호출
            Invoke("DestroyBullet", destroyTime);
        }


        // 총알이 충돌하면 호출되는 함수
        void OnTriggerEnter2D(Collider2D collision)
        {
            // 충돌한 객체가 플랫폼이면 총알을 파괴합니다.
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                    DestroyBullet();
            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                    DestroyBullet();
            }
        }

        // 총알을 파괴하는 함수
        void DestroyBullet()
        {
            Destroy(gameObject);
        }
    }
}
