using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        protected class Movement{
            protected Rigidbody2D rigid;
            protected SpriteRenderer spriteRenderer;
            private EnemyOneWayPlatform oneWay;

            protected float speed;

            public Movement(Rigidbody2D rigid, SpriteRenderer spriteRenderer, EnemyOneWayPlatform oneway, float speed){
                this.rigid = rigid;
                this.speed = speed;
                this.oneWay = oneway;
                this.spriteRenderer = spriteRenderer;
            }

            public void Move(float speed, int nextmove){
                rigid.velocity = new Vector2(speed*nextmove, rigid.velocity.y);
            }
            public void Stop()
            {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
            }
            public void UpJump(int nextmove)
            {
                Debug.Log("upjump");
                rigid.AddForce(Vector2.up * 25f,ForceMode2D.Impulse);
                rigid.AddForce(Vector2.right * nextmove * 5f,ForceMode2D.Impulse);
            }
            public void DownJump()
            {
                oneWay.DownJump();
                Debug.Log("downjump");
                rigid.AddForce(Vector2.up * 10f,ForceMode2D.Impulse);
            }

        }
        protected class Detection{

            private Enemy enemy;

            public Detection(Enemy enemy){
                this.enemy = enemy;
            }
            public void DetectPlayerInRangeHorizental(float detectionRange = 5f)
            {
                // 플레이어의 위치
                Vector2 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

                // 몬스터와 플레이어의 거리 계산
                float distanceToPlayerX = Mathf.Abs(playerPosition.x - enemy.transform.position.x);
                float distanceToPlayerY = Mathf.Abs(playerPosition.y - enemy.transform.position.y);

                // 감지범위 시각화      
                DebugDrawDetectionRangeHorizental(enemy.transform.position, detectionRange);
                
                
                if (distanceToPlayerY <= 1f)
                {
                    // 플레이어가 몬스터의 왼쪽에 있고 감지 범위 내에 있다면
                    if (enemy.isFacingLeft)
                    {
                        if (playerPosition.x < enemy.transform.position.x && distanceToPlayerX <= detectionRange)
                        {
                            Debug.Log("Player detected on the left!");
                            enemy.isDetectPlayer = true;
                        }
                    }
                    // 플레이어가 몬스터의 오른쪽에 있고 감지 범위 내에 있다면
                    else
                    {
                        if (playerPosition.x > enemy.transform.position.x && distanceToPlayerX <= detectionRange)
                        {
                            Debug.Log("Player detected on the right!");
                            enemy.isDetectPlayer = true;
                        }
                    }
                }
                else
                {
                    // Debug.Log("Player undetected!");
                    enemy.isDetectPlayer = false;
                }
            }

            void DebugDrawDetectionRangeHorizental(Vector2 center, float Width)
            {
                // 사각형 테두리 그리기
                Vector2 topLeft = center + new Vector2(-Width, 0.5f);
                Vector2 topRight = center + new Vector2(Width, 0.5f);
                Vector2 bottomLeft = center + new Vector2(-Width, -0.5f);
                Vector2 bottomRight = center + new Vector2(Width, -0.5f);

                Debug.DrawLine(topLeft, topRight, Color.red);
                Debug.DrawLine(topRight, bottomRight, Color.red);
                Debug.DrawLine(bottomRight, bottomLeft, Color.red);
                Debug.DrawLine(bottomLeft, topLeft, Color.red);
            }
            

        }
        protected class Attack{

            private Enemy enemy;
            public GameObject bulletPrefab;

            public Attack(Enemy enemy, GameObject bulletPrefab){
                this.enemy = enemy;
                this.bulletPrefab = bulletPrefab;                
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
                    float spawnX = enemy.transform.position.x + radius * Mathf.Cos(rotation * Mathf.Deg2Rad);
                    float spawnY = enemy.transform.position.y + radius * Mathf.Sin(rotation * Mathf.Deg2Rad);

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
        }
        
        protected Movement movement;
        protected Detection detection;
        protected Attack attack;


        public GameObject bulletPrefab;


        public float speed = 5f;
        public int nextmove = 1;
        public float cooldownTimer = 1.5f;


        public bool isJump = false;
        public bool isAttack = false;
        public bool isDetectPlayer = false;
        public bool isFacingLeft = false;
        
        protected virtual void Start()
        {
            Rigidbody2D rigid = GetComponent<Rigidbody2D>();
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            EnemyOneWayPlatform oneWay = GetComponent<EnemyOneWayPlatform>();
            movement = new Movement(rigid, spriteRenderer, oneWay, speed);
            detection = new Detection(this);
            attack = new Attack(this, bulletPrefab);
        }

        protected virtual void Update()
        {
            if(cooldownTimer>0){
                cooldownTimer -= Time.deltaTime;
            }
            else{
                if (!isJump){
                    movement.DownJump();
                    isJump = true;
                }
                else{
                    attack.FireBullet_8();
                    cooldownTimer = 1.5f;
                    detection.DetectPlayerInRangeHorizental(5f);
                    // movement.Move(speed, nextmove);
                }
            }
        }
    }
}
