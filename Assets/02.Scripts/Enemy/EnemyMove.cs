using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;

namespace Enemy
{
    public class EnemyMove : MonoBehaviour
    {
        private EnemyOneWayPlatform oneWay;

        Rigidbody2D rigid;
        SpriteRenderer spriteRenderer;
        private EnemyAttack enemyAttack;
        public float maxFlyDistance = 1.5f; // 오브젝트와 중앙 사이의 최대 거리
        public bool isFacingLeft = true;
        public int nextmove = 1;
        public float cooldownTimer = 3f;
        // public float dashDuration = 0.5f;
        Vector2 startPosition; // 시작 위치

        // Start is called before the first frame update
        void Awake()
        {
            oneWay = GetComponent<EnemyOneWayPlatform>();
            rigid = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            startPosition = transform.position;
        }
   
        public void Move(float moveSpeed = 2f)
        {
            
            rigid.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
            
            rigid.velocity = new Vector2(moveSpeed*nextmove, rigid.velocity.y);
            
        }
        public void Stop()
        {
            rigid.constraints |= RigidbodyConstraints2D.FreezePositionX;
            nextmove = 0;
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }

        public void UpJump()
        {
            Debug.Log("upjump");
            rigid.AddForce(Vector2.up * 25f,ForceMode2D.Impulse);
            rigid.AddForce(Vector2.right * nextmove * 5f,ForceMode2D.Impulse);

        }

        public void DownJump()
        {
            oneWay.DownJump();
            Debug.Log("downjump");
            rigid.AddForce(Vector2.up * 10f,ForceMode2D.Impulse);

        }

        public void Dash(float moveSpeed = 10f)
        {
            Debug.Log("Dash!");
            rigid.constraints &= RigidbodyConstraints2D.FreezePositionX;
            rigid.constraints |= RigidbodyConstraints2D.FreezeRotation;

            if(isFacingLeft){
                nextmove = -1;
            }
            else{
                nextmove = 1;  
            }
            rigid.velocity = new Vector2(nextmove * moveSpeed, rigid.velocity.y);  

        }

        public void Knockback(Vector2 direction)
        {
            rigid.constraints &= RigidbodyConstraints2D.FreezePositionX;

            float knockbackForce = 30f;

            Debug.Log("knockback");

            rigid.velocity = Vector2.zero; 
            rigid.AddForce(-direction * knockbackForce , ForceMode2D.Impulse);
            rigid.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);

        }


        public void MoveVertical(float moveSpeed = 2f)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, nextmove * moveSpeed);   
        }
        public void Fly(float moveSpeed = 2f)
        {
            // 일정 범위 내에서 위아래로 이동하기 위한 코드 추가
            float maxY = startPosition.y + maxFlyDistance;
            float minY = startPosition.y - maxFlyDistance;
            // 현재 위치가 일정 범위를 벗어나면 방향을 바꿔줍니다.
            if (transform.position.y >= maxY || transform.position.y <= minY)
            {
                nextmove *= -1;
            }   
            rigid.velocity = new Vector2( rigid.velocity.x, nextmove * moveSpeed);  
        }

        public void Flip()
        {
            spriteRenderer.flipX = isFacingLeft == true;
            isFacingLeft = !isFacingLeft;
        }

        public void Turn()
        {
            nextmove = nextmove * -1;
            spriteRenderer.flipX = isFacingLeft == true;
            isFacingLeft = !isFacingLeft;
        }
           
       


        public void EndKnockback()
        {
            Debug.Log("Endknockback");
            Turn();
        }
    }
}
