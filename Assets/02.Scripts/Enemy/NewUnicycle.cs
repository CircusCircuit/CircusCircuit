using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy{
    public class NewUnicycle : EnemyBase
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
            if (!isDying)
            {
                if (!isKnockback)
                {
                    if (!isDetectPlayer)
                    {
                        movement.Move(speed);
                        detection.DetectPlayerInRangeHorizental(5f);
                    }
                    else
                    {
                        // attack.Dash(10f);
                    }
                }
            }
        }
    }

}
