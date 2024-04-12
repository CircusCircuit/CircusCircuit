using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Inventory : CardAdaptManager
{
    CardSO.Murtiple getCard;
    public GameObject[] slot;
    CardController cardController;

    public class SlotCardInfo
    {
        public GameObject slotObj;
        public List<CardSO.Murtiple> cardInfo = new List<CardSO.Murtiple>();
        //public CardSO.Murtiple cardInfo;
        //public CardSO.Murtiple[] mergeCardInfo = new CardSO.Murtiple[5];
        public bool isPicked = false;
        public bool isMerged = false;
    }
    List<SlotCardInfo> slotInfo = new List<SlotCardInfo>();

    [SerializeField] Sprite mergeCardImg;

    private void Awake()
    {
        TestForMurtiCard();
    }

    [SerializeField] CardDropSO cardDropSO;
    void TestForMurtiCard()
    {

        SlotCardInfo info1 = new SlotCardInfo();
        info1.cardInfo.Add(cardDropSO.cards[0].card.mul[0]);
        SlotCardInfo info2 = new SlotCardInfo();
        info2.cardInfo.Add(cardDropSO.cards[0].card.mul[0]);
        SlotCardInfo info3 = new SlotCardInfo();
        info3.cardInfo.Add(cardDropSO.cards[1].card.mul[0]);
        SlotCardInfo info4 = new SlotCardInfo();
        info4.cardInfo.Add(cardDropSO.cards[2].card.mul[0]);
        SlotCardInfo info5 = new SlotCardInfo();
        info5.cardInfo.Add(cardDropSO.cards[3].card.mul[0]);
        SlotCardInfo info6 = new SlotCardInfo();
        info6.cardInfo.Add(cardDropSO.cards[0].card.mul[1]);

        slotInfo.Add(info1);
        slotInfo.Add(info2);
        slotInfo.Add(info3);
        slotInfo.Add(info4);
        slotInfo.Add(info5);
        slotInfo.Add(info6);


        for (int i = 0; i < slotInfo.Count/*slotCount*/; i++)
        {
            slotInfo[i].slotObj = slot[i].gameObject;
            slotInfo[i].slotObj.GetComponent<Image>().sprite = slotInfo[i].cardInfo[0].cardImage;

            slotInfo[i].slotObj.gameObject.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cardController = GameObject.Find("CardController").GetComponent<CardController>();

        ShowCard();
    }

    void ShowCard()
    {
        getCard = cardController.SendCard();

        if (getCard != null)
        {
            int numb = slotInfo.Count;

            SlotCardInfo info = new SlotCardInfo();
            info.cardInfo.Add(getCard);

            slotInfo.Add(info);

            slotInfo[numb].slotObj = slot[numb].gameObject;
            slotInfo[numb].slotObj.GetComponent<Image>().sprite = slotInfo[numb].cardInfo[0].cardImage;
            slotInfo[numb].slotObj.gameObject.SetActive(true);

            getCard = null;
        }
    }

    //Control choosing 5 cards
    public void ChooseCard()
    {
        GameObject pickedSlotObj = EventSystem.current.currentSelectedGameObject;

        //Choosing exceed 5
        if (savePickedCards.Count > 4)
        {
            if (savePickedCards == null) return;

            //First selected Card unFrame, Remove in savelist of Slotnumber & SlotCardInfo, isPicked == false

            int firstPickedCardNumber = savePickedCards[0].pickedSlotNumb;
            savePickedCards.RemoveAt(0);

            slotInfo[firstPickedCardNumber].slotObj.transform.GetChild(0).gameObject.SetActive(false);
            slotInfo[firstPickedCardNumber].isPicked = false;
        }

        //seleted Frame activation
        pickedSlotObj.transform.GetChild(0).gameObject.SetActive(true);

        //선택한 슬롯 번호, 카드 정보 리스트에 저장
        int pickedSlotNumber = int.Parse(pickedSlotObj.name);

        var pickedInfo = new SavePickedCard();
        if (slotInfo[pickedSlotNumber].cardInfo.Count == 5)
        {
            pickedInfo.pickedSlotNumb = pickedSlotNumber;

            int idx = 0;
            foreach (var picked in slotInfo[pickedSlotNumber].cardInfo)
            {
                pickedInfo.pickedSlotCard.Add(slotInfo[pickedSlotNumber].cardInfo[idx++]);
            }
        }
        else/* (slotInfo[pickedSlotNumber].cardInfo.Count == 1)*/
        {
            //선택 슬롯번호
            pickedInfo.pickedSlotNumb = pickedSlotNumber;
            //선택 슬롯카드정보
            pickedInfo.pickedSlotCard.Add(slotInfo[pickedSlotNumber].cardInfo[0]);
        }
        savePickedCards.Add(pickedInfo);

        //슬롯 선택 여부 반영
        slotInfo[pickedSlotNumber].isPicked = true;
    }

    public void UnUseCard()
    {
        GameObject pickedFrameObj = EventSystem.current.currentSelectedGameObject;
        int frameNumb = int.Parse(pickedFrameObj.name.Replace("Frame", ""));

        pickedFrameObj.SetActive(false);
        slotInfo[frameNumb].isPicked = false;

        int getIndex = 0;
        for (int i = 0; i < savePickedCards.Count; i++)
        {
            if (savePickedCards[i].pickedSlotNumb == frameNumb)
            {
                getIndex = i;
                break;
            }
        }

        savePickedCards.RemoveAt(getIndex);
    }

    public void InvenOkButton()
    {
        // 선택한 카드가 없으면
        if (savePickedCards == null)
        {
            this.gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            return;
        }

        if (savePickedCards.Count > 5)
        {
            Debug.Log("카드 선택 5개 초과 -> 카드 적용 오류" + "Seleted cards are exceed 5 -> Error adaption");
            return;
        }

        if (savePickedCards.Count < 5)
        {
            //5개 미만 카드 선택 -> 자동 장착
            //(소모형 카드의 경우, 장착 시 자동 삭제됨.(-> 재장착 불가)
            CardAdaptionCalc();
        }
        if (savePickedCards.Count == 5)
        {
            //5개 카드 선택 -> 합성 -> 강화된 카드 자동 장착
            //동일한 유형의 카드 합성 시 50%확률로 해당 유형의 1.5배 강화
            MergeCard();
        }

        //공격 증가 비율 계산
        GameManager.Instance.M_AttackDamage *= GameManager.Instance.coeffFightPower;
        GameManager.Instance.AttackSpeed /= GameManager.Instance.coeffFightSpeed;
        GameManager.Instance.PlayerSpeed *= GameManager.Instance.coeffMoveSpeed;
        GameManager.Instance.MaxBullet += GameManager.Instance.coeffMaxBullet;

         //사용 카드 제거 ->
        RemoveUsedCard();

        this.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        Cursor.visible = false;

        GameManager.Instance.curStageIndex = SceneManager.GetActiveScene().buildIndex;
    }


    void RemoveUsedCard()
    {
        int idx = 0;
        foreach (var _slot in slotInfo)
        {
            if (slotInfo[idx].isPicked && !slotInfo[idx].isMerged)
            {
                slotInfo[idx].isPicked = false;
                slotInfo[idx].slotObj.transform.GetChild(0).gameObject.SetActive(false);
                slotInfo[idx].slotObj.GetComponent<Image>().sprite = null;
                slotInfo[idx].slotObj.gameObject.SetActive(false);
            }
            ++idx;
        }

        savePickedCards.Clear();

        SlotReArrange();
    }

    void SlotReArrange()
    {
        //비활성화된 slotInfo의 내용물 비우기, foreach문 안에서는 list 삭제 안됨.
        for (int i = slotInfo.Count - 1; i >= 0; i--)
        {
            if (slotInfo[i].slotObj.activeSelf == false)
            {
                slotInfo.RemoveAt(i);
            }
        }

        //슬롯 전체 비활성화
        int idx = 0;
        foreach (var _slot in slot)
        {
            slot[idx].transform.GetChild(0).gameObject.SetActive(false);
            slot[idx].GetComponent<Image>().sprite = null;
            slot[idx].gameObject.SetActive(false);

            ++idx;
        }

        //머지카드 정보 저장
        if (mergeInfoList != null)
        {
            for (int i = 0; i < mergeInfoList.Count; i++)
            {
                //slotInfo[i].cardInfo = mergeInfos[i];
                SlotCardInfo info = new SlotCardInfo();
                info.cardInfo.Add(mergeInfoList[i][0]);
                info.cardInfo.Add(mergeInfoList[i][1]);
                info.cardInfo.Add(mergeInfoList[i][2]);
                info.cardInfo.Add(mergeInfoList[i][3]);
                info.cardInfo.Add(mergeInfoList[i][4]);

                slotInfo.Insert(i, info);

                slotInfo[i].isMerged = true;
                slotInfo[i].slotObj = slot[i].gameObject;
                slotInfo[i].slotObj.GetComponent<Image>().sprite = mergeCardImg;
                slotInfo[i].slotObj.gameObject.SetActive(true);
            }
        }

        //슬롯 첫칸부터 다시 채우기
        for (int i = mergeInfoList.Count; i < slotInfo.Count + mergeInfoList.Count - 1; i++)
        {
            slotInfo[i].slotObj = slot[i].gameObject;

            slotInfo[i].slotObj.GetComponent<Image>().sprite = slotInfo[i].cardInfo[0].cardImage;
            slotInfo[i].slotObj.gameObject.SetActive(true);
        }
    }
}
