using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;

namespace Enemy
{
    public class EnemyTest : MonoBehaviour
    {
        Rigidbody2D rigid;
        SpriteRenderer spriteRenderer;
        private EnemyAttack enemyAttack;

        public bool isFire = false;
        private bool isDetectPlayer = false;
        private bool isJump = false;
        public bool isFly = false;
        public bool isGround = false;
        public bool isDying = false;

        public bool isAttack = false;
        private bool isFacingLeft = true;
        private bool isKnockback = false;
        public int nextmove = 1;
        public float cooldownTimer = 3f;
        // public float dashDuration = 0.5f;
        public float dashSpeed = 10f;
        Vector2 startPosition; // 시작 위치


        // Start is called before the first frame update
        void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            enemyAttack = GetComponent<EnemyAttack>();
            // Invoke("Think", 1);
            startPosition = transform.position;

        }

        // Update is called once per frame
        void FixedUpdate()
        {
      
                if (cooldownTimer > 0f)
                {
                    cooldownTimer -= Time.deltaTime;
                }
                if(cooldownTimer<=0f){
                    enemyAttack.FireBullet_Circle12();
                    cooldownTimer = 5f;
                }

               
        }

        void Fly(float moveSpeed = 2f)
        {
            float maxDistance = 0.75f; // 오브젝트와 중앙 사이의 최대 거리

            isFly = true;

            // 일정 범위 내에서 위아래로 이동하기 위한 코드 추가
            float maxY = startPosition.y + maxDistance;
            float minY = startPosition.y - maxDistance;

            // 현재 위치가 일정 범위를 벗어나면 방향을 바꿔줍니다.
            if (transform.position.y >= maxY || transform.position.y <= minY)
            {
                nextmove *= -1;
            }

            // 이동 속도 적용
            if (!isAttack)
            {
                rigid.velocity = new Vector2(0, nextmove * moveSpeed);
            }
            else
            {
                rigid.velocity = new Vector2(0, 0);
            }
        }
        public void Think()
        {

            if (!isFly)
            {
                nextmove = Random.Range(-1, 2);
                if (nextmove != 0)
                {
                    spriteRenderer.flipX = nextmove == 1;
                    isFacingLeft = !isFacingLeft;
                }
                else
                {
                    enemyAttack.FireBullet_8();
                }
            }
            else
            {
                if (Random.value < 0.4)
                {
                    if (cooldownTimer <= 0f)
                    {
                        isAttack = true;
                        enemyAttack.FireBullet_8();
                        cooldownTimer = 2f;
                    }
                }
            }

            Invoke("Think", 3);
        }

        void Turn()
        {
            nextmove = nextmove * -1;
            spriteRenderer.flipX = nextmove == 1;
            isFacingLeft = !isFacingLeft;
        }
        void UpJump()
        {
            Debug.Log("upjump");
            rigid.velocity = new Vector2(rigid.velocity.x * 3f, 25f);
            isJump = true;
        }
        void DownJump()
        {
            Debug.Log("upjump");
            rigid.velocity = new Vector2(rigid.velocity.x * 2f, 10f);
            isJump = true;
            // Debug.Log("isJump"+isJump);
        }
        void Move(float moveSpeed = 2f)
        {
            Vector2 frontVec = new Vector2(rigid.position.x + nextmove * 0.2f, rigid.position.y);
            Vector3 downVec = new Vector2(rigid.position.x - 0.5f , rigid.position.y - 0.7f);
            Debug.DrawRay(frontVec, Vector3.down, new Color(1, 0, 0));
            Debug.DrawRay(frontVec, Vector2.right * nextmove * 0.3f, new Color(0, 1, 0));
            Debug.DrawRay(downVec, Vector3.right, new Color(0, 0, 1));

            
            RaycastHit2D rayHitDown = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));
            RaycastHit2D rayHitFoword = Physics2D.Raycast(frontVec, Vector2.right * nextmove * 0.1f, 0.3f, LayerMask.GetMask("Wall"));
            RaycastHit2D rayHitEnemy = Physics2D.Raycast(downVec, Vector3.right, 1f, LayerMask.GetMask("Enemy"));


            if (!isJump || rayHitEnemy.collider != null)
            {
                rigid.velocity = new Vector2(nextmove * moveSpeed, 0);
            }

            
            if (rayHitDown.collider == null && !isJump)
            {
                if (Random.value < 0.5)
                {
                    Turn();
                    CancelInvoke("Think");
                    Invoke("Think", 2);
                }
                else
                {
                    if (Random.value < 0.7)
                    {
                        UpJump();
                        CancelInvoke("Think");
                        Invoke("Think", 2);
                    }
                    else
                    {
                        DownJump();
                        CancelInvoke("Think");
                        Invoke("Think", 2);
                    }
                }
            }
            if (rayHitDown.collider != null)
            {
                rigid.velocity = new Vector2(nextmove * moveSpeed, rigid.velocity.y * 0.3f);
                isJump = false;
            }
            if (rayHitFoword.collider != null )
            {
                Turn();
                CancelInvoke("Think");
                Invoke("Think", 2);
            }


        }
        public void Dash(float dashSpeed = 10f)
        {
            CancelInvoke("Think");

            Vector2 frontVec = new Vector2(rigid.position.x + nextmove * 0.2f, rigid.position.y);
            Vector3 downVec = new Vector2(rigid.position.x - 0.5f , rigid.position.y - 0.7f);

            Debug.DrawRay(frontVec, Vector3.down, new Color(1, 0, 0));
            Debug.DrawRay(frontVec, Vector2.right * nextmove * 0.3f, new Color(0, 1, 0));
            RaycastHit2D rayHitFoword = Physics2D.Raycast(frontVec, Vector2.right * nextmove * 0.1f, 0.3f, LayerMask.GetMask("Wall"));
            RaycastHit2D rayHitDown = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));
            RaycastHit2D rayHitEnemy = Physics2D.Raycast(downVec, Vector3.right, 1f, LayerMask.GetMask("Enemy"));
            RaycastHit2D rayHitPlayer = Physics2D.Raycast(frontVec, Vector2.right * nextmove * 0.1f, 0.3f, LayerMask.GetMask("Player"));

            if (!isJump || rayHitEnemy.collider != null)
            {
                rigid.velocity = new Vector2(nextmove * dashSpeed, rigid.velocity.y);
            }



            if (rayHitFoword.collider != null || rayHitPlayer.collider != null)
            {
                enemyAttack.FireBullet_8();
                Knockback(transform.position.normalized);
                CancelInvoke("Think");
            }

            if (rayHitDown.collider == null)
            {
                isJump = true;
            }
            else
            {
                isJump = false;
            }

        }
        public void Knockback(Vector2 direction)
        {
            float knockbackForce = 5f;
            float knockbackDuration = 1.5f;

            if (!isKnockback)
            {
                isKnockback = true;
                // Vector2 collisionDirection = (collision.transform.position - transform.position).normalized;

                // knockbackDirection에 주어진 방향으로 힘을 가해서 몬스터를 밀어냅니다.
                rigid.velocity = Vector2.zero; // 이전의 속도를 초기화합니다.
                rigid.AddForce(direction * -knockbackForce * 2f, ForceMode2D.Impulse);

                // knockbackDuration 후에 knockback 상태를 해제합니다.
                Invoke("EndKnockback", knockbackDuration);


            }
        }
        void EndKnockback()
        {
            Debug.Log("knockback");
            isAttack = false;
            isKnockback = false;
            nextmove *= -1;
            spriteRenderer.flipX = nextmove == 1;
            isFacingLeft = !isFacingLeft;
            Invoke("Think", 2f);
        }
        void Stop()
        {
            rigid.velocity = new Vector2(0, 0);
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                if(!isAttack){
                    Turn();
                }
                else{
                    Knockback(transform.position.normalized);
                }
            }
        }
        void DetectPlayerInRange(float detectionRange = 5f, bool isHorizontal = false)
        {
            // 플레이어의 위치
            Vector2 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

            // 몬스터와 플레이어의 거리 계산
            float distanceToPlayerX = Mathf.Abs(playerPosition.x - transform.position.x);
            float distanceToPlayerY = Mathf.Abs(playerPosition.y - transform.position.y);
            float distance = Mathf.Sqrt(distanceToPlayerX * distanceToPlayerX + distanceToPlayerY * distanceToPlayerY);

            //수평감지 여부 판단
            if (isHorizontal == false)
            {
                // 만약 플레이어가 감지 범위 내에 있다면
                if (distance <= detectionRange)
                {
                    // Debug.Log("Player detected!");
                    if (cooldownTimer <= 0)
                    {
                        if (Random.value > 0.3)
                        {
                            isAttack = true;
                            enemyAttack.FireBullet();
                            cooldownTimer = 2f;
                        }
                        else
                        {
                            CancelInvoke("Think");
                            Invoke("Think", 4f);
                            isAttack = true;
                            enemyAttack.FireBullet_Rapid();
                            cooldownTimer = 2f;
                        }
                    }
                }
            }
            else
            {
                if (distanceToPlayerY <= 1f)
                {
                    if (isFacingLeft)
                    {
                        // 플레이어가 몬스터의 왼쪽에 있고 감지 범위 내에 있다면
                        if (playerPosition.x < transform.position.x && distanceToPlayerX <= detectionRange)
                        {
                            Debug.Log("Player detected on the left!");
                            isAttack = true;
                        }
                        else
                        {
                            Debug.Log("Player undetected!");
                            isAttack = false;
                        }
                    }
                    else // 몬스터가 오른쪽을 보고 있을 때
                    {
                        // 플레이어가 몬스터의 오른쪽에 있고 감지 범위 내에 있다면
                        if (playerPosition.x > transform.position.x && distanceToPlayerX <= detectionRange)
                        {
                            Debug.Log("Player detected on the right!");
                            isAttack = true;
                        }
                        else
                        {
                            Debug.Log("Player undetected!");
                            isAttack = false;
                        }
                    }
                }
                else
                {
                    Debug.Log("Player undetected!");
                    isAttack = false;
                }

            }
        }

    }

}
