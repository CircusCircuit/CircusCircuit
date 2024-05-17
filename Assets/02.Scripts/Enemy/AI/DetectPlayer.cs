using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class DetectPlayer : MonoBehaviour
    {
        private EnemyMove enemymove;

        public bool isDetectPlayer = false;

        // Update is called once per frame

        void Awake(){
            enemymove = GetComponent<EnemyMove>();
        }

        public void DetectPlayerInRange(float detectionRange = 5f)
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

        public void DetectPlayerInRangeHorizental(float detectionRange = 5f)
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

        public void DetectPlayerInRangeVertical(float detectionRange = 5f)
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
