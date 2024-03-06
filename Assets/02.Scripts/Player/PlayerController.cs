using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float moveX;
    Rigidbody2D rb;
    bool isGround, isHead, isBase;
    bool facingRight = true;

    [Header("Base")]
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
        if (Input.GetKeyDown(KeyCode.W) && isGround)
        {
            //print("Jump");

            rb.velocity = Vector2.up * jumpPower;
        }

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

        // [ ����߻� ]
        if (Input.GetMouseButtonDown(0))
        {
            //GameObject bulletClone = Instantiate(bulletPrefab, firePoint.transform.position,
            //    firePoint.transform.rotation);
            //bulletClone.transform.localScale = transform.localScale;
        }

        // [ ������(ȸ��) ]
        if (Input.GetMouseButtonDown(1))
        {

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        platform = collision.gameObject;
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
        }

        moveX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        //if (moveX > 0 && !facingRight) { Flip(); }  //������ �̵��ε� ���� ���� ������ ������
        //else if (moveX < 0 && facingRight) { Flip(); } //���� �̵��ε� ������ ���� ������ ������
    }

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
}
