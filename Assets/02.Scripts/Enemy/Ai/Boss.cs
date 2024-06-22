using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;

namespace Enemy
{
    public class Boss : EnemyBase
    {
        public GameObject B_Bullet;
        public GameObject M_Bullet;
        public GameObject F_Bullet;
        protected BossAttack bossAttack;

        protected int[] phase = {1,0,0};
        protected override void Start()
        {
            enemyHP = 40;
            base.Start();
            CancelInvoke();
            bossAttack = new BossAttack(this, bulletPrefab, G_Bullet, M_Bullet, F_Bullet, B_Bullet);
        }
        protected override void Update(){
            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;
            }
            else{
                bossAttack.Patten1();
                cooldownTimer =3f;
            }
            if(enemyHP <=20){
                phase[1]=1;
            }
            if(enemyHP <= 10){
                phase[2]=1;
            }

        }
    }

    public class BossAttack: Attack{
        public GameObject B_Bullet;
        public GameObject M_Bullet;
        public GameObject F_Bullet;       
        public BossAttack(EnemyBase enemy, GameObject bulletPrefab, GameObject G_Bullet, GameObject M_Bullet, 
                            GameObject F_Bullet, GameObject B_Bullet)
            : base(enemy, bulletPrefab, G_Bullet)
        {
            this.F_Bullet = F_Bullet;
            this.B_Bullet = B_Bullet;
            this.M_Bullet = M_Bullet;

        }
        
        public void Patten1(){
            GameObject bullet = GameObject.Instantiate(F_Bullet, new Vector2(0, -1), Quaternion.identity);
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
