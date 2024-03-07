using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid; 
    SpriteRenderer spriteRenderer;
    public int nextmove;

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
        Vector2 frontVec = new Vector2(rigid.position.x + nextmove*0.2f, rigid.position.y);
        Debug.DrawRay (frontVec, Vector3.down, new Color(0 ,1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if(rayHit.collider == null){
            Turn();
        }
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
}
