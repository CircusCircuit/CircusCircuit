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
        // [ �ö󰡱� �� ���� ]
        Jump();

        // [ �������� ]
        if (Input.GetKeyDown(KeyCode.S) && isGround)
        {
            platform.GetComponent<BoxCollider2D>().isTrigger = true;

            StartCoroutine(WaitFalling());
        }

        // [ ��ȣ�ۿ� ]
        if (Input.GetKeyDown(KeyCode.F))
        {

        }

        // [ ������(ȸ��) ]
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

    // [ �̵� ]
    void Move()
    {
        moveVec = new Vector2(moveX * moveSpeed, rb.velocity.y)/*.normalized*/;

        rb.velocity = moveVec;

        if (isDodge)
            moveVec = dodgeVec;

        //anim.SetBool("isRun", moveVec != Vector2.zero);
        //anim.SetBool("isWalk", wDown);
    }

    // [ ���� ]
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGround && isDodge == false)
        {
            rb.AddForce(Vector2.up * jumpPower);
            //anim.SetBool("isJump", true);
            //anim.SetTrigger("doJump");
            isJump = true;
        }

        //���� �� ������ ���� ����, ź�� ����, �浹�� ���� ���·� ȸ���Ѵ�.
        StartCoroutine(NoAttack());
    }

    // [ ������ ]
    void Dodge()
    {
        if (Input.GetMouseButtonDown(1) && moveVec != Vector2.zero && isJump == false && isDodge == false)
        {
            dodgeVec = moveVec;
            moveSpeed *= 2;
            //anim.SetTrigger("doDodge");

            isDodge = true;
            transform.GetChild(0).GetComponent<Shooting>().enabled = false;

            StartCoroutine(DodgeOut()); //�뷱�� ������
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


    // [ ������ ]
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
            //case "��������":
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

        //2�ʰ� ��������
        //���� ����, ź��, �浹�� ���� ���·� ȸ��

    }

    void Die()
    {
        print("Die���� ��������1���� �����");

        //é��1�� �������� 1���� �����
    }
}
