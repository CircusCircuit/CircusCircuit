using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Enemy{
    public class EnemyControl : MonoBehaviour
    {
        EnemyMove move;
        // Start is called before the first frame update
        void Start()
        {
            move = GetComponent<EnemyMove>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.A)){
                move.nextmove = -1;
                move.Move(5);
            }
            if (Input.GetKeyUp(KeyCode.A)) {
                move.Stop();
            }

            if (Input.GetKey(KeyCode.D)){
                move.nextmove = 1;
                move.Move(5);
            }
            if (Input.GetKeyUp(KeyCode.D)) {
                move.Stop();
            }
            
            if (Input.GetKeyDown(KeyCode.W)){
                move.UpJump();
            }

            if (Input.GetKeyDown(KeyCode.S)){
                move.DownJump();
            }

            if (Input.GetKey(KeyCode.LeftShift)){
                Debug.Log("Get Key Down LeftShift");
                move.Dash();
            }

            if (Input.GetKeyDown(KeyCode.Space)){
                Debug.Log("Get Key Down Space");
                move.Knockback(new Vector2(1,0));
            }
        }
    }
}
