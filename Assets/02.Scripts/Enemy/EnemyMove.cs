using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;

namespace Enemy
{
    public class EnemyMove : MonoBehaviour
    {
        Rigidbody2D rigid;
        SpriteRenderer spriteRenderer;
        private EnemyAttack enemyAttack;
        private bool isDetectPlatfrom;
        public bool isFire =false;
        private bool isDetectPlayer =false;

        private bool isJump = false;

        public int nextmove;
        public float dashDuration = 0.5f;
        public float dashSpeed = 10f;
        // Start is called before the first frame update
        void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            enemyAttack = GetComponent<EnemyAttack>();
            Invoke("Think", 3);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(!isJump){
                enemyAttack.FireBullet_Rapid();
                isJump=true;
            }
            //move
            // rigid.velocity = new Vector2(nextmove * dashSpeed, rigid.velocity.y);
            
            // DetectPlayerInRange(10f, true);
            // //Platform Check
            // if (isDetectPlatfrom ==false)
            // {
            //     Vector2 frontVec = new Vector2(rigid.position.x + nextmove * 0.2f, rigid.position.y);
            //     Debug.DrawRay(frontVec, Vector3.down, new Color(1, 0, 0));
            //     Debug.DrawRay(frontVec, Vector2.right * nextmove * 0.3f, new Color(0, 1, 0));
            //     RaycastHit2D rayHitDown = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));
            //     RaycastHit2D rayHitFoword = Physics2D.Raycast(frontVec, Vector2.right * nextmove * 0.3f, 1, LayerMask.GetMask("Wall"));
            //     if (rayHitFoword.collider != null){
            //         enemyAttack.FireBullet_8();
            //         Turn();

            //     }
            //     if (rayHitDown.collider == null && isJump == false)
            //     {
            //         // if(Random.value>0.5f){
            //         //     CancelInvoke("Stop");
            //         //     UpJump();
            //         //     Invoke("Stop",1f);
            //         // }
            //         // else{
            //         CancelInvoke("Stop");
            //         CancelInvoke("Think");
            //         DownJump();
            //         Invoke("Stop",1f);
            //         Invoke("Think",3f);

            //         isFire =false;
            //         // }
            //     }

            //     if (rayHitDown.collider != null){
            //         isJump = false;
            //     }
            // }
        }
        
        //몬스터 행동 결정 함수, 재귀
        void Think(){
            if(isFire){
                enemyAttack.FireBullet_8();
                Invoke("Think",2f);
                isFire = false;
            }
            else{
                Dash();
                isFire = true;
            }
        }
        void Dash()
        {   
            if(! isDetectPlayer){
                //-1, 1중 결정
                nextmove = Random.Range(-1, 1)*2 + 1;
            }
            else{
                
                // 플레이어의 위치
                Vector2 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position; 
        
                if(playerPosition.x - transform.position.x <= 0){
                    nextmove = -1;
                }
                else{
                    nextmove = 1;
                }
            }
            
            //방향전환
            if (nextmove != 0)
            {
                spriteRenderer.flipX = nextmove == 1;
                Invoke("Stop", dashDuration); 
            }
            
            Invoke("Think", 3+dashDuration);
        }
        void Stop(){
            nextmove = 0;
        }

        void UpJump()
        {
            Debug.Log("upjump");
            rigid.velocity = new Vector2(rigid.velocity.x/5, 30f);
            isJump = true;
            Debug.Log("isJump"+isJump);
        }
        void DownJump()
        {
            Debug.Log("downjump");
            rigid.velocity = new Vector2(rigid.velocity.x, 15f);
            isJump = true;
            Debug.Log("isJump"+isJump);
        }

        void Turn()
        {
            Debug.Log("Turn!");
            nextmove = nextmove * -1;
            spriteRenderer.flipX = nextmove == 1;
            CancelInvoke("Dash");
            Invoke("Dash", 2);
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
                    isDetectPlayer = true;
                }
                else{
                    Debug.Log("Player undetected!");
                    isDetectPlayer = false;
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
                        isDetectPlayer = true;

                    }
                    //적 기준 오른쪽 위치
                    else if (playerPosition.x < transform.position.x && distanceToPlayerX <= detectionRange)
                    {
                        Debug.Log("Player Right detected!");
                        isDetectPlayer = true;
                    }
                }
                else
                {
                    Debug.Log("Player undetected!");    
                    isDetectPlayer = false;

                }
            }
        }
    
    }

}
