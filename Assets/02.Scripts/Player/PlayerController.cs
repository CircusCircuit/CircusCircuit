using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    float moveX;
    Rigidbody2D rb;
    bool isGround, isHead, isBase, isSky, isPushDownKey;
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
    [SerializeField] Shooting shooting;

    BoxCollider2D playerCollider;
    SpriteRenderer spriteRenderer;

    GameObject FailUI;

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
        FailUI = GameObject.FindWithTag("FailUI").transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // [ 올라가기 및 점프 ]
        Jump();

        // [ 내려오기 ]
        if (Input.GetKeyDown(KeyCode.S) && isGround)
        {
            isPushDownKey = true;
        }

        if (GameManager.Instance.PlayerHp <= 0)
        {
            Die();
        }

        // [ 구르기(회피) ]
        Dodge();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerCollider.isTrigger = false;

        if ((!isDodge || !isJump) && collision.gameObject.tag == "Enemy" && !isAttacked)
        {
            //몬스터 충돌 시.
            isAttacked = true;
            MinusHp(collision.transform.tag);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerCollider.isTrigger = false;

        if ((!isDodge || !isJump) && collision.gameObject.tag == "EnemyBullet" && !isAttacked)
        {
            //총알 피격 시.
            isAttacked = true;
            MinusHp(collision.transform.tag);
        }

        if (collision.gameObject.name == "Lever")
        {
            stageController.isLever = true;
        }
    }

    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        isHead = Physics2D.OverlapCircle(headCheck.position, checkRadius, groundLayer);
        isBase = Physics2D.OverlapCircle(groundCheck.position, checkRadius, wallLayer);
        isSky = Physics2D.OverlapCircle(headCheck.position, checkRadius, wallLayer);

        if (isHead && !isSky)
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

    // [ 이동 ]
    void Move()
    {
        moveVec = new Vector2(moveX * GameManager.Instance.PlayerSpeed, rb.velocity.y)/*.normalized*/;

        rb.velocity = moveVec;

        if (isDodge)
            moveVec = dodgeVec;

        //anim.SetBool("isRun", moveVec != Vector2.zero);
        //anim.SetBool("isWalk", wDown);
    }

    // [ 점프 ]
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

        //점프 중 몬스터의 물리 공격, 탄막 공격, 충돌을 무적 상태로 회피한다.
        StartCoroutine(NoAttack());
    }

    // [ 구르기 ]
    void Dodge()
    {
        if (Input.GetMouseButtonDown(1) && moveVec != Vector2.zero && isJump == false && isDodge == false)
        {
            dodgeVec = moveVec;
            GameManager.Instance.PlayerSpeed *= 2;
            //anim.SetTrigger("doDodge");

            isDodge = true;
            shooting.canFire = false;

            StartCoroutine(DodgeOut()); //밸런스 딜레이
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
        shooting.canFire = true;
    }


    // [ 뒤집기 ]
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
                StartCoroutine(NoAttack());
                return;
            //case "물리공격":
            //    hp -= 1;
            //    return;
            case "Enemy":
                GameManager.Instance.PlayerHp -= 0.5f;
                StartCoroutine(AttackedEffect());
                StartCoroutine(NoAttack());
                return;
        }
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
        //2초간 무적상태
        //몬스터 물리, 탄막, 충돌을 무적 상태로 회피

        yield return new WaitForSeconds(2f);
        isAttacked = false;
    }

    void Die()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            FailUI.SetActive(true);

            StartCoroutine(RestartScene());
        }

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            FailUI.SetActive(true);
        }
    }

    IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(0.3f);

        SceneManager.LoadScene(1);
    }
}
