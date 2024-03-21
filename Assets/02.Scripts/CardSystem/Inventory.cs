using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    CardSO.Murtiple getCard;
    CardSO.Murtiple[] card;
    public GameObject[] slot;
    CardController cardController;

    int slotCount = 0;
    int maxSlot = 15;
    GameObject slotName;

    int[] pickedSlotNumb = new int[5];
    int pickCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        cardController = GameObject.Find("CardController").GetComponent<CardController>();
        card = new CardSO.Murtiple[maxSlot];

        print(cardController.SendCard().cardName);
        ShowCard();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShowCard()
    {
        getCard = cardController.SendCard();

        if (getCard != null)
        {
            card[slotCount] = getCard;

            slot[slotCount].gameObject.SetActive(true);
            slot[slotCount].GetComponent<Image>().sprite = card[slotCount].cardImage;
            slotCount++;

            getCard = null;
        }
    }

    public void ChooseCard()
    {
        if (pickCount > 5)
        {
            //5개 이상 선택 -> 선택 초기화
            for (int i = 0; i < 5; i++)
            {
                GameObject.Find(i.ToString()).transform.GetChild(0).gameObject.SetActive(false);
                pickedSlotNumb[i] = 0;
            }
            pickCount = 0;

            return;
        }

        else
        {
            slotName = EventSystem.current.currentSelectedGameObject;
            slotName.transform.GetChild(0).gameObject.SetActive(true);

            pickedSlotNumb[pickCount++] = int.Parse(slotName.name);

            if (pickCount == 5)
            {
                //5개 카드 선택 -> 합성 -> 강화된 카드 자동 장착
                //동일한 유형의 카드 합성 시 50%확률로 해당 유형의 1.5배 강화
                //ex. 공격200% + 공격200% + 공속300% + 최대총알1 + hp1증가 -> 공격"600%, 공속300%, 최대총알1, hp1증가
                MergeCard();

                return;
            }

            //5개 미만 카드 선택 -> 자동 장착
            //(소모형 카드의 경우, 장착 시 자동 삭제됨.(-> 재장착 불가)
            //ex. 공격200% + 공격200% 장착하면 -> 공격 4배됨

        }
    }

    public void UnUseCard()
    {
        pickedSlotNumb[pickCount - 1] = 0;
        pickCount--;
        EventSystem.current.currentSelectedGameObject.SetActive(false);
        //배열에서 빼기
    }

    public void InvenOkButton()
    {
        // 선택한 카드가 없으면
        if (pickCount == 0)
        {
            this.gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            return;
        }

        GameManager.Instance.IsCardEnhance = true;
        //현재 스테이지 깨고 다음 스테이지로 이동하면 공격계수 초기화

        CardAdaptionCalc();

        this.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


        //사용한거 버려야함!!!
    }

    void CardAdaptionCalc()
    {
        for (int i = 0; i < pickCount; i++)
        {
            var adapt = card[pickedSlotNumb[i] - 1];

            switch (adapt.Atype)
            {
                case CardSO.Murtiple.AttackType.FightPower:
                    //몬스터 데미지 배로 가함.
                    return;

                case CardSO.Murtiple.AttackType.FightSpeed:
                    GameManager.Instance.AttackSpeed /= adapt.increase / 100;
                    return;

                case CardSO.Murtiple.AttackType.MoveSpeed:
                    GameManager.Instance.PlayerSpeed *= adapt.increase / 100;
                    return;

                case CardSO.Murtiple.AttackType.MaxBullet:
                    GameManager.Instance.MaxBullet += adapt.increase;
                    return;
            }
        }
    }

    void MergeCard()
    {

    }
}
