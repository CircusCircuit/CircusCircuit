using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class BossStatus : MonoBehaviour
    {
        private float defaultEnemyHP = 50;
        //private float attackedDamage = 1;
        private float currentEnemyHP;
        private EnemyAttack enemyAttack;
        private EnemyMove enemyMove;
        private SpriteRenderer spriteRenderer;
        // Start is called before the first frame update
        void Start()
        {
            enemyAttack = GetComponent<EnemyAttack>();
            enemyMove = GetComponent<EnemyMove>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            currentEnemyHP = defaultEnemyHP;
        }

        // Update is called once per frame
        void FixedUpdate()
        {

        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == 7)
            {
                TakeDamage(0.5f);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("get");
            if (other.CompareTag("playerbullet"))
            {
                TakeDamage(GameManager.Instance.M_AttackDamage);
                Debug.Log("ahh");
            }
        }
        public void TakeDamage(float damage)
        {
            Debug.Log(":(");
            currentEnemyHP -= damage;
            StartCoroutine(AttackedEffect());
            if (currentEnemyHP <= 0)
            {
                Die();
            }
        }
        private void Die()
        {
            // enemyMove.isDying = true;
            StartCoroutine(ShrinkAndDestroy());
        }
        IEnumerator AttackedEffect()
        {
            for (int i = 0; i < 3; i++)
            {
                spriteRenderer.color = new Color32(243, 114, 114, 255);
                yield return new WaitForSeconds(0.1f);

                spriteRenderer.color = new Color32(255, 255, 255, 255);
                yield return new WaitForSeconds(0.1f);
            }
        }
        IEnumerator ShrinkAndDestroy()
        {
            // 시작 스프라이트 크기
            Vector3 originalScale = transform.localScale;

            // 스프라이트 크기를 줄여가면서 점진적으로 사라지게 함
            for (float t = 0.5f; t >= 0; t -= 2*Time.deltaTime)
            {
                transform.localScale = originalScale * t;
                yield return null;
            }

            // 스프라이트가 완전히 사라진 후 게임 오브젝트를 파괴
            Destroy(gameObject);
        }
    }
}
