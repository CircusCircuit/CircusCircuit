using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public Camera mainCam;
    private Vector3 mousePos;

    public GameObject bullet;
    public Transform bulletTransform;
    private int bulletCount = 7;
    public bool canFire;

    private float timer;
    public float timeBetweenFiring;

    private GameObject player;

    [SerializeField] TextMeshProUGUI bulletTxt;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
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
            if (timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }

        if (Input.GetMouseButtonDown(0) && canFire && bulletCount > 0 && GameManager.Instance.Clear1 == false)
        {
            bulletCount--;
            canFire = false;
            Instantiate(bullet, bulletTransform.position, Quaternion.identity);
            print("ÀÜ¿© ÃÑ¾Ë: " + bulletCount);
            bulletTxt.text = "BULLET X " + bulletCount;
        }

        if (bulletCount <= 0)
        {
            StartCoroutine(ReloadBullet());
        }
    }

    IEnumerator ReloadBullet()
    {
        yield return new WaitForSeconds(3f);

        bulletCount = 7;
        print("Bullet ÀçÀåÀü, " + bulletCount);
        bulletTxt.text = "BULLET X " + bulletCount;
    }
}
