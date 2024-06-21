using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class NewFanfare : EnemyBase
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
                cooldownTimer -= Time.deltaTime;
            }

            if (!isDying)
            {   

                detection.DetectPlayerInRange(5f);

                if (isDetectPlayer)
                {
                    if(cooldownTimer <= 0){
                        attack.FireBullet_area();
                        cooldownTimer = 3f;
                    }
                }

            }
        }
    }
    
}
