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
        protected override void Start()
        {
            base.Start();    
        }
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            // 기본 동작을 유지하기 위해 base.OnTriggerEnter2D를 호출합니다.
            base.OnTriggerEnter2D(collision);

            // 충돌한 객체가 벽이면 반사하는 동작을 추가합니다.
            if (collision.gameObject.CompareTag("Ground"))
            {
                // Vector2 normal = collision.contacts[0].normal;
                // rigid.velocity = Vector2.Reflect(rigid.velocity, normal);
            }
        }
 
    }
}
