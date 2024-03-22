using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static CardDropSO;

public class StageController : MonoBehaviour
{
    [SerializeField] GameObject Lever;
    [SerializeField] Animator anim;
    public bool isLever;

    [SerializeField] CardController cardController;

    GameObject SuccUI;

    // Start is called before the first frame update
    void Start()
    {
        SuccUI = GameObject.FindWithTag("SuccUI").transform.GetChild(0).gameObject;
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

        if (isLever)
        {
            Interaction();
        }

        if (SceneManager.GetActiveScene().buildIndex == 2 && GameManager.Instance.Clear2)
        {
            SuccUI.gameObject.SetActive(true);
        }
    }

    void Interaction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            //커튼콜 닫히는 애니메이션
            //팝업으로 공격형 카드 3장
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
}
