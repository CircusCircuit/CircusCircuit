using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    //5개 이상 선택 시, 첫번째 선택 카드 삭제용
    //int[] pickedSlotNumb = new int[5];
    Queue<int> _pickedQueue = new Queue<int>();
    int pickCount = 0;

    int[] pickedCardNumb/* = new int[5]*/;


    // Start is called before the first frame update
    void Start()
    {
        cardController = GameObject.Find("CardController").GetComponent<CardController>();
        card = new CardSO.Murtiple[maxSlot];

        print(cardController.SendCard().cardName);
        ShowCard();
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

    //5개 선택 제어 
    public void ChooseCard()
    {
        slotName = EventSystem.current.currentSelectedGameObject;

        //5개 이상 선택시 -> 첫번째거 빼고 마지막거 선택
        if (_pickedQueue.Count > 5)
        {
            //첫번째로 선택한 카드의 프레임 해제 및 첫번째거 빼기
            if (_pickedQueue == null) return;

            //프레임 해제
            //slot[pickedSlotNumb[0]].transform.GetChild(0).gameObject.SetActive(false);
            slot[_pickedQueue.Peek()].transform.GetChild(0).gameObject.SetActive(false);

            //데이터 빼기
            _pickedQueue.Dequeue();
            //for (int i = 0; i < 4; i++)
            //{
            //    pickedSlotNumb[i] = pickedSlotNumb[i + 1];
            //}
            //pickedSlotNumb[4] = 0;
            //pickCount = 3;
        }

        //선택한 슬롯의 프레임 활성화
        slotName.transform.GetChild(0).gameObject.SetActive(true);

        //pickedSlotNumb[pickCount++] = int.Parse(slotName.name);
        _pickedQueue.Enqueue(int.Parse(slotName.name));
    }

    public void UnUseCard()
    {
        //배열에서 빼기
        //pickedSlotNumb[pickCount - 1] = 0;
        //pickCount--;
        _pickedQueue.Dequeue();
        EventSystem.current.currentSelectedGameObject.SetActive(false);
    }

    public void InvenOkButton()
    {
        SavePickedCard();

        // 선택한 카드가 없으면
        if (pickedCardNumb == null)
        {
            this.gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            return;
        }

        if (pickedCardNumb.Length > 5)
        {
            Debug.Log("카드 선택 5개 초과, -> 카드 적용 오류");
            return;
        }

        if (pickedCardNumb.Length < 5)
        {
            //5개 미만 카드 선택 -> 자동 장착
            //(소모형 카드의 경우, 장착 시 자동 삭제됨.(-> 재장착 불가)
            //ex. 공격200% + 공격200% 장착하면 -> 공격 4배됨
            CardAdaptionCalc();
        }
        if (pickedCardNumb.Length == 5)
        {
            //5개 카드 선택 -> 합성 -> 강화된 카드 자동 장착
            //동일한 유형의 카드 합성 시 50%확률로 해당 유형의 1.5배 강화
            //ex. 공격200% + 공격200% + 공속300% + 최대총알1 + hp1증가 -> 공격"600%, 공속300%, 최대총알1, hp1증가
            MergeCard();
        }

        this.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        Cursor.visible = false;
    }

    void SavePickedCard()
    {
        for (int i = 0; i < slotCount; i++)
        {
            //프레임이 활성화된 슬롯 번호 저장
            if (slot[i].transform.GetChild(0).gameObject.activeSelf)
            {
                pickedCardNumb[i] = int.Parse(slot[i].name) - 1;
            }
        }
    }

    void CardAdaptionCalc()
    {
        //초기화 하고 계산 필요 ->
        InitializedRate();

        for (int i = 0; i < /*pickCount*/pickedCardNumb.Length; i++)
        {
            var adapt = card[/*pickedSlotNumb*/pickedCardNumb[i] - 1];

            switch (adapt.Atype)
            {
                case CardSO.Murtiple.AttackType.FightPower:
                    GameManager.Instance.M_AttackDamage += adapt.increase / 100;
                    return;

                case CardSO.Murtiple.AttackType.FightSpeed:
                    GameManager.Instance.AttackSpeed /= (adapt.increase / 100) + 1;
                    return;

                case CardSO.Murtiple.AttackType.MoveSpeed:
                    GameManager.Instance.PlayerSpeed += adapt.increase / 100;
                    return;

                case CardSO.Murtiple.AttackType.MaxBullet:
                    GameManager.Instance.MaxBullet += adapt.increase;
                    return;
            }
        }

        //사용 카드 제거 ->
        RemoveUsedCard();
    }

    void InitializedRate()
    {
        GameManager.Instance.M_AttackDamage = 5;
        GameManager.Instance.AttackSpeed = 1;
        GameManager.Instance.PlayerSpeed = 5;
        GameManager.Instance.MaxBullet = 7;
    }

    void RemoveUsedCard()
    {
        pickCount = 0;
        for (int i = 0; i < pickCount - 1; i++)
        {
            card[pickedCardNumb[i] - 1] = null;
            slot[pickedCardNumb[i]].gameObject.SetActive(false);
            slot[pickedCardNumb[i]].transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    void MergeCard()
    {
        InitializedRate();
    }
}
