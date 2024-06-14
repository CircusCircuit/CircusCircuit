using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    Camera mainCam;
    private Vector3 mousePos;

    public GameObject bullet;
    public Transform bulletTransform;
    public bool canFire;
    public bool isPlayed = false;

    bool facingRight = true;

    public AudioClip shootSound;
    public AudioClip reloadSound;

    private AudioSource audioSource; // AudioSource ????


    private float timer;
    //public float timeBetweenFiring;

    private GameObject player;

    TextMeshProUGUI bulletTxt;
    //GameObject Lever;

    [SerializeField] GameObject loadingObj;
    Image loadingImg;

    string curSenceIdx;

    // Start is called before the first frame update
    void Start()
    {
        curSenceIdx = SceneManager.GetActiveScene().name;

        mainCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        player = GameObject.FindWithTag("Player");

        if (curSenceIdx != "Tutorial")
        {
            bulletTxt = GameObject.Find("Bullet").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            bulletTxt.text = "BULLET X " + GameManager.Instance.CurBulletCount;
        }


        //이부분 수정필요
        //Lever = GameObject.FindWithTag("GameController").transform.GetChild(0).gameObject;

        //loadingObj = GameObject.FindWithTag("GameController").transform.GetChild(2).GetChild(0).gameObject;
        loadingImg = loadingObj.GetComponent<Image>();

        audioSource = GetComponent<AudioSource>();

        //curSenceIdx = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
        transform.position = new Vector2(player.transform.position.x, player.transform.position.y);

        loadingObj.transform.position = Camera.main.WorldToScreenPoint(player.transform.position + new Vector3(0, 1.5f, 0));


        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > GameManager.Instance.AttackSpeed)
            {
                canFire = true;
                timer = 0;
            }
        }



        if (CrosshairCursor.instance.mouseCursorPos.x > 0 && !facingRight) { GunFlip(); }
        else if (CrosshairCursor.instance.mouseCursorPos.x < 0 && facingRight) { GunFlip(); }



        if (Input.GetMouseButtonDown(0) && canFire && GameManager.Instance.CurBulletCount > 0 /*&& !Lever.activeSelf*/)
        {

            //audioSource.PlayOneShot(shootSound);

            GameManager.Instance.CurBulletCount -= 1;
            if (curSenceIdx != "Tutorial")
            {
                bulletTxt.text = "BULLET X " + GameManager.Instance.CurBulletCount;
            }

            Instantiate(bullet, bulletTransform.position, Quaternion.identity);

            canFire = false;
            //print("잔여 총알: " + GameManager.Instance.CurBulletCount);
        }


        if (GameManager.Instance.CurBulletCount <= 0)
        {
            if (!isPlayed)
            {
                audioSource.PlayOneShot(reloadSound);
                Invoke("AudioLoop", 0.7f);
                isPlayed = true;
            }
            BeAttacked();
        }

    }
    void AudioLoop()
    {
        isPlayed = false;
    }
    void BeAttacked()
    {
        canFire = false;

        StartCoroutine(ReloadBullet());
        StartCoroutine(LoadingUI());
    }

    IEnumerator ReloadBullet()
    {
        //print("ReloadBullet 호출");

        yield return new WaitForSeconds(3f);

        GameManager.Instance.CurBulletCount = 0;
        GameManager.Instance.CurBulletCount = GameManager.Instance.MaxBullet;
        //print("Bullet 재장전, " + GameManager.Instance.CurBulletCount);
        bulletTxt.text = "BULLET X " + GameManager.Instance.CurBulletCount;
        //curTime = 0;
        loadingImg.fillAmount = 0;
        loadingObj.SetActive(false);

        //StopAllCoroutines();
    }

    IEnumerator LoadingUI()
    {
        loadingObj.SetActive(true);

        float curTime = 0;
        float totalTime = 3;

        while (curTime <= totalTime)
        {
            curTime += Time.deltaTime;
            loadingImg.fillAmount = curTime / totalTime;

            yield return /*new WaitForFixedUpdate()*/null;
        }
    }

    public void GunFlip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.y *= -1;
        transform.localScale = scale;
    }
}
