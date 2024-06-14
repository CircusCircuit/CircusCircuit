using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace controller
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance = null;
        float moveX;
        Rigidbody2D rb;
        bool isGround, isHead, isLeft, isRight, isBase, isSky, isPushDownKey, isDodgeDelay;
        bool isJump = false;
        bool isDodge = false;
        bool facingRight = true;
        bool isAttacked = false;
        bool doDownDelay = false;

        [Header("Base")]
        public float jumpPower;
        public float checkRadius;

        [Header("")]
        public Transform headCheck;
        public Transform groundCheck;
        public Transform leftCheck;
        public Transform rightCheck;
        public LayerMask groundLayer;
        public LayerMask wallLayer;

        [Header("Shoot")]
        public GameObject bulletPrefab;
        public Transform firePoint;

        Vector2 moveVec;
        Vector2 dodgeVec;
        Vector2 curPos;

        //StageController stageController;
        [SerializeField] Shooting shooting;

        BoxCollider2D playerCollider;
        SpriteRenderer spriteRenderer;
        Animator playerAnim;
        GameObject currentOneWayPlatform;

        GameObject FailUI;

        enum States
        {
            Idle,
            Jumping,
            Falling,
            Dodging
        }
        States curState;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            playerCollider = GetComponent<BoxCollider2D>();
            spriteRenderer = transform.GetChild(2).GetComponent<SpriteRenderer>();
            playerAnim = GetComponent<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            //if (SceneManager.GetActiveScene().name != "Tutorial")
            //{
            //    stageController = GameObject.FindWithTag("GameController").GetComponent<StageController>();
            //}

            if (GameObject.FindWithTag("FailUI") == null) return;
            FailUI = GameObject.FindWithTag("FailUI").transform.GetChild(0).gameObject;

            curState = States.Idle;
        }

        // Update is called once per frame
        void Update()
        {
            // [ 올라가기 및 점프 ]
            if (curState == States.Idle && Input.GetKeyDown(KeyCode.W))
            {
                Jump();
            }


            // [ 내려오기 ]
            if (Input.GetKeyDown(KeyCode.S) && isGround)
            {
                isPushDownKey = true;

                curState = States.Falling;
            }

            if (GameManager.Instance.PlayerHp <= 0)
            {
                Die();
            }

            // [ 구르기(회피) ]
            Dodge();
            if (curState == States.Dodging)
            {
                if (!isBase || !isGround)
                {
                    rb.gravityScale = 4;
                    playerCollider.isTrigger = false;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.name.Contains("Ground"))
            {
                playerCollider.isTrigger = false;
            }
            if (collision.gameObject.tag == "Enemy") return;
            if (collision.gameObject.CompareTag("Platform"))
            {
                currentOneWayPlatform = collision.gameObject;
            }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Platform"))
            {
                currentOneWayPlatform = null;
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (/*isGround && */!isDodge && collision.gameObject.tag == "Enemy" && !isAttacked)
            {
                //몬스터 충돌 시.
                isAttacked = true;
                MinusHp(collision.transform.tag);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                playerCollider.isTrigger = false;
                rb.gravityScale = 4;
            }
            if (collision.gameObject.tag != "Enemy")
            {
                playerCollider.isTrigger = false;
            }


            if (isGround && !isDodge && collision.gameObject.tag == "EnemyBullet" && !isAttacked)
            {
                //총알 피격 시.
                isAttacked = true;
                MinusHp(collision.transform.tag);
            }
        }

        private void FixedUpdate()
        {
            isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, wallLayer/*groundLayer*/);
            isHead = Physics2D.OverlapCircle(headCheck.position, checkRadius, wallLayer/*groundLayer*/);
            isBase = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer/*wallLayer*/);
            isSky = Physics2D.OverlapCircle(headCheck.position, checkRadius, groundLayer/*wallLayer*/);

            isLeft = Physics2D.OverlapCircle(leftCheck.position, 0.2f, groundLayer);
            isRight = Physics2D.OverlapCircle(rightCheck.position, 0.2f, groundLayer);

            if (isLeft || isRight)
            {
                playerCollider.isTrigger = false;
            }

            if (isHead && !isSky)
            {
                //playerCollider.isTrigger = true;
            }

            if (isGround || isBase)
            {
                isJump = false;
                curState = States.Idle;
            }

            if (isPushDownKey && isGround && !doDownDelay)
            {
                //playerCollider.isTrigger = true;
                if (currentOneWayPlatform != null)
                {
                    StartCoroutine(DisableCollision());
                    doDownDelay = true;
                }

                //rb.AddForce(-transform.up * 15f);
                isPushDownKey = false;

                curState = States.Idle;
            }

            //moveX = Input.GetAxisRaw("Horizontal");
            Move();


            if (CrosshairCursor.instance.mouseCursorPos.x > transform.position.x && !facingRight) { Flip(); }
            else if (CrosshairCursor.instance.mouseCursorPos.x < transform.position.x && facingRight) { Flip(); }
        }

        IEnumerator DisableCollision()
        {
            CompositeCollider2D platformCollider = currentOneWayPlatform.GetComponent<CompositeCollider2D>();

            Physics2D.IgnoreCollision(playerCollider, platformCollider);
            yield return new WaitForSeconds(0.25f);
            Physics2D.IgnoreCollision(playerCollider, platformCollider, false);

            StartCoroutine(DownDelay());
        }

        IEnumerator DownDelay()
        {
            yield return new WaitForSeconds(0.25f);
            doDownDelay = false;

        }

        // [ 이동 ]
        void Move()
        {
            moveX = Input.GetAxisRaw("Horizontal");
            moveVec = new Vector2(moveX * GameManager.Instance.PlayerSpeed, rb.velocity.y)/*.normalized*/;

            rb.velocity = moveVec;

            if (isDodge)
            {
                moveVec = dodgeVec;
            }
            else
            {
                playerAnim.SetBool("isWalk", moveVec != Vector2.zero);
            }
        }

        // [ 점프 ]
        void Jump()
        {
            if ((isGround || isBase) && !isDodgeDelay && !isJump)
            {
                rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                //anim.SetBool("isJump", true);
                //anim.SetTrigger("doJump");
                isJump = true;
                curState = States.Jumping;
                playerAnim.SetTrigger("isJump");
            }

            //점프 중 몬스터의 물리 공격, 탄막 공격, 충돌을 무적 상태로 회피한다.
        }

        // [ 구르기 ]
        void Dodge()
        {
            if (Input.GetKeyDown(KeyCode.Space) && moveVec != Vector2.zero && (isGround || isBase) && !isDodgeDelay)
            {
                curState = States.Dodging;
                playerAnim.SetTrigger("isDodge");
                //playerAnim.SetBool("isWalk", moveVec != Vector2.zero);

                rb.gravityScale = 0;
                playerCollider.isTrigger = true;

                dodgeVec = moveVec;
                GameManager.Instance.PlayerSpeed = 10/**= 1.5f*/;

                isDodge = true;
                shooting.canFire = false;

                StartCoroutine(DodgeOut()); //밸런스 딜레이
            }
        }
        IEnumerator DodgeOut()
        {
            yield return new WaitForSecondsRealtime(0.2f);

            GameManager.Instance.PlayerSpeed = 5/**= 0.5f*/;

            yield return new WaitForFixedUpdate();

            playerCollider.isTrigger = false;
            rb.gravityScale = 4;

            isDodge = false;
            shooting.canFire = true;
            curState = States.Idle;

            isDodgeDelay = true;

            StartCoroutine(DodgeDelay());
        }
        IEnumerator DodgeDelay()
        {
            yield return new WaitForSecondsRealtime(1f);

            isDodgeDelay = false;
        }


        // [ 뒤집기 ]
        void Flip()
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        public void getChangedPos(Vector2 pos)
        {
            curPos = this.transform.position;
            this.transform.position = pos;
            StartCoroutine(MoveEffect());
        }

        IEnumerator MoveEffect()
        {
            yield return new WaitForSeconds(3f);
            this.transform.position = curPos;
        }

        void MinusHp(string tag)
        {
            StartCoroutine(AttackedEffect());
            StartCoroutine(NoAttack(tag));
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


        IEnumerator NoAttack(string tag)
        {
            //2초간 무적상태
            //몬스터 물리, 탄막, 충돌을 무적 상태로 회피

            if (isAttacked)
            {
                if (tag == "EnemyBullet")
                {
                    GameManager.Instance.PlayerHp -= 1;
                }
                if (tag == "Enemy")
                {
                    GameManager.Instance.PlayerHp -= 0.5f;
                }
            }

            //print("hp는 " + GameManager.Instance.PlayerHp);

            yield return new WaitForSeconds(2f);
            isAttacked = false;
        }

        void Die()
        {
            StartCoroutine(RestartScene());
            //if (SceneManager.GetActiveScene().buildIndex == 4)
            //{
            //    FailUI.SetActive(true);
            //}
            //else
            //{
            //    StartCoroutine(RestartScene());
            //}
        }

        IEnumerator RestartScene()
        {
            yield return new WaitForSeconds(0.3f);

            Destroy(GameManager.Instance.gameObject);
            SceneManager.LoadScene(0);
        }
    }
}
