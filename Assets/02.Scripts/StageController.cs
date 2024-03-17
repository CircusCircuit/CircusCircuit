using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static CardDropSO;

public class StageController : MonoBehaviour
{
    [SerializeField] GameObject Lever;
    Animator anim;
    bool isLever;

    int cardSetCount = 3;
    [SerializeField] GameObject[] card;

    public CardDropSO cardDropSO;
    CardSO.Murtiple[] showCard;
    CardSO.Murtiple pickCard;

    GameObject invenObj;

    private void Awake()
    {
        //for (int i = 0; i < cardSetCount; i++)
        //{
        //    //card[i] = Lever.transform.GetChild(1).GetChild(i + 2).gameObject;
        //    cardUI[i] = card[i].GetComponent<Button>();
        //}

        invenObj = Lever.transform.GetChild(2).gameObject;

        showCard = new CardSO.Murtiple[cardSetCount];
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.Clear1)
        {
            CreateLever();
        }

        Interaction();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Lever")
        {
            isLever = true;
        }
    }

    void Interaction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            //Ŀư�� ������ �ִϸ��̼�
            //�˾����� ������ ī�� 3��
            StartCoroutine(CurtainCall());
        }
    }

    void CreateLever()
    {
        Lever.gameObject.SetActive(true);
        anim = Lever.transform.GetChild(1).gameObject.GetComponent<Animator>();
    }

    IEnumerator CurtainCall()
    {
        print("CurtainCall");

        anim.SetBool("play", true);
        yield return new WaitForSeconds(0.01f);

        var length = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);

        RandomCard();
    }

    public void SelectCard()
    {
        invenObj.SetActive(true);

        for (int i = 0; i < cardSetCount; i++)
        {
            if (EventSystem.current.currentSelectedGameObject.name == "Card" + (i + 1))
            {
                //print("���� ��ư�� " + EventSystem.current.currentSelectedGameObject.name + " �̰� �����°� " + showCard[i].cardName);
                pickCard =  showCard[i];
            }
        }
    }

    public CardSO.Murtiple SendCard()
    {
        return pickCard;
    }

    public void RandomCard()
    {
        for (int i = 0; i < cardSetCount; i++)
        {
            card[i].GetComponent<Button>().enabled = true;

            showCard[i] = cardDropSO.TimesPick();
            card[i].GetComponent<Image>().sprite = showCard[i].cardImage;
        }
    }
}
