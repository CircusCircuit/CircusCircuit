using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class NewHoop : EnemyBase
    {
        public float dashSpeed = 15f;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            CancelInvoke();
        }

        protected override void Update()
        {
            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.fixedDeltaTime;
            }
            if (!isAttack)
            {
                detection.DetectPlayerInRangeVertical(5f);
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
                movement.MoveVertical(dashSpeed);
            }
            
        }
    }

    
}
