using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Enemy{
    public class EnemyControl : EnemyBase
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            if (Input.GetKey(KeyCode.A)){
                nextmove = -1;
                movement.Move(5);
            }
            if (Input.GetKeyUp(KeyCode.A)) {
                movement.Stop();
            }

            if (Input.GetKey(KeyCode.D)){
                nextmove = 1;
                movement.Move(5);
            }
            if (Input.GetKeyUp(KeyCode.D)) {
                movement.Stop();
            }
            
            if (Input.GetKeyDown(KeyCode.W)){
                movement.UpJump();
            }

            if (Input.GetKeyDown(KeyCode.S)){
                movement.DownJump();
            }

            if (Input.GetKey(KeyCode.LeftShift)){
                Debug.Log("Get Key Down LeftShift");
                movement.Dash();
            }

            if (Input.GetKeyDown(KeyCode.Space)){
                Debug.Log("Get Key Down Space");
                movement.Knockback(new Vector2(1,0));
            }
        }
    }
}
