using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class NewUnicycle : EnemyBase
    {
        bool flag = false;
        public float knockbackDuration = 1.5f;
        private Interact interactScript;
        protected override void Start()
        {
            GameObject interactionManager = GameObject.FindWithTag("InteractionManager");
            if (interactionManager != null)
            {
                interactScript = interactionManager.GetComponent<Interact>();
            }
            base.Start();
            enemyHP = 5;
        }

        protected override void Update()
        {
            if (!isDying)
            {
                if (!isKnockback)
                {
                    if (!isDetectPlayer)
                    {
                        GroundMove(2f);
                        detection.DetectPlayerInRangeHorizental(5f);
                    }
                    else
                    {
                        Dash(10f);
                    }
                }
            }
            if(enemyHP <= 0){
                if(!flag){
                    interactScript.PlayerSpeedUp(3.0f);
                    flag = true;
                }
            }
        }

        protected void Dash(float dashSpeed = 10f)
        {
            isAttack = true;

            CancelInvoke("Think");

            Vector2 frontVec = new Vector2(rigid.position.x + nextmove * 0.2f, rigid.position.y);
            Vector3 downVec = new Vector2(rigid.position.x - 0.5f, rigid.position.y - 0.7f);
            Debug.DrawRay(frontVec, Vector3.down, new Color(1, 0, 0));
            Debug.DrawRay(frontVec, Vector2.right * nextmove * 0.3f, new Color(0, 1, 0));

            RaycastHit2D rayHitWall = Physics2D.Raycast(frontVec, Vector2.right * nextmove * 0.1f, 0.3f, LayerMask.GetMask("Ground"));
            RaycastHit2D rayHitEnemy = Physics2D.Raycast(downVec, Vector3.right, 1f, LayerMask.GetMask("Enemy"));
            RaycastHit2D rayHitPlayer = Physics2D.Raycast(frontVec, Vector2.right * nextmove * 0.1f, 0.3f, LayerMask.GetMask("Player"));
            RaycastHit2D rayHitGround = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));


            if (!isJump || rayHitEnemy.collider != null)
            {
                movement.Dash(dashSpeed);
            }

            //벽 혹은 플레이어와 박았을 시 행동
            if (rayHitWall.collider != null || rayHitPlayer.collider != null)
            {
                isKnockback = true;
                movement.Knockback(transform.position.normalized);
                attack.FireBullet_8();
                // knockbackDuration 후에 knockback 상태를 해제합니다.
                Invoke("EndKnockback", knockbackDuration);
                isDetectPlayer = false;
                isAttack = false;
            }

            if (rayHitGround.collider == null && !isJump)
            {
                isJump = true;
                movement.DownJump();
            }
            if (rayHitGround.collider != null)
            {
                isJump = false;
            }
        }
        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);
            if (collision.gameObject.CompareTag("Enemy"))
            {
                if (!isJump)
                {
                    if (!isAttack)
                    {
                        movement.Turn();
                    }
                    else
                    {
                        isKnockback = true;
                        isAttack = false;
                        movement.Knockback(transform.position.normalized);
                        Invoke("EndKnockback", knockbackDuration);
                    }
                }

            }
        }
        public void EndKnockback()
        {
            //Debug.Log("Endknockback");
            isKnockback = false;
            movement.Turn();
            Think();
        }
    }

}
