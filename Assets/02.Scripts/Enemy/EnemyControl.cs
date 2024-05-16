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
                Debug.Log("key Down A");
                move.Move(5,-1);
            }
            if (Input.GetKeyUp(KeyCode.A)) {
                move.Stop();
            }

            if (Input.GetKey(KeyCode.D)){
                move.Move(5,1);
                Debug.Log("key Down D");
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


            if (Input.GetKeyDown(KeyCode.Space)){
                Debug.Log("Get Key Down Space");
                move.Dash();
            }
        }
    }
}
