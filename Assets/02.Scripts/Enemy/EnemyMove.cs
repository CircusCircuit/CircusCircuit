using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy{
    public class EnemyMove : MonoBehaviour
    {
        Rigidbody2D rigid; 
        SpriteRenderer spriteRenderer;
        public GameObject bulletPrefab;
        public int nextmove;
        public bool detected = false;


        // Start is called before the first frame update
        void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            Invoke("Think",3);
        }

        void Updata(){
            
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //move
            rigid.velocity = new Vector2(nextmove*3, rigid.velocity.y);

            //Platform Check
            if(detected == false){
                Vector2 frontVec = new Vector2(rigid.position.x + nextmove*0.2f, rigid.position.y);
                Debug.DrawRay (frontVec, Vector3.down, new Color(0 ,1, 0));
                RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));
                if(rayHit.collider == null){
                    Turn();
                }
            }
            
            //Player Check
            DetectPlayerInRange(3, true);

            FireBullet();
        
        }

        //몬스터 행동 결정 함수, 재귀
        void Think()
        {
            //다음 활동 설정
            nextmove = Random.Range(-1, 2);
            
            //방향전환
            if(nextmove != 0){
                spriteRenderer.flipX = nextmove == 1;
            }

            //재귀
            Invoke("Think", 3);
        }

        void Turn(){
            Debug.Log("Turn!");
            nextmove = nextmove * -1;
            spriteRenderer.flipX = nextmove == 1;
            CancelInvoke("Think");
            Invoke("Think",2);
        }

        void DetectPlayerInRange(float detectionRange = 10f, bool horizontal = false)
        {
            // 플레이어의 위치
            Vector2 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

            // 몬스터와 플레이어의 거리 계산
            float distanceToPlayerX = Mathf.Abs(playerPosition.x - transform.position.x);
            float distanceToPlayerY = Mathf.Abs(playerPosition.y - transform.position.y);
            
            if (horizontal == false) {
                // 만약 플레이어가 감지 범위 내에 있다면
                if (distanceToPlayerX <= detectionRange)
                {
                    // 플레이어를 감지했을 때 수행할 동작을 추가합니다.
                    Debug.Log("Player detected!");
                }
            }
            else{
                if (nextmove > 0) {
                    if(distanceToPlayerY <= 5f){
                        if (playerPosition.x > transform.position.x && distanceToPlayerX <= detectionRange){
                            Debug.Log("Player detected!");
                            detected = true;
                            CancelInvoke("Think");
                            nextmove = 5;
                        }
                    }            
                }
                else {
                    if(distanceToPlayerY <= 5f){
                        if (playerPosition.x < transform.position.x && distanceToPlayerX <= detectionRange){
                            nextmove = -5;
                            detected = true;
                            CancelInvoke("Think");
                            Debug.Log("Player detected!");
                        }
                    }
                }
            }
        }


        void FireBullet()
        {
            Debug.Log("fire!");
            if (Random.value < 0.1f) // 1% 확률로 발사
            {
                for (int i = 0; i < 8; i++)
                {
                    // 각 방향에 따른 회전 각도
                    float rotation = i * 45f;

                    // 총알을 회전시켜 생성합니다.
                    float radius = 0.7f; // 반지름 값은 적절히 조정하십시오.

                    // 원 주위의 랜덤한 위치 계산
                    float spawnX = transform.position.x + radius * Mathf.Cos(rotation * Mathf.Deg2Rad);
                    float spawnY = transform.position.y + radius * Mathf.Sin(rotation * Mathf.Deg2Rad);

                    // 오브젝트 생성
                    GameObject bullet = Instantiate(bulletPrefab, new Vector2(spawnX, spawnY), Quaternion.identity);
                    // 총알의 초기 속도 설정
                    float bulletSpeed = 10f;
                    float bulletDirectionX = Mathf.Cos(Mathf.Deg2Rad * rotation);
                    float bulletDirectionY = Mathf.Sin(Mathf.Deg2Rad * rotation);
                    Vector2 bulletDirection = new Vector2(bulletDirectionX, bulletDirectionY).normalized;
                    bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;
                }
            }
        }
    }

}
