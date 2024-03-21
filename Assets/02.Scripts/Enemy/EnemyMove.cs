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

        public bool isFire = false;
        private bool isDetectPlayer = false;
        private bool isJump = false;
        public bool isAttack = false;
        
        private bool isKnockback = false;
        public int nextmove;
        // public float dashDuration = 0.5f;
        public float dashSpeed = 10f;
        // Start is called before the first frame update
        void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            enemyAttack = GetComponent<EnemyAttack>();
            Invoke("Think", 1);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(!isKnockback){
                if(!isAttack){
                    Move(2f);
                }
                else{
                    Dash(10f);
                }
                DetectPlayerInRange(5f, true);
            }
        }
        public void Think()
        {
            nextmove = Random.Range(-1, 2);
            //방향전환
            if (nextmove != 0)
            {
                spriteRenderer.flipX = nextmove == 1;
            }
            else{
                enemyAttack.FireBullet_8();
            }
            Invoke("Think", 3);
        }
        void Turn()
        {
            nextmove = nextmove * -1;
            spriteRenderer.flipX = nextmove == 1;
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
            if (!isJump)
            {
                rigid.velocity = new Vector2(nextmove * moveSpeed, 0);
            }
            
            Vector2 frontVec = new Vector2(rigid.position.x + nextmove * 0.2f, rigid.position.y);
            Debug.DrawRay(frontVec, Vector3.down, new Color(1, 0, 0));
            Debug.DrawRay(frontVec, Vector2.right * nextmove * 0.3f, new Color(0, 1, 0));
            RaycastHit2D rayHitDown = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));
            RaycastHit2D rayHitFoword = Physics2D.Raycast(frontVec, Vector2.right * nextmove * 0.1f, 0.3f, LayerMask.GetMask("Wall"));

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
                rigid.velocity = new Vector2(nextmove * moveSpeed, rigid.velocity.y*0.3f);
                isJump = false;
            }
            if (rayHitFoword.collider != null)
            {
                Turn();
                CancelInvoke("Think");
                Invoke("Think", 2);
            }

        }
        public void Dash(float dashSpeed = 10f)
        {   
            CancelInvoke("Think");
            
            if (!isJump)
            {
                rigid.velocity = new Vector2(nextmove * dashSpeed, rigid.velocity.y);
            }

            Vector2 frontVec = new Vector2(rigid.position.x + nextmove * 0.2f, rigid.position.y);
            Debug.DrawRay(frontVec, Vector3.down, new Color(1, 0, 0));
            Debug.DrawRay(frontVec, Vector2.right * nextmove * 0.3f, new Color(0, 1, 0));
            RaycastHit2D rayHitFoword = Physics2D.Raycast(frontVec, Vector2.right * nextmove * 0.1f, 0.3f, LayerMask.GetMask("Wall"));
            RaycastHit2D rayHitDown = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));

            if (rayHitFoword.collider != null)
            {
                enemyAttack.FireBullet_8();
                Knockback(transform.position.normalized);
                CancelInvoke("Think");
                
            }
            if (rayHitDown.collider == null)
            {
                isJump = true;
            }
            else{
                isJump = false;
            }

        }
        public void Knockback(Vector2 direction)
        {
            float knockbackForce = 5f;
            float knockbackDuration = 1.5f;
            
            if (!isKnockback)
            {
                // Vector2 collisionDirection = (collision.transform.position - transform.position).normalized;

                // knockbackDirection에 주어진 방향으로 힘을 가해서 몬스터를 밀어냅니다.
                rigid.velocity = Vector2.zero; // 이전의 속도를 초기화합니다.
                rigid.AddForce(direction * -knockbackForce, ForceMode2D.Impulse);

                // knockbackDuration 후에 knockback 상태를 해제합니다.
                Invoke("EndKnockback", knockbackDuration);

                isKnockback = true;
            }
        }
        void EndKnockback(){
            Debug.Log("knockback");
            isAttack = false;
            isKnockback = false;
            nextmove *= -1;
            spriteRenderer.flipX = nextmove == 1;
            Invoke("Think", 2f);
        }
        void Stop()
        {
            nextmove = 0;
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
                    isAttack = true;
                }
                else
                {
                    Debug.Log("Player undetected!");
                    isAttack = false;
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
                        isAttack = true;

                    }
                    //적 기준 오른쪽 위치
                    else if (playerPosition.x < transform.position.x && distanceToPlayerX <= detectionRange)
                    {
                        Debug.Log("Player Right detected!");
                        isAttack = true;
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
