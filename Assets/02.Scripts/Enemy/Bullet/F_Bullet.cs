using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Enemy
{
    public class F_Bullet : E_Bullet
    {
        float cooldownTimer = 0.5f;
        bool isAttack = false;
        public GameObject bulletPrefab;

        private float inclination;
        
        private Vector3 initialPosition; // 초기 위치
        // Start is called before the first frame update
        protected override void Start()
        {
            destroyTime = 10f;
            base.Start();
            initialPosition = transform.position;
            inclination = initialPosition.y / math.pow(initialPosition.x, 2);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(cooldownTimer<0){
                MoveObjectToOrigin();
            }
            else{
                if(transform.position.x <= 0.1f && transform.position.y <= 0.1f){
                    rigid.position = Vector2.zero;

                    if(!isAttack){
                        Invoke("DestroyBullet",3f);
                        isAttack = true;
                    }
                }
                else{
                    MoveObjectToOrigin();
                }
            }   
        }

        public void MoveObjectToOrigin()
        {
            float y_position = inclination * (math.pow(transform.position.x, 2));
            rigid.position = new Vector2(transform.position.x - 0.1f, y_position);
        }

        //패턴 1 저장
        // void DestroyBullet()
        // {
        //     enemyAttack.FireBullet_8();
        //     Destroy(gameObject);
        // }

        public void FireBulletSpiral()
        {
            StartCoroutine(FireBulletSpiralCoroutine());
        }
        IEnumerator FireBulletSpiralCoroutine()
        {

            for (int i = 0; i < 66; i++)
            {
                // 각 방향에 따른 회전 각도
                float rotation = i * 10f;
                float rotation2 = rotation +180f;

                // 총알을 회전시켜 생성합니다.
                float radius = 0.5f; // 반지름 값은 적절히 조정하십시오.

                // 원 주위의 랜덤한 위치 계산
                float spawnX = transform.position.x + radius * Mathf.Cos(rotation * Mathf.Deg2Rad);
                float spawnY = transform.position.y + radius * Mathf.Sin(rotation * Mathf.Deg2Rad);
                float spawnX2 = transform.position.x + radius * Mathf.Cos(rotation2 * Mathf.Deg2Rad);
                float spawnY2 = transform.position.y + radius * Mathf.Sin(rotation2 * Mathf.Deg2Rad);
                // 오브젝트 생성
                GameObject bullet = GameObject.Instantiate(bulletPrefab, new Vector2(spawnX, spawnY), Quaternion.identity);
                GameObject bullet2 = GameObject.Instantiate(bulletPrefab, new Vector2(spawnX2, spawnY2), Quaternion.identity);

                // 총알의 초기 속도 설정
                float bulletSpeed = 5f;
                float bulletDirectionX = Mathf.Cos(Mathf.Deg2Rad * rotation);
                float bulletDirectionY = Mathf.Sin(Mathf.Deg2Rad * rotation);
                Vector2 bulletDirection = new Vector2(bulletDirectionX, bulletDirectionY).normalized;
                float bulletDirectionX2 = Mathf.Cos(Mathf.Deg2Rad * rotation2);
                float bulletDirectionY2 = Mathf.Sin(Mathf.Deg2Rad * rotation2);
                Vector2 bulletDirection2 = new Vector2(bulletDirectionX2, bulletDirectionY2).normalized;
                bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;
                bullet2.GetComponent<Rigidbody2D>().velocity = bulletDirection2 * bulletSpeed;
                yield return new WaitForSeconds(0.05f);
            }
  
        }
    }
}
