using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        DetectPlayerInRange(10f, true);
    }

    void DetectPlayerInRange(float detectionRange = 5f, bool isHorizontal = false)
    {
        // 플레이어의 위치
        Vector2 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

        // 몬스터와 플레이어의 거리 계산
        float distanceToPlayerX = Mathf.Abs(playerPosition.x - transform.position.x);
        float distanceToPlayerY = Mathf.Abs(playerPosition.y - transform.position.y);

        //수평감지 여부 판단
        if (isHorizontal == false)
        {
            // 만약 플레이어가 감지 범위 내에 있다면
            if (distanceToPlayerX <= detectionRange)
            {
                Debug.Log("Player detected!");
            }
            else{
                Debug.Log("Player undetected!");    
            }
        }
        else
        {
            if (distanceToPlayerY <= 1f)
            {
                //적 기준 왼쪽 위치
                if (playerPosition.x > transform.position.x && distanceToPlayerX <= detectionRange)
                {
                    Debug.Log("Player left detected!");
                }
                //적 기준 오른쪽 위치
                else if (playerPosition.x < transform.position.x && distanceToPlayerX <= detectionRange)
                {
                    Debug.Log("Player Right detected!");
                }
                else{
                    Debug.Log("Player undetected!");    
                }
            }
        }
    }
}
