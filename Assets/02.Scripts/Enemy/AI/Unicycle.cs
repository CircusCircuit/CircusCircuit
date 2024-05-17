using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;

namespace Enemy
{
    public class Unicycle : MonoBehaviour
    {
        Rigidbody2D rigid;
        SpriteRenderer spriteRenderer;
        private EnemyAttack enemyAttack;
        private DetectPlayer enemyDetect;

        private EnemyMove enemymove;

        int think = 0;
        public bool isFire = false;
        public bool isDetectPlayer = false;
        private bool isJump = false;

        public bool isGround = false;
        public bool isDying = false;

        public bool isAttack = false;
        public bool isKnockback = false;

        float knockbackDuration = 1.5f;
        public float cooldownTimer = 3f;
        // public float dashDuration = 0.5f;
        public float dashSpeed = 10f;

        // Start is called before the first frame update
        void Awake()
        {
            enemymove = GetComponent<EnemyMove>();
            rigid = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            enemyAttack = GetComponent<EnemyAttack>();
            think = enemymove.nextmove;
            enemyDetect = GetComponent<DetectPlayer>();

            Invoke("Think", 1);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!isDying)
            {
                if (!isKnockback)
                {
                    if (!enemyDetect.isDetectPlayer)
                    {
                        GroundMove(2f);
                        enemyDetect.DetectPlayerInRangeHorizental(5f);
                    }
                    else
                    {
                        Dash(10f);
                    }
                }
            }

        }

        public void Think()
        {
            think = Random.Range(-1, 2);
            if (think != 0)
            {
                enemymove.nextmove = think;
                if (think > 0){
                    if(enemymove.isFacingLeft){                    
                        enemymove.Flip();
                    }
                }
                else{
                    if(!enemymove.isFacingLeft){                    
                        enemymove.Flip();
                    }
                }
            }
            else
            {
                enemymove.Stop();
                enemyAttack.FireBullet_8();
            }
            Invoke("Think", 3);
        }

        void GroundMove(float moveSpeed = 2f)
        {
            Vector2 frontVec = new Vector2(rigid.position.x + enemymove.nextmove * 0.2f, rigid.position.y);
            Vector3 downVec = new Vector2(rigid.position.x - 0.5f, rigid.position.y - 0.7f);
            Debug.DrawRay(frontVec, Vector3.down, new Color(1, 0, 0));
            Debug.DrawRay(frontVec, Vector2.right * enemymove.nextmove * 0.3f, new Color(0, 1, 0));
            Debug.DrawRay(downVec, Vector3.right, new Color(0, 0, 1));

            RaycastHit2D rayHitGround = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));
            RaycastHit2D rayHitFoword = Physics2D.Raycast(frontVec, Vector2.right * enemymove.nextmove * 0.1f, 0.3f, LayerMask.GetMask("Ground"));
            RaycastHit2D rayHitEnemy = Physics2D.Raycast(downVec, Vector3.right, 1f, LayerMask.GetMask("Enemy"));


            // 점프 중이 아니거나 앞에 몬스터가 아래 있으면 이동
            if (!isJump || rayHitEnemy.collider != null)
            {
                enemymove.Move(moveSpeed);
            }

            //앞에 벽 감지시 돌아서 이동
            if (rayHitFoword.collider != null)
            {
                Debug.Log(rayHitFoword.collider);
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
            if (isJump)
            {
                rigid.velocity = new Vector2(enemymove.nextmove * 5f, rigid.velocity.y);
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

            RaycastHit2D rayHitWall = Physics2D.Raycast(frontVec, Vector2.right * enemymove.nextmove * 0.1f, 0.3f, LayerMask.GetMask("Ground"));
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
                enemyDetect.isDetectPlayer = false;
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
                        Invoke("CallEndKnockback", knockbackDuration);
                    }
                }

            }
        }
        void CallEndKnockback()
        {
            isKnockback = false;
            isDetectPlayer = false;
            Think();
        }
        
    }
}
