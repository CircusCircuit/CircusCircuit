using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        player = GameObject.FindWithTag("Player");
        bulletTxt = GameObject.Find("Bullet").transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        Lever = GameObject.FindWithTag("GameController").transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
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

        if (Input.GetMouseButtonDown(0) && canFire && GameManager.Instance.CurBulletCount > 0 && !Lever.activeSelf)
        {
            GameManager.Instance.CurBulletCount--;
            canFire = false;
            Instantiate(bullet, bulletTransform.position, Quaternion.identity);
            print("ÀÜ¿© ÃÑ¾Ë: " + GameManager.Instance.CurBulletCount);
            bulletTxt.text = "BULLET X " + GameManager.Instance.CurBulletCount;
        }

        if (GameManager.Instance.CurBulletCount <= 0)
        {
            StartCoroutine(ReloadBullet());
        }
    }

    IEnumerator ReloadBullet()
    {
        yield return new WaitForSeconds(3f);

        GameManager.Instance.CurBulletCount = GameManager.Instance.MaxBullet;
        print("Bullet ÀçÀåÀü, " + GameManager.Instance.CurBulletCount);
        bulletTxt.text = "BULLET X " + GameManager.Instance.CurBulletCount;
    }
}
