using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Enemy
{
    public class E_B_Bullet : E_Bullet
    {

        private Vector2 previousPosition;
        private Vector2 lastVelocity;

        public int boundCnt = 2;
        protected override void Start()
        {
            base.Start();
            previousPosition = rigid.position;
        }

        protected void Update(){
            lastVelocity = rigid.velocity;
        }
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                if(boundCnt>1){
                    var speed = lastVelocity.magnitude;
                    var dir = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

                    rigid.velocity = dir * Mathf.Max(speed, 0f);    
                    boundCnt-=1;
                }
                else{
                    DestroyBullet();
                }
                
            }
            else if (collision.gameObject.CompareTag("Platform"))
            {
                Collider2D collider = collision.collider;
                collider.isTrigger = true;
            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player") 
            || collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                DestroyBullet();
            }
        }
        public void InvertDirection(float dx, float dy)
        {
            Vector2 currentVelocity = rigid.velocity;
            currentVelocity.y = -currentVelocity.y; // x 방향 반전
            rigid.velocity = currentVelocity;
        }
    }
}
