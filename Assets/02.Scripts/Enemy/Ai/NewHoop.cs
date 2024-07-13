using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class NewHoop : EnemyBase
    {
        public float dashSpeed = 15f;
        Vector2 startPosition;

        // Start is called before the first frame update
        protected override void Start()
        {
            startPosition = transform.position;
            base.Start();
            enemyHP = 3;
            CancelInvoke();
        }

        protected override void Update()
        {
            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;
            }

            if (!isDying)
            {
                if (!isAttack)
                {
                    detection.DetectPlayerInRangeVertical(5f);
                }
                
                if (!isKnockback)
                {
                    if (isDetectPlayer)
                    {
                        if (cooldownTimer <= 0)
                        {
                            isAttack = true;
                            DashVertical(dashSpeed);
                            cooldownTimer=2f;
                        }
                    }
                }
            }
        }

        void DashVertical(float dashSpeed = 15f)
        {
            movement.MoveVertical(dashSpeed);
        }
        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);
            movement.Stop();
            nextmove = 0;
            isAttack = false;
        }
    }

    
}
