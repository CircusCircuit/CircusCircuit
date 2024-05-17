using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;

namespace Enemy
{
    public class Boss : MonoBehaviour
    {
        Rigidbody2D rigid;
        SpriteRenderer spriteRenderer;
        Vector2 startPosition; 

        private BossAttack bossAttack;
        private DetectPlayer enemyDetect;

        public bool isFire = false;
        public bool isDying = false;
        public bool isAttack = false;


        public float cooldownTimer = 3f;
        // public float dashDuration = 0.5f;
        public int think = 1;

        // Start is called before the first frame update
        void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            bossAttack = GetComponent<BossAttack>();
            enemyDetect = GetComponent<DetectPlayer>();
            Invoke("Think",1f);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(cooldownTimer>0){
                cooldownTimer-= Time.deltaTime;
            }
            else{
                cooldownTimer = 100f;
                // bossAttack.FireBullet_Rapid();
            }
        }

        public void Think()
        {
            think = Random.Range(-1, 2);
           
            Invoke("Think", 3);
        }
        
        void EndAttack()
        {
            isAttack = false;
        }

        
    }
}
