using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Enemy
{
    public class F_Bullet : E_Bullet
    {
        float cooldownTimer = 0.5f;
        bool isAttack = false;

        private float inclination;
        
        private Vector3 initialPosition; // 초기 위치
        // Start is called before the first frame update
        protected override void Start()
        {
            destroyTime = 10f;
            base.Start();
            initialPosition = transform.position;
            inclination = initialPosition.y / math.pow(initialPosition.x, 2);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(cooldownTimer<0){
                MoveObjectToOrigin();
            }
            else{
                if(transform.position.x <= 0.1f && transform.position.y <= 0.1f){
                    rigid.position = Vector2.zero;

                    if(!isAttack){
                        Invoke("DestroyBullet",3f);
                        isAttack = true;
                    }
                }
                else{
                    MoveObjectToOrigin();
                }
            }   
        }

        public void MoveObjectToOrigin()
        {
            float y_position = inclination * (math.pow(transform.position.x, 2));
            rigid.position = new Vector2(transform.position.x - 0.1f, y_position);
        }

        //패턴 1 저장
        // void DestroyBullet()
        // {
        //     enemyAttack.FireBullet_8();
        //     Destroy(gameObject);
        // }
    }
}
