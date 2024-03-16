using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float moveX;
    Rigidbody2D rb;
    bool isGround, isHead, isBase;
    bool isJump = false;
    bool isDodge = false;
    bool facingRight = true;
    bool isAttacked = false;

    [Header("Base")]
    float hp = 4;
    public float moveSpeed;
    public float jumpPower;
    public float checkRadius;

    [Header("")]
    public Transform headCheck;
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Shot")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    GameObject platform;

    Vector2 moveVec;
    Vector2 dodgeVec;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        // [ 올라가기 및 점프 ]
        Jump();

        // [ 내려오기 ]
        if (Input.GetKeyDown(KeyCode.S) && isGround)
        {
            platform.GetComponent<BoxCollider2D>().isTrigger = true;

            StartCoroutine(WaitFalling());
        }

        // [ 상호작용 ]
        if (Input.GetKeyDown(KeyCode.F))
        {

        }

        // [ 구르기(회피) ]
        Dodge();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            platform = collision.gameObject;
        }
        else
        {
            isAttacked = true;

            if (hp <= 0) { Die(); }
            MinusHp(collision.transform.tag);

            StartCoroutine(NoAttack());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        isHead = Physics2D.OverlapCircle(headCheck.position, checkRadius, groundLayer);

        if (isHead)
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }

        if (isGround)
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = false;

            //anim.SetBool("isJump", false);
            isJump = false;
        }

        moveX = Input.GetAxisRaw("Horizontal");
        Move();

        if (CrosshairCursor.instance.mouseCursorPos.x > 0 && !facingRight) { Flip(); }
        else if (CrosshairCursor.instance.mouseCursorPos.x < 0 && facingRight) { Flip(); }
    }

    // [ 이동 ]
    void Move()
    {
        moveVec = new Vector2(moveX * moveSpeed, rb.velocity.y)/*.normalized*/;

        rb.velocity = moveVec;

        if (isDodge)
            moveVec = dodgeVec;

        //anim.SetBool("isRun", moveVec != Vector2.zero);
        //anim.SetBool("isWalk", wDown);
    }

    // [ 점프 ]
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGround && isDodge == false)
        {
            rb.AddForce(Vector2.up * jumpPower);
            //anim.SetBool("isJump", true);
            //anim.SetTrigger("doJump");
            isJump = true;
        }

        //점프 중 몬스터의 물리 공격, 탄막 공격, 충돌을 무적 상태로 회피한다.
        StartCoroutine(NoAttack());
    }

    // [ 구르기 ]
    void Dodge()
    {
        if (Input.GetMouseButtonDown(1) && moveVec != Vector2.zero && isJump == false && isDodge == false)
        {
            dodgeVec = moveVec;
            moveSpeed *= 2;
            //anim.SetTrigger("doDodge");

            isDodge = true;
            transform.GetChild(0).GetComponent<Shooting>().enabled = false;

            StartCoroutine(DodgeOut()); //밸런스 딜레이
        }
    }
    IEnumerator DodgeOut()
    {
        yield return new WaitForSeconds(0.5f);

        moveSpeed *= 0.5f;

        StartCoroutine(DodgeDelay());
    }
    IEnumerator DodgeDelay()
    {
        yield return new WaitForSeconds(1f);

        isDodge = false;
        transform.GetChild(0).GetComponent<Shooting>().enabled = true;
    }


    // [ 뒤집기 ]
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    IEnumerator WaitFalling()
    {
        yield return new WaitForSeconds(0.5f);

        platform.GetComponent<BoxCollider2D>().isTrigger = false;
        platform = null;
    }

    void MinusHp(string tag)
    {
        switch (tag)
        {
            case "bullet":
                hp -= 1;
                return;
            //case "물리공격":
            //    hp -= 1;
            //    return;
            case "monster":
                hp -= 0.5f;
                return;
        }

        isAttacked = false;
        getHp();
    }

    public float getHp()
    {
        return hp;
    }

    IEnumerator NoAttack()
    {
        yield return new WaitForSeconds(2f);

        //2초간 무적상태
        //몬스터 물리, 탄막, 충돌을 무적 상태로 회피

    }

    void Die()
    {
        print("Die상태 스테이지1부터 재시작");

        //챕터1의 스테이지 1부터 재시작
    }
}
