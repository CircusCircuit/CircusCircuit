using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;

namespace Enemy
{
    public class Hulahoop : MonoBehaviour
    {
        Rigidbody2D rigid;
        SpriteRenderer spriteRenderer;
        private EnemyAttack enemyAttack;
        private EnemyMove enemymove;

        public bool isDetectPlayer = false;

        private bool isJump = false;
        public bool isDying = false;

        public bool isAttack = false;
        public bool isKnockback = false;

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
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.fixedDeltaTime;
            }
            if (!isAttack)
            {
                DetectPlayerInRangeVertical(5f);
            }
            if (!isDying)
            {
                if (!isKnockback)
                {
                    if (isDetectPlayer)
                    {
                        if (cooldownTimer <= 0)
                        {
                            DashVertical(dashSpeed);
                        }
                    }
                }

            }

        }

        void DashVertical(float dashSpeed = 15f)
        {
            isAttack = true;

            if (!isJump)
            {
                enemymove.MoveVertical(dashSpeed);
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
            if (isAttack)
            {
                isKnockback = true;
                isAttack = false;
                enemymove.Knockback(transform.position.normalized);
                Invoke("CallEndKnockback", 0.3f);
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
