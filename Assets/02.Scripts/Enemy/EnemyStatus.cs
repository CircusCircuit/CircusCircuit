using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EnemyStatus{
    public class EnemyStatus : MonoBehaviour
    {
        private float defaultEnemyHP = 10;
        private float attackedDamage = 1;
        private float currentEnemyHP;

        public bool isDetected = false;
        private float cooldownTimer = 0f;
        // Start is called before the first frame update
        void Start()
        {
            currentEnemyHP = defaultEnemyHP;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            
        }
        void OnTriggerEnter(Collider other){
            if (other.CompareTag("playerbullet")){
                TakeDamage(attackedDamage = 1);
                Debug.Log("ahh");
            }
        }
        void TakeDamage(float damage){
            currentEnemyHP -= damage;
            if (currentEnemyHP <=0){
                Die();
            }
        } 
        void Die(){
            Destroy(gameObject);
        }   
    }       
}
