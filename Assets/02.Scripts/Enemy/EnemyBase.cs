using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyBase : MonoBehaviour
    {
        public static EnemyBase Instance = null;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        protected Movement movement;
        protected Detection detection;
        protected Attack attack;
        protected Status status;


        public GameObject bulletPrefab;
        public GameObject G_Bullet;
        protected Rigidbody2D rigid;


        protected float speed = 2f;
        public int nextmove = 1;
        protected float enemyHP = 10;
        protected float cooldownTimer = 1.5f;
        protected int think = 0;


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
            attack = new Attack(this, bulletPrefab, G_Bullet);
            status = gameObject.AddComponent<Status>();
            status.Initialize(this, spriteRenderer, enemyHP);
            Invoke("Think", 0.5f);
        }

        protected virtual void Update()
        {
            detection.DetectPlayerInRange(5);

            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;
            }

            if (isDetectPlayer)
            {
                if (cooldownTimer <= 0)
                {
                    attack.FireBullet();
                    cooldownTimer = 1.5f;
                }
            }
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
            RaycastHit2D rayHitGround = Physics2D.Raycast(frontVec, Vector2.down, 1f, LayerMask.GetMask("Ground"));
            RaycastHit2D rayHitPlatform = Physics2D.Raycast(frontVec, Vector2.down, 1f, LayerMask.GetMask("Platform"));
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
                //Debug.Log(rayHitFoword.collider);
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
                if (isJump)
                {
                    isJump = false;
                }
            }
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == 7)
            {
                status.TakeDamage(0.5f);
            }

        }


        GameObject caughtEnemy = null;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("playerbullet"))
            {
                enemyHP = status.TakeDamage(GameManager.Instance.M_AttackDamage);
            }
            if (other.gameObject.name == "BuffCollider")
            {
                caughtEnemy = this.gameObject;
                PlayerBuff.Instance.EnemyDebuffSkill(caughtEnemy);
            }
            else caughtEnemy = null;
        }
    }
    public class Movement
    {
        private EnemyBase enemy;
        protected Rigidbody2D rigid;
        protected SpriteRenderer spriteRenderer;
        private EnemyOneWayPlatform oneWay;

        protected float speed;
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
            rigid.velocity = new Vector2(0, 0);
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
            float knockbackForce = 5f;

            Debug.Log("knockback");
            // Debug.Log(direction);

            rigid.velocity = Vector2.zero;
            rigid.AddForce(-direction * knockbackForce, ForceMode2D.Impulse);
            rigid.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);

        }
        public void Dash(float moveSpeed = 5f)
        {
            Debug.Log("Dash!");

            if (enemy.isFacingLeft)
            {
                enemy.nextmove = -1;
            }
            else
            {
                enemy.nextmove = 1;
            }
            rigid.velocity = new Vector2(enemy.nextmove * moveSpeed, rigid.velocity.y);
        }

        public void MoveVertical(float Speed)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, enemy.nextmove * Speed);
        }


        public void Fly(Vector2 startPosition, float moveSpeed = 2f, float maxFlyDistance = 5f)
        {

            // 일정 범위 내에서 위아래로 이동하기 위한 코드 추가
            float maxY = startPosition.y + maxFlyDistance;
            float minY = startPosition.y - maxFlyDistance;
            // 현재 위치가 일정 범위를 벗어나면 방향을 바꿔줍니다.
            if (enemy.transform.position.y >= maxY || enemy.transform.position.y <= minY)
            {
                enemy.nextmove *= -1;
            }
            rigid.velocity = new Vector2(rigid.velocity.x, enemy.nextmove * moveSpeed);
        }

    }
    public class Detection
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
        public void DetectPlayerInRangeVertical(float detectionRange = 5f)
        {
            // 플레이어의 위치
            Vector2 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

            // 몬스터와 플레이어의 거리 계산
            float distanceToPlayerX = Mathf.Abs(playerPosition.x - enemy.transform.position.x);
            float distanceToPlayerY = Mathf.Abs(playerPosition.y - enemy.transform.position.y);

            // 감지범위 시각화      
            DebugDrawDetectionRangeVertical(enemy.transform.position, detectionRange);


            if (distanceToPlayerX <= 1f)
            {
                // 플레이어가 몬스터의 아래 쪽에 있을 때
                if (playerPosition.y < enemy.transform.position.y && distanceToPlayerY <= detectionRange)
                {
                    Debug.Log("Player detected below!!");
                    enemy.isDetectPlayer = true;
                    enemy.nextmove = -1;
                }

                // 플레이어가 몬스터의 위쪽에 있을 때
                else
                {
                    if (playerPosition.y > enemy.transform.position.y && distanceToPlayerY <= detectionRange)
                    {
                        Debug.Log("Player detected above!!");
                        enemy.isDetectPlayer = true;
                        enemy.nextmove = 1;
                    }
                }
            }
            else
            {
                // Debug.Log("Player undetected!");
                enemy.isDetectPlayer = false;
            }
        }
        public void DetectPlayerInRange(float detectionRange = 5f)
        {
            // 플레이어의 위치
            Vector2 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

            // 몬스터와 플레이어의 거리 계산
            float distanceToPlayerX = Mathf.Abs(playerPosition.x - enemy.transform.position.x);
            float distanceToPlayerY = Mathf.Abs(playerPosition.y - enemy.transform.position.y);
            float distance = Mathf.Sqrt(distanceToPlayerX * distanceToPlayerX + distanceToPlayerY * distanceToPlayerY);

            // 감지범위 시각화      
            DebugDrawDetectionRange(enemy.transform.position, detectionRange);

            // 만약 플레이어가 감지 범위 내에 있다면
            if (distance <= detectionRange)
            {
                enemy.isDetectPlayer = true;
                Debug.Log("Player detected!");
            }
            else
            {
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
        void DebugDrawDetectionRangeVertical(Vector2 center, float Width)
        {
            // 사각형 테두리 그리기
            Vector2 topLeft = center + new Vector2(-0.5f, Width);
            Vector2 topRight = center + new Vector2(0.5f, Width);
            Vector2 bottomLeft = center + new Vector2(-0.5f, -Width);
            Vector2 bottomRight = center + new Vector2(0.5f, -Width);

            Debug.DrawLine(topLeft, topRight, Color.red);
            Debug.DrawLine(topRight, bottomRight, Color.red);
            Debug.DrawLine(bottomRight, bottomLeft, Color.red);
            Debug.DrawLine(bottomLeft, topLeft, Color.red);
        }
        void DebugDrawDetectionRange(Vector2 center, float radius, int segments = 20)
        {
            float angleStep = 2 * Mathf.PI / segments;

            Vector2 prevPoint = center + new Vector2(radius, 0);

            for (int i = 1; i <= segments; i++)
            {
                float angle = i * angleStep;
                Vector2 nextPoint = center + new Vector2(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
                Debug.DrawLine(prevPoint, nextPoint, Color.red);
                prevPoint = nextPoint;
            }

            // 마지막 점에서 첫 번째 점으로 선 그리기
            Vector2 firstPoint = center + new Vector2(radius, 0);
            Debug.DrawLine(prevPoint, firstPoint, Color.red);
        }

    }
    public class Attack
    {

        protected EnemyBase enemy;
        public GameObject bulletPrefab;
        public GameObject G_Bullet;
        protected bool isDelay = false;

        public Attack(EnemyBase enemy, GameObject bulletPrefab, GameObject G_Bullet)
        {
            this.enemy = enemy;
            this.bulletPrefab = bulletPrefab;
            this.G_Bullet = G_Bullet;
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
                GameObject bullet = GameObject.Instantiate(bulletPrefab, new Vector2(spawnX, spawnY), Quaternion.identity);
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
                float spawnX = enemy.transform.position.x + radius * Mathf.Cos(rotation * Mathf.Deg2Rad);
                float spawnY = enemy.transform.position.y + radius * Mathf.Sin(rotation * Mathf.Deg2Rad);

                // 오브젝트 생성
                GameObject bullet = GameObject.Instantiate(bulletPrefab, new Vector2(spawnX, spawnY), Quaternion.identity);
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
            enemy.StartCoroutine(CountAttackDelay());
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


        public void FireBullet()
        {

            GameObject player = GameObject.FindGameObjectWithTag("Player");

            Vector2 directionToPlayer = (player.transform.position - enemy.transform.position).normalized;


            float radius = 1f; // 반지름 값은 적절히 조정하십시오.

            float spawnX = enemy.transform.position.x + directionToPlayer.x * radius;
            float spawnY = enemy.transform.position.y + directionToPlayer.y * radius;

            GameObject bullet = GameObject.Instantiate(bulletPrefab, new Vector2(spawnX, spawnY), Quaternion.identity);

            float bulletSpeed = 10f;
            Vector2 bulletDirection = directionToPlayer;
            bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;
        }

        public void FireBullet_Rapid()
        {
            enemy.StartCoroutine(FireBulletCoroutine());
        }
        IEnumerator FireBulletCoroutine()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            // 플레이어가 없으면 코루틴을 종료합니다.
            if (player == null)
            {
                Debug.LogWarning("Player object not found.");
                yield break;
            }

            Vector2 directionToPlayer = (player.transform.position - enemy.transform.position).normalized;

            for (int i = 0; i < 8; i++)
            {
                float radius = 1f; // 반지름 값은 적절히 조정하십시오.

                float spawnX = enemy.transform.position.x + directionToPlayer.x * radius;
                float spawnY = enemy.transform.position.y + directionToPlayer.y * radius;

                GameObject bullet = GameObject.Instantiate(bulletPrefab, new Vector2(spawnX, spawnY), Quaternion.identity);

                float bulletSpeed = 10f;
                Vector2 bulletDirection = directionToPlayer;
                bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;

                yield return new WaitForSeconds(0.1f);
            }
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
                float spawnX = enemy.transform.position.x + radius * Mathf.Cos(rotation * Mathf.Deg2Rad);
                float spawnY = enemy.transform.position.y + radius * Mathf.Sin(rotation * Mathf.Deg2Rad);

                // 오브젝트 생성
                GameObject bullet = GameObject.Instantiate(bulletPrefab, new Vector2(spawnX, spawnY), Quaternion.identity);
                // 총알의 초기 속도 설정
                float randomVelocity = Random.Range(5, 10);
                float bulletSpeed = randomVelocity;
                float bulletDirectionX = Mathf.Cos(Mathf.Deg2Rad * rotation);
                float bulletDirectionY = Mathf.Sin(Mathf.Deg2Rad * rotation);
                Vector2 bulletDirection = new Vector2(bulletDirectionX, bulletDirectionY).normalized;
                bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;
            }
        }

        public void FireBullet_area()
        {
            enemy.StartCoroutine(FireBulletAreaCoroutine());
        }
        IEnumerator FireBulletAreaCoroutine()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector2 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

            for (int i = 0; i < 20; i++)
            {
                // 각 방향에 따른 회전 각도
                float val = Random.Range(0, 45);
                float rotation = val;
                if (playerPosition.x > enemy.transform.position.x)
                {
                    rotation -= 65;
                }
                else
                {
                    rotation -= 155;
                }
                // 총알을 회전시켜 생성합니다.
                float radius = 1f; // 반지름 값은 적절히 조정하십시오.

                // 원 주위의 랜덤한 위치 계산
                float spawnX = enemy.transform.position.x + radius * Mathf.Cos(rotation * Mathf.Deg2Rad);
                float spawnY = enemy.transform.position.y + radius * Mathf.Sin(rotation * Mathf.Deg2Rad);

                // 오브젝트 생성
                GameObject bullet = GameObject.Instantiate(G_Bullet, new Vector2(spawnX, spawnY), Quaternion.identity);
                // 총알의 초기 속도 설정
                float bulletSpeed = 5f;
                float bulletDirectionX = Mathf.Cos(Mathf.Deg2Rad * rotation);
                float bulletDirectionY = Mathf.Sin(Mathf.Deg2Rad * rotation);
                Vector2 bulletDirection = new Vector2(bulletDirectionX, bulletDirectionY).normalized;
                bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;
                yield return new WaitForSeconds(0.05f);
            }
        }

        public void FireBulletSpiral()
        {
            enemy.StartCoroutine(FireBulletSpiralCoroutine());
        }
        IEnumerator FireBulletSpiralCoroutine()
        {

            for (int i = 0; i < 66; i++)
            {
                // 각 방향에 따른 회전 각도
                float rotation = i * 10f;
                float rotation2 = rotation + 180f;

                // 총알을 회전시켜 생성합니다.
                float radius = 0.5f; // 반지름 값은 적절히 조정하십시오.

                // 원 주위의 랜덤한 위치 계산
                float spawnX = enemy.transform.position.x + radius * Mathf.Cos(rotation * Mathf.Deg2Rad);
                float spawnY = enemy.transform.position.y + radius * Mathf.Sin(rotation * Mathf.Deg2Rad);
                float spawnX2 = enemy.transform.position.x + radius * Mathf.Cos(rotation2 * Mathf.Deg2Rad);
                float spawnY2 = enemy.transform.position.y + radius * Mathf.Sin(rotation2 * Mathf.Deg2Rad);
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
    public class Status : MonoBehaviour
    {
        public static Status EnemyInstance = null;
        private void Awake()
        {
            if (EnemyInstance == null)
            {
                EnemyInstance = this;
            }
        }

        private EnemyBase enemy;
        private SpriteRenderer spriteRenderer;
        private float enemyHP;

        public void Initialize(EnemyBase enemyBase, SpriteRenderer spriteRenderer, float initialHP)
        {
            this.enemy = enemyBase;
            this.spriteRenderer = spriteRenderer;
            this.enemyHP = initialHP;
        }

        public float TakeDamage(float damage)
        {
            // Debug.Log(":(");
            if (enemyHP <= 3)
            {
                PlayerBuff.Instance.Magician1Skill();
            }

            enemyHP -= damage;
            enemy.StartCoroutine(AttackedEffect());

            if (enemyHP <= 0)
            {
                Die();
            }
            return enemyHP;
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
            GameObject.Destroy(enemy.gameObject);
        }
    }
}
