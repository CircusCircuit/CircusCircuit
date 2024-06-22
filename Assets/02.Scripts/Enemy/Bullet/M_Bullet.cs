using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Enemy
{
    public class M_Bullet : E_Bullet
    {
        public float amount = 3.0f; // 이동하는 시간
        public float distance = 2.0f; // d 값
        private float t = 0.0f; // 시간 변수
        private float inclination;
        
        private Vector3 initialPosition; // 초기 위치

        protected override void Start()
        {
            destroyTime = 20f;
            base.Start();
            initialPosition = transform.position;
            inclination = initialPosition.y / math.pow(initialPosition.x, 2);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Drow();
        }

        public void Drow(){
            t = t+Time.deltaTime;
            float x = amount * Mathf.Sin(t) - distance * Mathf.Sin(amount * t);
            float y = amount * Mathf.Cos(t) + distance * Mathf.Cos(amount * t);

            // 오브젝트 이동
            transform.position = new Vector2(x/1f, y/1.5f);
        }


        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            
        }

        // 총알을 파괴하는 함수

    }
}
