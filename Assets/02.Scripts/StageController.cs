using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static CardDropSO;
using Random = UnityEngine.Random;

public class StageController : MonoBehaviour
{
    public AudioClip reverSound; 
    public AudioClip curtainCloseSound; 
    public AudioClip curtainOpenSound; 



    private AudioSource audioSource; // AudioSource 컴포넌트
    public static StageController Instance { get; private set; }

    [SerializeField] GameObject Lever;
    public bool isLever;
    [SerializeField] Animator anim;
    [SerializeField] GameObject yellowCurtain;

    [SerializeField] CardController cardController;

    GameObject SuccUI;
    [SerializeField] GameObject openCurtain;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(curtainOpenSound);
        
        if (openCurtain == null) { return; }
        openCurtain.SetActive(true);
        StartCoroutine(OpeningCurtain());
        
        if (GameObject.FindWithTag("SuccUI") == null) return;
        SuccUI = GameObject.FindWithTag("SuccUI").transform.GetChild(0).gameObject;
    }

    IEnumerator OpeningCurtain()
    {
        yield return new WaitForSeconds(1.5f);

        openCurtain.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.Clear1 && SceneManager.GetActiveScene().buildIndex == 1)
        {
            CreateLever();
        }
        if (GameManager.Instance.Clear2 && SceneManager.GetActiveScene().buildIndex == 2)
        {
            CreateLever();
        }
        if (GameManager.Instance.Clear3 && SceneManager.GetActiveScene().buildIndex == 3)
        {
            CreateLever();
        }
        if (GameManager.Instance.Clear4 && SceneManager.GetActiveScene().buildIndex == 4)
        {
            CreateLever();
        }

        if (isLever)
        {
            Interaction();
        }

        if (SceneManager.GetActiveScene().buildIndex == 4 && GameManager.Instance.Clear4)
        {
            SuccUI.gameObject.SetActive(true);
        }
    }

    void Interaction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            audioSource.PlayOneShot(reverSound);

            //커튼콜 닫히는 애니메이션
            //팝업으로 공격형 카드 3장
            audioSource.PlayOneShot(curtainCloseSound);
            StartCoroutine(CurtainCall());
        }
    }

    void CreateLever()
    {
        Lever.gameObject.SetActive(true);
    }

    IEnumerator CurtainCall()
    {
        print("CurtainCall");

        anim.SetBool("play", true);
        yield return new WaitForSeconds(0.01f);

        var length = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);

        cardController.RandomCard();
        Cursor.visible = true;
    }




    //Whether go to Event Stage
    public void CheckFetherCondition()
    {
        if (GameManager.Instance.FreeFeather < 15)
        {
            GameObject.FindGameObjectWithTag("Inventory").transform.GetChild(0).gameObject.SetActive(false);
            yellowCurtain.SetActive(true);
            yellowCurtain.transform.GetChild(2).GetComponent<Button>().enabled = true;
            yellowCurtain.transform.GetChild(3).GetComponent<Button>().enabled = true;
        }
    }

    public void SelectBurfFeather()
    {
        //깃털 버프, 공격력 디버프
        DeBurfAttackCard("Deburf");
        SceneManager.LoadScene("EventStage");
    }

    void DeBurfAttackCard(string type)
    {
        int rand = Random.Range(0, 2 + 1);

        switch (rand)
        {
            case 0:
                if (type == "Deburf")
                {
                    GameManager.Instance.AttackPoewr /= 2;
                }
                if (type == "Burf")
                {
                    GameManager.Instance.AttackPoewr *= 2;
                }
                return;

            case 1:
                if (type == "Deburf")
                {
                    GameManager.Instance.AttackSpeed /= 2;
                }
                if (type == "Burf")
                {
                    GameManager.Instance.AttackSpeed *= 2;
                }
                return;

            case 2:
                if (type == "Deburf")
                {
                    GameManager.Instance.PlayerSpeed /= 2;
                }
                if (type == "Burf")
                {
                    GameManager.Instance.PlayerSpeed *= 2;
                }
                return;
        }
    }

    public void SelectDeburfFeather()
    {
        //깃털 디버프, 공격력 버프
        GameManager.Instance.FreeFeather /= 2;
        DeBurfAttackCard("Burf");

        GameObject.FindGameObjectWithTag("Inventory").transform.GetChild(0).gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
