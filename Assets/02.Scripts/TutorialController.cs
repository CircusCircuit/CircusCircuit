using Enemy;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    GameObject player;
    Camera mainCam;

    [Header("")]
    [SerializeField] GameObject moveTutoUI;
    Animator moveAnim;
    [SerializeField] GameObject moveTutoObj;
    [SerializeField] GameObject dodgeTutoUI;
    Animator dodgeAnim;
    [SerializeField] GameObject shootTutoUI;
    Animator shootAnim;
    [SerializeField] GameObject attackTutoUI;

    [Header("")]
    [SerializeField] GameObject ReadyUI;
    [SerializeField] GameObject SkipUI;

    Vector3 mousePos;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletTransform;
    float timer;
    bool canFire;

    bool isAttackTuto = false;
    bool isClear = false;

    bool[] doTuto = new bool[6]; // 0:move, 1:Jump, 2:Down, 3:Dodge, 4:Shooting, 5:Attack

    bool[] clearSound = { false, false, false, false, false, false };

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.PlayerHp = 1000;

        Cursor.visible = false;

        player = GameObject.FindWithTag("Player");
        mainCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        moveAnim = moveTutoUI.GetComponent<Animator>();
        dodgeAnim = dodgeTutoUI.GetComponent<Animator>();
        shootAnim = shootTutoUI.GetComponent<Animator>();

        SoundManager.instance.Play("Tutorial_BGM", 0.6f, SoundType.BGM);
        StartCoroutine(WaitForStartTutorial());
    }

    IEnumerator WaitForStartTutorial()
    {
        yield return new WaitForSeconds(0.2f);

        SoundManager.instance.Play("UI_guide_01");

        moveTutoUI.SetActive(true);
        moveAnim.SetBool("doHorizon", true);
        moveTutoObj.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isClear) { return; }
        if (moveTutoUI.activeSelf || dodgeTutoUI.activeSelf)
        {
            GameObject.Find("MoveTutorial").transform.position = new Vector2(player.transform.position.x + 1, player.transform.position.y + 1);
        }

        if (player.transform.localPosition.x > 0.2f && player.transform.localPosition.x < 1f)
        {
            doTuto[0] = true;
            Check();
        }
        if (Input.GetKeyDown(KeyCode.W) && doTuto[0])
        {
            doTuto[1] = true;
            Check();
        }
        if (Input.GetKeyDown(KeyCode.S) && doTuto[0] && doTuto[1])
        {
            doTuto[2] = true;
            Check();
        }
        if (GameManager.Instance.PlayerSpeed == 10 && doTuto[0] && doTuto[1] && doTuto[2])
        {
            doTuto[3] = true;
            Check();
        }
        if (doTuto[0] && doTuto[1] && doTuto[2] && doTuto[3])
        {
            ShootTuto();
        }
        if (isAttackTuto && attackTutoUI == null)
        {
            SoundManager.instance.Play("Tutorial_Finish_01");

            Debug.Log("End Tutorial");
            ReadyUI.SetActive(true);
            SkipUI.SetActive(false);
            Cursor.visible = true;
            isClear = true;

            //GameManager.Instance.PlayerHp = 4;
        }
    }

    void Check()
    {
        if (doTuto[0] && !clearSound[0])
        {
            GuideSound();
            clearSound[0] = true;

            moveAnim.SetBool("doHorizon", false);
            moveTutoObj.SetActive(false);
            moveAnim.SetBool("doUp", true);
        }
        if (doTuto[1] && !clearSound[1])
        {
            GuideSound();
            clearSound[1] = true;

            moveAnim.SetBool("doUp", false);
            moveAnim.SetBool("doDown", true);
        }
        if (doTuto[2] && !clearSound[2])
        {
            GuideSound();
            clearSound[2] = true;

            moveTutoUI.SetActive(false);

            dodgeTutoUI.SetActive(true);
            dodgeAnim.SetBool("doSpace", true);
        }
        if (doTuto[3] && !clearSound[3])
        {
            GuideSound();
            clearSound[3] = true;

            dodgeTutoUI.SetActive(false);
            shootTutoUI.SetActive(true);
            shootAnim.SetBool("doPointer", true);
        }
        if (doTuto[4] && !clearSound[4])
        {
            GuideSound();
            clearSound[4] = true;

            shootTutoUI.SetActive(false);
            attackTutoUI.SetActive(true);
            isAttackTuto = true;
        }
    }

    void ShootTuto()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        bulletTransform.transform.rotation = Quaternion.Euler(0, 0, rotZ);
        bulletTransform.transform.position = new Vector2(player.transform.position.x, player.transform.position.y);

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
            SoundManager.instance.Play("SFX_Gunshot_01");

            Instantiate(bullet, bulletTransform.position, Quaternion.identity);
            canFire = false;

            doTuto[4] = true;
            Check();
        }
    }

    void GuideSound()
    {
        SoundManager.instance.Play("UI_guide_01");
    }
}
