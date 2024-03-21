using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy{
    public class EnemyStatus : MonoBehaviour
    {
        private float defaultEnemyHP = 10;
        //private float attackedDamage = 1;
        private float currentEnemyHP;
        private bool isAttacked = false;
        private EnemyAttack enemyAttack;
        private EnemyMove enemyMove;

        private float cooldownTimer = 0f;
        // Start is called before the first frame update
        void Start()
        {
            enemyAttack = GetComponent<EnemyAttack>();
            enemyMove = GetComponent<EnemyMove>();
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
                Debug.Log("attacted");
                TakeDamage(0.5f);
                enemyAttack.FireBullet_8();
                CancelInvoke("enemyMove.Stop");
                enemyMove.nextmove *= -1; 
                enemyMove.isFire = false;
            }
        }
        private void OnTriggerEnter(Collider other){
            Debug.Log("get");
            if (other.CompareTag("playerbullet")){
                // TakeDamage(GameManager.Instance.M_AttackDamage = 1);
                Debug.Log("ahh");
            }
        }
        public void TakeDamage(float damage = 1){
            Debug.Log(":(");
            currentEnemyHP -= damage;
            if (currentEnemyHP <=0){
                Die();
            }
        } 
        private void Die(){
            Destroy(gameObject);
        }   
    }       
}
