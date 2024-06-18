using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;

namespace Enemy
{
    public class BossBase : EnemyBase
    {
        protected bool phase1 =false;
        protected bool phase2 =false;
        protected bool phase3 =false;
        protected override void Start()
        {
            enemyHP = 40;
            base.Start();
            attack = new BossAttack(this, bulletPrefab, G_Bullet);
        }
        protected override void Update(){
            Debug.Log($"p1:{phase1},p2:{phase2},p3:{phase3}");
            if(enemyHP <=20){
                phase1=false;
                phase2=true;
            }
            else if(enemyHP <= 20){
                phase2=false;
                phase3=true;
            }
        }


    }

    public class BossAttack: Attack{
        public BossAttack(EnemyBase enemy, GameObject bulletPrefab, GameObject G_Bullet)
            : base(enemy, bulletPrefab, G_Bullet)
        {
        }
        
        public void FireBulletFW()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            Vector2 directionToPlayer = (player.transform.position - enemy.transform.position).normalized;

            float radius = 1f; // 반지름 값은 적절히 조정하십시오.

            float spawnX = enemy.transform.position.x + directionToPlayer.x * radius;
            float spawnY = enemy.transform.position.y + directionToPlayer.y * radius;

            GameObject bullet = GameObject.Instantiate(bulletPrefab, new Vector2(spawnX, spawnY), Quaternion.identity);

            float bulletSpeed = 3f;
            Vector2 bulletDirection = directionToPlayer;
            bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;

        }
    }
}
