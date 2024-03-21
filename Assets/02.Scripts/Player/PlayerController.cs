using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    float moveX;
    Rigidbody2D rb;
    bool isGround, isHead, isBase, isPushDownKey;
    bool isJump = false;
    bool isDodge = false;
    bool facingRight = true;
    bool isAttacked = false;

    [Header("Base")]
    public float jumpPower;
    public float checkRadius;

    [Header("")]
    public Transform headCheck;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    [Header("Shot")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    Vector2 moveVec;
    Vector2 dodgeVec;

    StageController stageController;

    BoxCollider2D playerCollider;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        playerCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stageController = GameObject.FindWithTag("GameController").GetComponent<StageController>();

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
            isPushDownKey = true;
        }

        if (GameManager.Instance.PlayerHp <= 0) { Die(); }

        // [ ������(ȸ��) ]
        Dodge();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            //platform = collision.gameObject;

            //print("collisionEnter -> " + platform.name);
            //playerCollider.isTrigger = true;
        }
        else
        {
            isAttacked = true;

            //if (GameManager.Instance.PlayerHp <= 0) { Die(); }
            MinusHp(collision.transform.tag);

            StartCoroutine(NoAttack());
        }

        if (collision.gameObject.name == "Lever")
        {
            stageController.isLever = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerCollider.isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerCollider.isTrigger = false;

        if (collision.gameObject.name == "Lever")
        {
            stageController.isLever = true;
        }
        else
        {
            isAttacked = true;

            //if (GameManager.Instance.PlayerHp <= 0) { Die(); }
            MinusHp(collision.transform.tag);

            StartCoroutine(NoAttack());
        }
    }

    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        isHead = Physics2D.OverlapCircle(headCheck.position, checkRadius, groundLayer);
        isBase = Physics2D.OverlapCircle(groundCheck.position, checkRadius, wallLayer);

        if (isHead)
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }

        if (isGround || isBase)
        {
            //anim.SetBool("isJump", false);
            isJump = false;
        }

        if (isPushDownKey && isGround)
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            isPushDownKey = false;
        }

        moveX = Input.GetAxisRaw("Horizontal");
        Move();

        if (CrosshairCursor.instance.mouseCursorPos.x > 0 && !facingRight) { Flip(); }
        else if (CrosshairCursor.instance.mouseCursorPos.x < 0 && facingRight) { Flip(); }
    }

    // [ �̵� ]
    void Move()
    {
        moveVec = new Vector2(moveX * GameManager.Instance.PlayerSpeed, rb.velocity.y)/*.normalized*/;

        rb.velocity = moveVec;

        if (isDodge)
            moveVec = dodgeVec;

        //anim.SetBool("isRun", moveVec != Vector2.zero);
        //anim.SetBool("isWalk", wDown);
    }

    // [ ���� ]
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if ((isGround && !isDodge) || isBase)
            {
                rb.AddForce(Vector2.up * jumpPower);
                //anim.SetBool("isJump", true);
                //anim.SetTrigger("doJump");
                isJump = true;
            }
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
            GameManager.Instance.PlayerSpeed *= 2;
            //anim.SetTrigger("doDodge");

            isDodge = true;
            transform.GetChild(0).GetComponent<Shooting>().enabled = false;

            StartCoroutine(DodgeOut()); //�뷱�� ������
        }
    }
    IEnumerator DodgeOut()
    {
        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.PlayerSpeed *= 0.5f;

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

    void MinusHp(string tag)
    {
        switch (tag)
        {
            case "EnemyBullet":
                GameManager.Instance.PlayerHp -= 1;
                StartCoroutine(AttackedEffect());
                return;
            //case "��������":
            //    hp -= 1;
            //    return;
            case "Enemy":
                GameManager.Instance.PlayerHp -= 0.5f;
                StartCoroutine(AttackedEffect());
                return;
        }

        isAttacked = false;
    }

    IEnumerator AttackedEffect()
    {
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = new Color32(243, 114, 114, 255);
            yield return new WaitForSeconds(0.1f);

            spriteRenderer.color = new Color32(255, 255, 255, 255);
            yield return new WaitForSeconds(0.1f);
        }
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

        SceneManager.LoadScene(1);
    }
}
