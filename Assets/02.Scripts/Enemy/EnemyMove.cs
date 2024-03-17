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
            Invoke("Dash", 3);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //move
            rigid.velocity = new Vector2(nextmove * dashSpeed, rigid.velocity.y);

            //Platform Check
            if (isDetectPlatfrom ==false)
            {
                Vector2 frontVec = new Vector2(rigid.position.x + nextmove * 0.2f, rigid.position.y);
                Debug.DrawRay(frontVec, Vector3.down, new Color(1, 0, 0));
                Debug.DrawRay(frontVec, Vector2.right * nextmove * 0.3f, new Color(0, 1, 0));
                RaycastHit2D rayHitDown = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));
                RaycastHit2D rayHitFoword = Physics2D.Raycast(frontVec, Vector2.right * nextmove * 0.3f, 1, LayerMask.GetMask("Ground"));
                if (rayHitFoword.collider != null){
                    enemyAttack.FireBullet_8();
                    Turn();

                }
                if (rayHitDown.collider == null && isJump == false)
                {
                    if(Random.value>0.5f){
                        CancelInvoke("Stop");
                        UpJump();
                        Invoke("Stop",1f);
                    }
                    else{
                        CancelInvoke("Stop");
                        DownJump();
                        Invoke("Stop",1f);
                    }
                }

                if (rayHitDown.collider != null){
                    isJump = false;
                }
            }
            
            
            
        }

        //몬스터 행동 결정 함수, 재귀
        void Dash()
        {
            //-1, 1중 결정
            nextmove = Random.Range(-1, 1)*2 + 1;
    
            //방향전환
            if (nextmove != 0)
            {
                spriteRenderer.flipX = nextmove == 1;
                Invoke("Stop", dashDuration); 
            }
            
            Invoke("Dash", 3+dashDuration);
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

    }

}
