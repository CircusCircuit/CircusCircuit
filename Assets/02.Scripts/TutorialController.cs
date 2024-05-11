using Enemy;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    GameObject player;
    Camera mainCam;

    [SerializeField] GameObject moveTutoUI;
    Animator moveAnim;
    [SerializeField] GameObject moveTutoObj;
    [SerializeField] GameObject dodgeTutoUI;
    Animator dodgeAnim;
    [SerializeField] GameObject shootTutoUI;
    Animator shootAnim;
    //[SerializeField] GameObject attackTutoUI;
    //Animator attackAnim;
    //[SerializeField] GameObject useCardTutoUI;
    //Animator useCardAnim;


    Vector3 mousePos;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletTransform;
    float timer;
    bool canFire;

    bool[] doTuto = new bool[7]; // 0:move, 1:Jump, 2:Down, 3:Dodge, 4:Shooting, 5:Attack, 6:UseCard

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        mainCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        moveAnim = moveTutoUI.GetComponent<Animator>();
        dodgeAnim = dodgeTutoUI.GetComponent<Animator>();
        shootAnim = shootTutoUI.GetComponent<Animator>();
        //attackAnim = attackTutoUI.GetComponent<Animator>();
        //useCardAnim = useCardTutoUI.GetComponent<Animator>();

        StartCoroutine(MoveTuto());
        StartCoroutine(DodgeTuto());
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.localPosition.x > 0.6f && player.transform.localPosition.x < 1.6f)
        {
            doTuto[0] = true;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            doTuto[1] = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            doTuto[2] = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            doTuto[3] = true;
        }
    }

    IEnumerator MoveTuto()
    {
        moveTutoUI.SetActive(true);

        moveAnim.SetBool("doHorizon", true);
        moveTutoObj.SetActive(true);
        //while (!doTuto[0]) { }
        //moveAnim.SetBool("doHorizon", false);

        //moveAnim.SetBool("doJump", true);
        //while (!doTuto[1]) { }
        //moveAnim.SetBool("doJump", false);

        //moveAnim.SetBool("doDown", true);
        //while (!doTuto[2]) { }
        //moveAnim.SetBool("doDown", false);

        //moveTutoUI.SetActive(false);

        yield return StartCoroutine(MoveTuto());
    }

    IEnumerator DodgeTuto()
    {
        dodgeTutoUI.SetActive(true);

        dodgeAnim.SetBool("doDodge", true);
        //while (!doTuto[3]) { }
        dodgeTutoUI.SetActive(false);

        yield return StartCoroutine(DodgeTuto());
    }

    void ShootTuto()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
        transform.position = new Vector2(player.transform.position.x, player.transform.position.y);

        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > GameManager.Instance.AttackSpeed)
            {
                canFire = true;
                timer = 0;
            }

        }

        if (Input.GetMouseButtonDown(0) && canFire)
        {
            Instantiate(bullet, bulletTransform.position, Quaternion.identity);
            canFire = false;

            doTuto[4] = true;
        }
    }
}
