using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;

namespace Enemy
{
    public class EnemyAi : MonoBehaviour
    {
        Rigidbody2D rigid;
        SpriteRenderer spriteRenderer;
        private EnemyAttack enemyAttack;
        private EnemyMove enemymove;

        int think = 0;
        public bool isFire = false;
        public bool isDetectPlayer = false;


        private bool isJump = false;
        public bool isDying = false;

        public bool isAttack = false;
        public bool isKnockback = false;

        float knockbackDuration = 1.5f;
        public float cooldownTimer = 3f;
        // public float dashDuration = 0.5f;
        public float dashSpeed = 10f;
        Vector2 startPosition; // 시작 위치


        // Start is called before the first frame update
        void Awake()
        {
            enemymove = GetComponent<EnemyMove>();
            rigid = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            enemyAttack = GetComponent<EnemyAttack>();
            Invoke("Think",1f);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.fixedDeltaTime;
            }

            if (!isDying)
            {   
                if(cooldownTimer <= 0){
                    enemyAttack.FireBullet();
                    cooldownTimer = 1.5f;
                }
            }
        }

        public void Think()
        {
            think = Random.Range(-1, 2);
            if (think != 0)
            {
                enemymove.nextmove = think;
                if (think > 0)
                {
                    if (enemymove.isFacingLeft)
                    {
                        enemymove.Flip();
                    }
                }
                else
                {
                    if (!enemymove.isFacingLeft)
                    {
                        enemymove.Flip();
                    }
                }
            }
            else
            {
                enemymove.Stop();
                // enemyAttack.FireBullet_8();
            }
            Invoke("Think", 3);
        }
        public void ThinkFly()
        {
            if (enemymove.nextmove == 0)
            {
                enemymove.nextmove = Random.Range(-1, 2);
            }

            if (cooldownTimer <= 0)
            {
                if (Random.value > 0.6f)
                {
                    enemyAttack.FireBullet_8();
                    cooldownTimer = 2f;
                }
            }
            Invoke("ThinkFly", 1f);
        }

        void GroundMove(float moveSpeed = 2f)
        {
            Vector2 frontVec = new Vector2(rigid.position.x + enemymove.nextmove * 0.2f, rigid.position.y);
            Vector3 downVec = new Vector2(rigid.position.x - 0.5f, rigid.position.y - 0.7f);
            Debug.DrawRay(frontVec, Vector3.down, new Color(1, 0, 0));
            Debug.DrawRay(frontVec, Vector2.right * enemymove.nextmove * 0.3f, new Color(0, 1, 0));
            Debug.DrawRay(downVec, Vector3.right, new Color(0, 0, 1));


            RaycastHit2D rayHitGround = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));
            RaycastHit2D rayHitFoword = Physics2D.Raycast(frontVec, Vector2.right * enemymove.nextmove * 0.1f, 0.3f, LayerMask.GetMask("Wall"));
            RaycastHit2D rayHitEnemy = Physics2D.Raycast(downVec, Vector3.right, 1f, LayerMask.GetMask("Enemy"));


            // 점프 중이 아니거나 앞에 몬스터가 아래 있으면 이동
            if (!isJump || rayHitEnemy.collider != null)
            {
                enemymove.Move(moveSpeed);
            }

            //앞에 벽 감지시 돌아서 이동
            if (rayHitFoword.collider != null)
            {
                enemymove.Turn();
                CancelInvoke("Think");
                Invoke("Think", 2);
            }

            // 낭떨어지 만났을 때 점프 혹은 뒤돌기
            if (rayHitGround.collider == null && !isJump)
            {
                // 50% 확률
                // 뒤돌기
                if (Random.value < 0.5)
                {
                    enemymove.Turn();
                    CancelInvoke("Think");
                    Invoke("Think", 2);
                }
                // 점프
                else
                {
                    isJump = true;
                    if (Random.value < 0.5)
                    {
                        enemymove.UpJump();
                        CancelInvoke("Think");
                        Invoke("Think", 2);
                    }
                    else
                    {
                        enemymove.DownJump();
                        CancelInvoke("Think");
                        Invoke("Think", 2);
                    }
                }
            }

            // 점프 중 속도 조절
            if (rayHitGround.collider != null)
            {
                rigid.velocity = new Vector2(enemymove.nextmove * moveSpeed, rigid.velocity.y * 0.3f);
            }

            // 착지 후 점프 종료
            if (rayHitGround.collider != null && isJump)
            {
                isJump = false;
            }
        }

        void Dash(float dashSpeed = 10f)
        {
            isAttack = true;

            CancelInvoke("Think");

            Vector2 frontVec = new Vector2(rigid.position.x + enemymove.nextmove * 0.2f, rigid.position.y);
            Vector3 downVec = new Vector2(rigid.position.x - 0.5f, rigid.position.y - 0.7f);
            Debug.DrawRay(frontVec, Vector3.down, new Color(1, 0, 0));
            Debug.DrawRay(frontVec, Vector2.right * enemymove.nextmove * 0.3f, new Color(0, 1, 0));

            RaycastHit2D rayHitWall = Physics2D.Raycast(frontVec, Vector2.right * enemymove.nextmove * 0.1f, 0.3f, LayerMask.GetMask("Wall"));
            RaycastHit2D rayHitEnemy = Physics2D.Raycast(downVec, Vector3.right, 1f, LayerMask.GetMask("Enemy"));
            RaycastHit2D rayHitPlayer = Physics2D.Raycast(frontVec, Vector2.right * enemymove.nextmove * 0.1f, 0.3f, LayerMask.GetMask("Player"));
            RaycastHit2D rayHitGround = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));


            if (!isJump || rayHitEnemy.collider != null)
            {
                enemymove.Dash(dashSpeed);
            }

            //벽 혹은 플레이어와 박았을 시 행동
            if (rayHitWall.collider != null || rayHitPlayer.collider != null)
            {
                isKnockback = true;
                enemymove.Knockback(transform.position.normalized);
                enemyAttack.FireBullet_8();
                // knockbackDuration 후에 knockback 상태를 해제합니다.
                Invoke("CallEndKnockback", knockbackDuration);

                isAttack = false;
            }

            if (rayHitGround.collider == null && !isJump)
            {
                isJump = true;
                enemymove.DownJump();
            }
            if (rayHitGround.collider != null)
            {
                isJump = false;
            }


        }
        void DashVertical(float dashSpeed = 15f)
        {
            isAttack = true;

            Vector2 upVec = new Vector2(rigid.position.x - 0.5f, rigid.position.y + 0.6f);
            Vector3 downVec = new Vector2(rigid.position.x - 0.5f, rigid.position.y - 0.6f);
            
            Debug.DrawRay(downVec, Vector3.right, new Color(0, 0, 1));
            Debug.DrawRay(upVec, Vector3.right, new Color(1, 0, 0));

            RaycastHit2D rayHitWallDown = Physics2D.Raycast(downVec, Vector2.right, 1f, LayerMask.GetMask("Wall"));
            RaycastHit2D rayHitPlayerDown = Physics2D.Raycast(downVec, Vector2.up * enemymove.nextmove * 0.1f, 0.3f, LayerMask.GetMask("Player"));
            RaycastHit2D rayHitWallUp = Physics2D.Raycast(upVec, Vector2.right, 1f, LayerMask.GetMask("Wall"));
            RaycastHit2D rayHitPlayerUp = Physics2D.Raycast(upVec, Vector2.up * enemymove.nextmove * 0.1f, 0.3f, LayerMask.GetMask("Player"));
            
            // RaycastHit2D rayHitGround = Physics2D.Raycast(upVec, Vector3.down, 1, LayerMask.GetMask("Ground"));

            if (!isJump)
            {
                enemymove.MoveVertical(dashSpeed);
            }

            //벽 혹은 플레이어와 박았을 시 행동
            if (rayHitWallUp.collider != null || rayHitPlayerUp.collider != null
                ||rayHitWallDown.collider != null || rayHitPlayerDown.collider != null)
            {
                Debug.Log("wall");
                isKnockback = true;
                enemymove.Knockback(transform.position.normalized);
                
                // knockbackDuration 후에 knockback 상태를 해제합니다.
                Invoke("CallEndKnockback", 0.2f);

                isAttack = false;
            }

        }
        // public void VerticalDash(float dashSpeed = 10f)
        // {
        //     CancelInvoke("Think");

        //     Vector2 frontVec = new Vector2(rigid.position.x + nextmove * 0.2f, rigid.position.y);
        //     Vector3 downVec = new Vector2(rigid.position.x - 0.5f , rigid.position.y - 0.7f);

        //     Debug.DrawRay(frontVec, Vector3.down, new Color(1, 0, 0));
        //     Debug.DrawRay(frontVec, Vector2.right * nextmove * 0.3f, new Color(0, 1, 0));
        //     RaycastHit2D rayHitFoword = Physics2D.Raycast(frontVec, Vector2.right * nextmove * 0.1f, 0.3f, LayerMask.GetMask("Wall"));
        //     RaycastHit2D rayHitDown = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));
        //     RaycastHit2D rayHitEnemy = Physics2D.Raycast(downVec, Vector3.right, 1f, LayerMask.GetMask("Enemy"));
        //     RaycastHit2D rayHitPlayer = Physics2D.Raycast(frontVec, Vector2.right * nextmove * 0.1f, 0.3f, LayerMask.GetMask("Player"));

        //     if (!isJump || rayHitEnemy.collider != null)
        //     {
        //         rigid.velocity = new Vector2( rigid.velocity.x, nextmove * dashSpeed);
        //     }


        //     if (rayHitFoword.collider != null || rayHitPlayer.collider != null)
        //     {
        //         Knockback(transform.position.normalized);
        //         CancelInvoke("Think");
        //     }

        // }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                if (!isJump)
                {
                    if (!isAttack)
                    {
                        enemymove.Turn();
                    }
                    else
                    {
                        isKnockback = true;
                        isAttack = false;
                        enemymove.Knockback(transform.position.normalized);
                        Invoke("CallEndKnockback", 0.3f);
                    }
                }

            }
        }
        void CallEndKnockback()
        {
            enemymove.Stop();
            isKnockback = false;         
            cooldownTimer = 3f;
        }

        void EndAttack()
        {
            isAttack = false;
        }

        void DetectPlayerInRange(float detectionRange = 5f)
        {
            // 플레이어의 위치
            Vector2 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

            // 몬스터와 플레이어의 거리 계산
            float distanceToPlayerX = Mathf.Abs(playerPosition.x - transform.position.x);
            float distanceToPlayerY = Mathf.Abs(playerPosition.y - transform.position.y);
            float distance = Mathf.Sqrt(distanceToPlayerX * distanceToPlayerX + distanceToPlayerY * distanceToPlayerY);

            // 감지범위 시각화      
            DebugDrawDetectionRange(transform.position, detectionRange);

            // 만약 플레이어가 감지 범위 내에 있다면
            if (distance <= detectionRange)
            {
                isDetectPlayer = true;
                Debug.Log("Player detected!");
            }
            else
            {
                Debug.Log("Player undetected!");
                isDetectPlayer = false;
            }


        }
        void DetectPlayerInRangeHorizental(float detectionRange = 5f)
        {
            // 플레이어의 위치
            Vector2 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

            // 몬스터와 플레이어의 거리 계산
            float distanceToPlayerX = Mathf.Abs(playerPosition.x - transform.position.x);
            float distanceToPlayerY = Mathf.Abs(playerPosition.y - transform.position.y);

            // 감지범위 시각화      
            DebugDrawDetectionRangeHorizental(transform.position, detectionRange);
            if (distanceToPlayerY <= 1f)
            {
                // 플레이어가 몬스터의 왼쪽에 있고 감지 범위 내에 있다면
                if (enemymove.isFacingLeft)
                {
                    if (playerPosition.x < transform.position.x && distanceToPlayerX <= detectionRange)
                    {
                        Debug.Log("Player detected on the left!");
                        isDetectPlayer = true;
                    }
                }
                // 플레이어가 몬스터의 오른쪽에 있고 감지 범위 내에 있다면
                else
                {
                    if (playerPosition.x > transform.position.x && distanceToPlayerX <= detectionRange)
                    {
                        Debug.Log("Player detected on the right!");
                        isDetectPlayer = true;
                    }
                }
            }
            else
            {
                // Debug.Log("Player undetected!");
                isDetectPlayer = false;
            }
        }
        void DetectPlayerInRangeVertical(float detectionRange = 5f)
        {
            // 플레이어의 위치
            Vector2 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

            // 몬스터와 플레이어의 거리 계산
            float distanceToPlayerX = Mathf.Abs(playerPosition.x - transform.position.x);
            float distanceToPlayerY = Mathf.Abs(playerPosition.y - transform.position.y);

            // 감지범위 시각화      
            DebugDrawDetectionRangeVertical(transform.position, detectionRange);
            if (distanceToPlayerX <= 1f)
            {
                // 플레이어가 몬스터의 아래 쪽에 있을 때
                if (playerPosition.y < transform.position.y && distanceToPlayerY <= detectionRange)
                {
                    Debug.Log("Player detected below!!");
                    isDetectPlayer = true;
                    enemymove.nextmove = -1;
                }

                // 플레이어가 몬스터의 위쪽에 있을 때
                else
                {
                    if (playerPosition.y > transform.position.y && distanceToPlayerY <= detectionRange)
                    {
                        Debug.Log("Player detected above!!");
                        isDetectPlayer = true;
                        enemymove.nextmove = 1;

                    }
                }
            }
            else
            {
                // Debug.Log("Player undetected!");
                isDetectPlayer = false;
            }
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
    }
}
