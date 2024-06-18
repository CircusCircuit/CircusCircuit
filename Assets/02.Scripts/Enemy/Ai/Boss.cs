using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;

namespace Enemy
{
    public class Boss : EnemyBase
    {
        protected int[] phase = {1,0,0};
        protected override void Start()
        {
            enemyHP = 40;
            base.Start();
            attack = new BossAttack(this, bulletPrefab, G_Bullet);
        }
        protected override void Update(){
            if(enemyHP <=20){
                phase[1]=1;
            }
            if(enemyHP <= 10){
                phase[2]=1;
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
