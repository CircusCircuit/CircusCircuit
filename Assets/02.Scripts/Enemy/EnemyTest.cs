using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;

namespace Enemy
{
    public class EnemyTest : MonoBehaviour
    {
        Rigidbody2D rigid;
        SpriteRenderer spriteRenderer;
        private EnemyAttack enemyAttack;


        public bool isAttack = false;
        private bool isFacingLeft = true;
        private bool isKnockback = false;
        public int nextmove = 1;
        public float cooldownTimer = 3f;
        // public float dashDuration = 0.5f;
        Vector2 startPosition; // 시작 위치

        // Start is called before the first frame update
        void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            startPosition = transform.position;
        }
   
        void Move(float moveSpeed = 2f)
        {
            rigid.velocity = new Vector2(nextmove * moveSpeed, 0);   
        }
        void MoveVertical(float moveSpeed = 2f)
        {
            float maxDistance = 0.75f; // 오브젝트와 중앙 사이의 최대 거리

            // 일정 범위 내에서 위아래로 이동하기 위한 코드 추가
            float maxY = startPosition.y + maxDistance;
            float minY = startPosition.y - maxDistance;

            // 현재 위치가 일정 범위를 벗어나면 방향을 바꿔줍니다.
            if (transform.position.y >= maxY || transform.position.y <= minY)
            {
                nextmove *= -1;
            }
        }
        void Stop()
        {
            rigid.velocity = new Vector2(0, 0);
        }
        void Turn()
        {
            nextmove = nextmove * -1;
            spriteRenderer.flipX = nextmove == 1;
            isFacingLeft = !isFacingLeft;
        }
        void UpJump()
        {
            Debug.Log("upjump");
            rigid.velocity = new Vector2(rigid.velocity.x * 3f, 25f);
        }
        void DownJump()
        {
            Debug.Log("downjump");
            rigid.velocity = new Vector2(rigid.velocity.x * 2f, 10f);
        }   
        void Dash(float dashSpeed = 10f)
        {
            rigid.velocity = new Vector2(nextmove * dashSpeed, rigid.velocity.y);
        }
        void DashVertical(float dashSpeed = 10f)
        {
            rigid.velocity = new Vector2( rigid.velocity.x, nextmove * dashSpeed);   
        }
        void Knockback(Vector2 direction)
        {
            float knockbackForce = 5f;
            float knockbackDuration = 1.5f;

            if (!isKnockback)
            {
                isKnockback = true;
                Debug.Log("knockback");
                // Vector2 collisionDirection = (collision.transform.position - transform.position).normalized;

                // knockbackDirection에 주어진 방향으로 힘을 가해서 몬스터를 밀어냅니다.
                rigid.velocity = Vector2.zero; // 이전의 속도를 초기화합니다.
                rigid.AddForce(direction * -knockbackForce * 2f, ForceMode2D.Impulse);

                // knockbackDuration 후에 knockback 상태를 해제합니다.
                Invoke("EndKnockback", knockbackDuration);
            }
        }
        void EndKnockback()
        {
            Debug.Log("Endknockback");
            isKnockback = false;
            nextmove *= -1;
            spriteRenderer.flipX = nextmove == 1;
            isFacingLeft = !isFacingLeft;
        }
    }
}
