using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;

namespace Enemy
{
    public class MagicHat : MonoBehaviour
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
            think = enemymove.nextmove;
            Invoke("ThinkFly", 1);
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
                if (!isAttack)
                {
                    enemymove.Fly();
                    DetectPlayerInRange(5f);
                }

                if (isDetectPlayer)
                {
                    if (cooldownTimer <= 0)
                    {
                        if (Random.value > 0.4)
                        {
                            CancelInvoke("ThinkFly");
                            Invoke("ThinkFly", 1f);
                            enemyAttack.FireBullet();
                            cooldownTimer = 2f;
                        }
                        else
                        {
                            isAttack = true;
                            CancelInvoke("ThinkFly");
                            enemymove.Stop();
                            Invoke("EndAttack", 1f);
                            Invoke("ThinkFly", 1.5f);
                            enemyAttack.FireBullet_Rapid();
                            cooldownTimer = 2f;
                        }
                    }
                }
            }

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
                        enemymove.Knockback(transform.position.normalized);
                        Invoke("CallEndKnockback", knockbackDuration);
                    }
                }

            }
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
}
