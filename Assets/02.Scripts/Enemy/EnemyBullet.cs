using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed;
    public float destroyTime = 2f; // 총알이 생성된 후 파괴될 시간

    // Start is called before the first frame update
    void Start()
    {
        // 일정 시간 후에 총알을 파괴하는 Invoke 함수 호출
        Invoke("DestroyBullet", destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        MoveBullet();
    }

    // 총알을 이동시키는 함수
    void MoveBullet()
    {
        // 총알을 왼쪽으로 이동시킵니다.
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    // 총알이 충돌하면 호출되는 함수
    void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 객체가 플랫폼이면 총알을 파괴합니다.
        if (other.CompareTag("Platform"))
        {
            DestroyBullet();
        }
    }

    // 총알을 파괴하는 함수
    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
