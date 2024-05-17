using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class BossAttack : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public GameObject dBullet;
        public GameObject fwBullet;
        public GameObject bBullet;
        // private EnemyMove enemyMove;
        public bool isDelay;
        // Start is called before the first frame update

        IEnumerator FireBulletCoroutine()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            // 플레이어가 없으면 코루틴을 종료합니다.
            if (player == null)
            {
                Debug.LogWarning("Player object not found.");
                yield break;
            }

            Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;

            for (int i = 0; i < 35; i++)
            {
                float radius = 1f; // 반지름 값은 적절히 조정하십시오.

                float spawnX = transform.position.x + directionToPlayer.x * radius;
                float spawnY = transform.position.y + directionToPlayer.y * radius;

                GameObject bullet = Instantiate(dBullet, new Vector2(spawnX, spawnY), Quaternion.identity);

                float bulletSpeed = 10f;
                Vector2 bulletDirection = directionToPlayer;
                bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;

                yield return new WaitForSeconds(0.2f);
            }

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
                GameObject bullet = Instantiate(bulletPrefab, new Vector2(spawnX, spawnY), Quaternion.identity);
                GameObject bullet2 = Instantiate(bulletPrefab, new Vector2(spawnX2, spawnY2), Quaternion.identity);

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
        public void FireBulletSpiral()
        {
            StartCoroutine(FireBulletSpiralCoroutine());
        }


    
        public void FireBulletFW()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;

            float radius = 1f; // 반지름 값은 적절히 조정하십시오.

            float spawnX = transform.position.x + directionToPlayer.x * radius;
            float spawnY = transform.position.y + directionToPlayer.y * radius;

            GameObject bullet = Instantiate(bulletPrefab, new Vector2(spawnX, spawnY), Quaternion.identity);

            float bulletSpeed = 3f;
            Vector2 bulletDirection = directionToPlayer;
            bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;

        }
        public void FireBullet_Rapid()
        {
            StartCoroutine(FireBulletCoroutine());
        }
        public void FireBullet_Circle12()
        {
            Debug.Log("fire!");

            for (int i = 0; i < 12; i++)
            {
                // 각 방향에 따른 회전 각도
                float randomVector = Random.Range(0, 360);
                float rotation = randomVector;

                // 총알을 회전시켜 생성합니다.
                float radius = 1f; // 반지름 값은 적절히 조정하십시오.

                // 원 주위의 랜덤한 위치 계산
                float spawnX = transform.position.x + radius * Mathf.Cos(rotation * Mathf.Deg2Rad);
                float spawnY = transform.position.y + radius * Mathf.Sin(rotation * Mathf.Deg2Rad);

                // 오브젝트 생성
                GameObject bullet = Instantiate(fwBullet, new Vector2(spawnX, spawnY), Quaternion.identity);
                // 총알의 초기 속도 설정
                float randomVelocity = Random.Range(5, 10);
                float bulletSpeed = randomVelocity;
                float bulletDirectionX = Mathf.Cos(Mathf.Deg2Rad * rotation);
                float bulletDirectionY = Mathf.Sin(Mathf.Deg2Rad * rotation);
                Vector2 bulletDirection = new Vector2(bulletDirectionX, bulletDirectionY).normalized;
                bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;
            }

        }
        public void FireBullet_8()
        {
            // Debug.Log("fire!");

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

        }
        public void FireBullet_16()
        {
            // Debug.Log("fire!");

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
        }
        public void FireBullet_8_16()
        {
            FireBullet_8();
            isDelay = true;
            StartCoroutine(CountAttackDelay());
            if (!isDelay)
            {
                FireBullet_16();
            }
        }
        public IEnumerator CountAttackDelay()
        {
            yield return new WaitForSeconds(0.5f);
            isDelay = false;
        }

    }
}
