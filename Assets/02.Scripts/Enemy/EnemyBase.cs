using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyBase : MonoBehaviour
    {
        protected class Movement
        {
            private EnemyBase enemy;
            protected Rigidbody2D rigid;
            protected SpriteRenderer spriteRenderer;
            private EnemyOneWayPlatform oneWay;

            protected float speed;
            protected int think;

            public Movement(EnemyBase enemy, Rigidbody2D rigid, SpriteRenderer spriteRenderer, EnemyOneWayPlatform oneway, float speed)
            {
                this.enemy = enemy;
                this.rigid = rigid;
                this.speed = speed;
                this.oneWay = oneway;
                this.spriteRenderer = spriteRenderer;
            }

            public void Move(float speed)
            {
                rigid.velocity = new Vector2(speed * enemy.nextmove, rigid.velocity.y);
            }
            public void Stop()
            {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
                enemy.nextmove = 0;
            }
            public void UpJump()
            {
                Debug.Log("upjump");
                rigid.AddForce(Vector2.up * 20f, ForceMode2D.Impulse);
                rigid.AddForce(Vector2.right * enemy.nextmove * 5f, ForceMode2D.Impulse);
            }
            public void DownJump()
            {
                oneWay.DownJump();
                Debug.Log("downjump");
                rigid.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
            }

            public void Flip()
            {
                spriteRenderer.flipX = enemy.isFacingLeft == true;
                enemy.isFacingLeft = !enemy.isFacingLeft;
            }

            public void Turn()
            {
                enemy.nextmove = enemy.nextmove * -1;
                spriteRenderer.flipX = enemy.isFacingLeft == true;
                enemy.isFacingLeft = !enemy.isFacingLeft;
            }

            public void Knockback(Vector2 direction)
            {
                rigid.constraints |= RigidbodyConstraints2D.FreezePositionX;

                float knockbackForce = 15f;

                Debug.Log("knockback");
                Debug.Log(direction);


                rigid.velocity = Vector2.zero;
                rigid.AddForce(-direction * knockbackForce, ForceMode2D.Impulse);
                rigid.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);

            }

        }
        protected class Detection
        {

            private EnemyBase enemy;

            public Detection(EnemyBase enemy)
            {
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
        protected class Attack
        {

            private EnemyBase enemy;
            public GameObject bulletPrefab;

            public Attack(EnemyBase enemy, GameObject bulletPrefab)
            {
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
        protected class Status
        {
            private EnemyBase enemy;
            private SpriteRenderer spriteRenderer;
            private float EnemyHP;

            public Status(EnemyBase enemy, SpriteRenderer spriteRenderer, float EnemyHP)
            {
                this.enemy = enemy;
                this.spriteRenderer = spriteRenderer;
                this.EnemyHP = EnemyHP;
            }

            public void TakeDamage(float damage)
            {
                Debug.Log(":(");
                EnemyHP -= damage;
                enemy.StartCoroutine(AttackedEffect());
                if (EnemyHP <= 0)
                {
                    Die();
                }
            }
            private void Die()
            {
                // enemyMove.isDying = true;
                enemy.StartCoroutine(ShrinkAndDestroy());
            }

            IEnumerator AttackedEffect()
            {
                for (int i = 0; i < 3; i++)
                {
                    spriteRenderer.color = new Color32(243, 114, 114, 255);
                    yield return new WaitForSeconds(0.1f);

                    spriteRenderer.color = new Color32(255, 255, 255, 255);
                    yield return new WaitForSeconds(0.1f);
                }
            }
            IEnumerator ShrinkAndDestroy()
            {
                // 시작 스프라이트 크기
                Vector2 originalScale = enemy.transform.localScale;

                // 스프라이트 크기를 줄여가면서 점진적으로 사라지게 함
                for (float t = 0.5f; t >= 0; t -= 2 * Time.deltaTime)
                {
                    enemy.transform.localScale = originalScale * t;
                    yield return null;
                }

                // 스프라이트가 완전히 사라진 후 게임 오브젝트를 파괴
                Destroy(enemy.gameObject);
            }
        }

        protected Movement movement;
        protected Detection detection;
        protected Attack attack;
        protected Status status;


        public GameObject bulletPrefab;
        Rigidbody2D rigid;


        public float speed = 2f;
        public int nextmove = 1;
        public int enemyHP = 10;
        public float cooldownTimer = 1.5f;
        public int think = 0;


        public bool isDying = false;
        public bool isKnockback = false;
        public bool isJump = false;
        public bool isAttack = false;
        public bool isDetectPlayer = false;
        public bool isFacingLeft = false;

        protected virtual void Start()
        {
            rigid = GetComponent<Rigidbody2D>();
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            EnemyOneWayPlatform oneWay = GetComponent<EnemyOneWayPlatform>();
            movement = new Movement(this, rigid, spriteRenderer, oneWay, speed);
            detection = new Detection(this);
            attack = new Attack(this, bulletPrefab);
            status = new Status(this, spriteRenderer, enemyHP);
            // Think();
        }

        protected virtual void Update()
        {
            // if (!isDying)
            // {
            //     if (!isKnockback)
            //     {
            //         if (!isDetectPlayer)
            //         {
            //             GroundMove(2f);
            //             detection.DetectPlayerInRangeHorizental(5f);
            //         }
            //         else
            //         {
            //             // Dash(10f);
            //         }
            //     }
            // }
            GroundMove(speed);
        }

        public void Think()
        {
            think = Random.Range(-1, 2);
            if (think != 0)
            {
                nextmove = think;
                if (think > 0)
                {
                    if (isFacingLeft)
                    {
                        movement.Flip();
                    }
                }
                else
                {
                    if (!isFacingLeft)
                    {
                        movement.Flip();
                    }
                }
            }
            else
            {
                movement.Stop();
                attack.FireBullet_8();
            }
            Invoke("Think", 3);
        }

        public void GroundMove(float moveSpeed)
        {
            Vector2 frontVec = new Vector2(rigid.position.x + nextmove * 0.2f, rigid.position.y);
            Vector2 downVec = new Vector2(rigid.position.x - 0.5f, rigid.position.y - 0.7f);
    
            Debug.DrawRay(frontVec, Vector2.down, new Color(1, 0, 0));
            Debug.DrawRay(frontVec, Vector2.right, new Color(0, 1, 0)); 
            Debug.DrawRay(downVec, Vector2.right, new Color(0, 0, 1)); 
            
            // Raycast를 실행합니다.
            RaycastHit2D rayHitGround = Physics2D.Raycast(frontVec, Vector2.down, 1f,LayerMask.GetMask("Ground")); 
            RaycastHit2D rayHitPlatform = Physics2D.Raycast(frontVec, Vector2.down, 1f,LayerMask.GetMask("Platform")); 
            RaycastHit2D rayHitFoword = Physics2D.Raycast(frontVec, Vector2.right * nextmove * 0.1f, 0.3f, LayerMask.GetMask("Ground")); 
            RaycastHit2D rayHitEnemy = Physics2D.Raycast(downVec, Vector2.right, 1f, LayerMask.GetMask("Enemy"));

            // 점프 중이 아니거나 앞에 몬스터가 아래 있으면 이동
            if (!isJump || rayHitEnemy.collider != null)
            {
                movement.Move(moveSpeed);
            }

            //앞에 벽 감지시 돌아서 이동
            if (rayHitFoword.collider != null)
            {
                Debug.Log(rayHitFoword.collider);
                movement.Turn();
                CancelInvoke("Think");
                Invoke("Think", 2);
            }

            // 낭떨어지 만났을 때 점프 혹은 뒤돌기
         
            if (rayHitGround.collider == null && rayHitPlatform.collider == null && !isJump)
            {

                // 50% 확률
                // 뒤돌기
                if (Random.value < 0.5)
                {
                    movement.Turn();
                    CancelInvoke("Think");
                    Invoke("Think", 2);
                }
                // 점프
                else
                {
                    isJump = true;
                    if (Random.value < 0.5)
                    {
                        movement.UpJump();
                        CancelInvoke("Think");
                        Invoke("Think", 2);
                    }
                    else
                    {
                        movement.DownJump();
                        CancelInvoke("Think");
                        Invoke("Think", 2);
                    }
                }
            }

            // 착지 후 점프 종료
            if (rayHitGround.collider != null || rayHitPlatform.collider != null)
            {
                if(isJump){
                    isJump = false;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == 7)
            {
                status.TakeDamage(0.5f);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("playerbullet"))
            {
                status.TakeDamage(GameManager.Instance.M_AttackDamage);
            }
        }
    }
}
