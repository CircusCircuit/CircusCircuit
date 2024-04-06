using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    Camera mainCam;
    private Vector3 mousePos;

    public GameObject bullet;
    public Transform bulletTransform;
    public bool canFire;

    private float timer;
    //public float timeBetweenFiring;

    private GameObject player;

    TextMeshProUGUI bulletTxt;
    GameObject Lever;

    GameObject loadingObj;
    Image loadingImg;

    float curTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        player = GameObject.FindWithTag("Player");
        bulletTxt = GameObject.Find("Bullet").transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        Lever = GameObject.FindWithTag("GameController").transform.GetChild(0).gameObject;

        loadingObj = GameObject.FindWithTag("GameController").transform.GetChild(2).GetChild(0).gameObject;
        loadingImg = loadingObj.GetComponent<Image>();
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

        if (Input.GetMouseButtonDown(0) && canFire && GameManager.Instance.CurBulletCount > 0 && !Lever.activeSelf)
        {
            GameManager.Instance.CurBulletCount -= 1;
            bulletTxt.text = "BULLET X " + GameManager.Instance.CurBulletCount;

            Instantiate(bullet, bulletTransform.position, Quaternion.identity);

            canFire = false;
            print("ÀÜ¿© ÃÑ¾Ë: " + GameManager.Instance.CurBulletCount);
        }

        if (GameManager.Instance.CurBulletCount <= 0)
        {
            BeAttacked();
        }
    }

    void BeAttacked()
    {
        canFire = false;

        StartCoroutine(ReloadBullet());
        StartCoroutine(LoadingUI());
    }

    IEnumerator ReloadBullet()
    {
        print("ReloadBullet È£Ãâ");

        yield return new WaitForSeconds(3f);

        GameManager.Instance.CurBulletCount = 0;
        GameManager.Instance.CurBulletCount = GameManager.Instance.MaxBullet;
        //print("Bullet ÀçÀåÀü, " + GameManager.Instance.CurBulletCount);
        bulletTxt.text = "BULLET X " + GameManager.Instance.CurBulletCount;

        curTime = 0;
        loadingImg.fillAmount = 0;

        StopAllCoroutines();
    }

    IEnumerator LoadingUI()
    {
        while (curTime <= 180)
        {
            curTime += Time.deltaTime;
            loadingImg.fillAmount = curTime / 180;

            yield return new WaitForFixedUpdate();
        }
    }
}
