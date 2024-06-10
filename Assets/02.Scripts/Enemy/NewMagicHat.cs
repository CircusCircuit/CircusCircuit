using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class NewMagicHat : EnemyBase
    {
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

            if (!isDying)
            {
                if (!isAttack)
                {
                    // enemymove.Fly();
                    detection.DetectPlayerInRange(5f);
                }

                if (isDetectPlayer)
                {
                    if (cooldownTimer <= 0)
                    {
                        if (Random.value > 0.4)
                        {
                            CancelInvoke("ThinkFly");
                            Invoke("ThinkFly", 1f);
                            // attack.FireBullet();
                            cooldownTimer = 2f;
                        }
                        else
                        {
                            isAttack = true;
                            CancelInvoke("ThinkFly");
                            movement.Stop();
                            Invoke("EndAttack", 1f);
                            Invoke("ThinkFly", 1.5f);
                            // attack.FireBullet_Rapid();
                            cooldownTimer = 2f;
                        }
                    }
                }
            }
        }

        public void ThinkFly()
        {
            if (nextmove == 0)
            {
                nextmove = Random.Range(-1, 2);
            }

            if (cooldownTimer <= 0)
            {
                if (Random.value > 0.6f)
                {
                    attack.FireBullet_8();
                    cooldownTimer = 2f;
                }
            }
            Invoke("ThinkFly", 1f);
        }
    }   
}
