using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy{
    public class EnemyAttack : MonoBehaviour
    {
        public GameObject bulletPrefab;
        // Start is called before the first frame update
        private float cooldownTimer = 1f;
        
        void FixedUpdate()
        {
            if (cooldownTimer > 0f)
            {   
                cooldownTimer -= Time.fixedDeltaTime;
            }
        }
        public float FireBullet_8(float cooldownTimer)
        {
            //쿨타임일때 시전 안함
            if (cooldownTimer > 0f){
                return cooldownTimer;
            }

            Debug.Log("fire!");
           
            for (int i = 0; i < 8; i++)
            {
                // 각 방향에 따른 회전 각도
                float rotation = i * 45f;

                // 총알을 회전시켜 생성합니다.
                float radius = 1f; // 반지름 값은 적절히 조정하십시오.

                // 원 주위의 랜덤한 위치 계산
                float spawnX = transform.position.x + radius * Mathf.Cos(rotation * Mathf.Deg2Rad);
                float spawnY = transform.position.y + radius * Mathf.Sin(rotation * Mathf.Deg2Rad);

                // 오브젝트 생성
                GameObject bullet = Instantiate(bulletPrefab, new Vector2(spawnX, spawnY), Quaternion.identity);
                // 총알의 초기 속도 설정
                float bulletSpeed = 10f;
                float bulletDirectionX = Mathf.Cos(Mathf.Deg2Rad * rotation);
                float bulletDirectionY = Mathf.Sin(Mathf.Deg2Rad * rotation);
                Vector2 bulletDirection = new Vector2(bulletDirectionX, bulletDirectionY).normalized;
                bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;
            }
            return 1f;
        }
        
        public float FireBullet_16(float cooldownTimer = 0.5f)
        {
            //쿨타임일때 시전 안함
            if (cooldownTimer > 0f){
                return cooldownTimer;
            }

            Debug.Log("fire!");
           
            for (int i = 0; i < 16; i++)
            {
                // 각 방향에 따른 회전 각도
                float rotation = i * 22.5f;

                // 총알을 회전시켜 생성
                float radius = 1f; // 반지름 값 조정

                // 원 주위의 위치 계산
                float spawnX = transform.position.x + radius * Mathf.Cos(rotation * Mathf.Deg2Rad);
                float spawnY = transform.position.y + radius * Mathf.Sin(rotation * Mathf.Deg2Rad);

                // 오브젝트 생성
                GameObject bullet = Instantiate(bulletPrefab, new Vector2(spawnX, spawnY), Quaternion.identity);
                // 총알의 초기 속도 설정
                float bulletSpeed = 10f;
                float bulletDirectionX = Mathf.Cos(Mathf.Deg2Rad * rotation);
                float bulletDirectionY = Mathf.Sin(Mathf.Deg2Rad * rotation);
                Vector2 bulletDirection = new Vector2(bulletDirectionX, bulletDirectionY).normalized;
                bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;
            }
            return 1f;
        }

        public float FireBullet_8_16(float cooldownTimer = 0.5f){
            if (cooldownTimer > 0f){
                return cooldownTimer;
            }
            FireBullet_8(0f);

            Invoke("FireBullet_16",0.5f);
            
            return 2f;
        }    
    
        // public IEnumerator FireBullet_rapid(float cooldownTimer)
        // {
        //     // 쿨타임일때 시전 안함
        //     if (cooldownTimer > 0f)
        //     {
        //         yield return cooldownTimer;
        //     }

        //     Debug.Log("fire!");

        //     float delayBetweenBullets = 0.5f; // 총알 생성 간격 (0.5초)

        //     for (int i = 0; i < 8; i++)
        //     {
        //         // 각 방향에 따른 회전 각도
        //         float rotation = i * 45f;

        //         // 총알을 회전시켜 생성합니다.
        //         float radius = 1f; // 반지름 값은 적절히 조정하십시오.

        //         // 원 주위의 랜덤한 위치 계산
        //         float spawnX = transform.position.x + radius * Mathf.Cos(rotation * Mathf.Deg2Rad);
        //         float spawnY = transform.position.y + radius * Mathf.Sin(rotation * Mathf.Deg2Rad);

        //         // 오브젝트 생성
        //         GameObject bullet = Instantiate(bulletPrefab, new Vector2(spawnX, spawnY), Quaternion.identity);
        //         // 총알의 초기 속도 설정
        //         float bulletSpeed = 10f;
        //         float bulletDirectionX = Mathf.Cos(Mathf.Deg2Rad * rotation);
        //         float bulletDirectionY = Mathf.Sin(Mathf.Deg2Rad * rotation);
        //         Vector2 bulletDirection = new Vector2(bulletDirectionX, bulletDirectionY).normalized;
        //         bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;

        //         yield return new WaitForSeconds(delayBetweenBullets); // 일정한 간격으로 총알 생성
        //     }

        //     yield return null;
        // }
    }

}
