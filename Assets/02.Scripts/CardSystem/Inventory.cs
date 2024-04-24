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
    private AudioSource audioSource; // AudioSource 컴포넌트
    [SerializeField] AudioClip cardSelectSFX;
    [SerializeField] AudioClip okBtnSFX;

    CardSO.Murtiple getCard;
    public GameObject[] slot;
    CardController cardController;

    public class SlotCardInfo
    {
        public GameObject slotObj;
        public CardSO.Murtiple cardInfo;
        //public bool isPicked = false;
        public bool isMerged = false;
        public CardSO.Murtiple[] mergeCardInfo = new CardSO.Murtiple[5];
    }
    List<SlotCardInfo> slotInfo = new List<SlotCardInfo>();

    [SerializeField] Sprite mergeCardImg;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        TestForMurtiCard();
    }

    [SerializeField] CardDropSO cardDropSO;
    void TestForMurtiCard()
    {

        SlotCardInfo info1 = new SlotCardInfo();
        info1.cardInfo = cardDropSO.cards[0].card.mul[0];
        SlotCardInfo info2 = new SlotCardInfo();
        info2.cardInfo = cardDropSO.cards[0].card.mul[0];
        SlotCardInfo info3 = new SlotCardInfo();
        info3.cardInfo = cardDropSO.cards[1].card.mul[0];
        SlotCardInfo info4 = new SlotCardInfo();
        info4.cardInfo = cardDropSO.cards[2].card.mul[0];
        SlotCardInfo info5 = new SlotCardInfo();
        info5.cardInfo = cardDropSO.cards[3].card.mul[0];
        SlotCardInfo info6 = new SlotCardInfo();
        info6.cardInfo = cardDropSO.cards[0].card.mul[1];

        slotInfo.Add(info1);
        slotInfo.Add(info2);
        slotInfo.Add(info3);
        slotInfo.Add(info4);
        slotInfo.Add(info5);
        slotInfo.Add(info6);


        for (int i = 0; i < slotInfo.Count/*slotCount*/; i++)
        {
            slotInfo[i].slotObj = slot[i].gameObject;
            slotInfo[i].slotObj.GetComponent<Image>().sprite = slotInfo[i].cardInfo.cardImage;

            slotInfo[i].slotObj.gameObject.SetActive(true);
        }
    }


    public void ShowCard(CardSO.Murtiple getCard)
    {
        if (getCard != null)
        {
            int numb = slotInfo.Count;

            SlotCardInfo info = new SlotCardInfo();
            info.cardInfo = getCard;

            slotInfo.Add(info);

            slotInfo[numb].slotObj = slot[numb].gameObject;
            slotInfo[numb].slotObj.GetComponent<Image>().sprite = slotInfo[numb].cardInfo.cardImage;
            slotInfo[numb].slotObj.gameObject.SetActive(true);

            getCard = null;
        }
    }

    //Control choosing 5 cards
    public void ChooseCard()
    {
        audioSource.PlayOneShot(cardSelectSFX);

        GameObject pickedSlotObj = EventSystem.current.currentSelectedGameObject;

        //Choosing exceed 5
        if (savePickedCards.Count > 4)
        {
            if (savePickedCards == null) return;

            //First selected Card unFrame, Remove in savelist of Slotnumber & SlotCardInfo, isPicked == false

            int firstPickedCardNumber = savePickedCards[0].pickedSlotNumb;
            savePickedCards.RemoveAt(0);

            slotInfo[firstPickedCardNumber].slotObj.transform.GetChild(0).gameObject.SetActive(false);
            //slotInfo[firstPickedCardNumber].isPicked = false;
        }

        //seleted Frame activation
        pickedSlotObj.transform.GetChild(0).gameObject.SetActive(true);

        //선택한 슬롯 번호, 카드 정보 리스트에 저장
        int pickedSlotNumber = int.Parse(pickedSlotObj.name);

        var savePickedCard = new SavePickedCard();
        //선택 슬롯번호
        savePickedCard.pickedSlotNumb = pickedSlotNumber;

        if (slotInfo[pickedSlotNumber].isMerged)
        {
            savePickedCard.pickedSlotCard.Add(slotInfo[pickedSlotNumber].mergeCardInfo[0]);
            savePickedCard.pickedSlotCard.Add(slotInfo[pickedSlotNumber].mergeCardInfo[1]);
            savePickedCard.pickedSlotCard.Add(slotInfo[pickedSlotNumber].mergeCardInfo[2]);
            savePickedCard.pickedSlotCard.Add(slotInfo[pickedSlotNumber].mergeCardInfo[3]);
            savePickedCard.pickedSlotCard.Add(slotInfo[pickedSlotNumber].mergeCardInfo[4]);
        }
        else
        {
            //선택 슬롯카드정보
            savePickedCard.pickedSlotCard.Add(slotInfo[pickedSlotNumber].cardInfo);
        }


        savePickedCards.Add(savePickedCard);

        //슬롯 선택 여부 반영
        //slotInfo[pickedSlotNumber].isPicked = true;
    }

    public void UnUseCard()
    {
        audioSource.PlayOneShot(cardSelectSFX);

        GameObject pickedFrameObj = EventSystem.current.currentSelectedGameObject;
        int frameNumb = int.Parse(pickedFrameObj.name.Replace("Frame", ""));

        pickedFrameObj.SetActive(false);
        //slotInfo[frameNumb].isPicked = false;

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
        audioSource.PlayOneShot(okBtnSFX);

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
        //int idx = 0;
        //foreach (var _slot in slotInfo)
        //{
        //    if (slotInfo[idx].isPicked && !slotInfo[idx].isMerged)
        //    {
        //        slotInfo[idx].isPicked = false;
        //        slotInfo[idx].slotObj.transform.GetChild(0).gameObject.SetActive(false);
        //        slotInfo[idx].slotObj.GetComponent<Image>().sprite = null;
        //        slotInfo[idx].slotObj.gameObject.SetActive(false);
        //    }
        //    ++idx;
        //}

        savePickedCards.Clear();

        SlotReArrange();
    }

    void SlotReArrange()
    {
        ////비활성화된 slotInfo의 내용물 비우기, foreach문 안에서는 list 삭제 안됨.
        //for (int i = slotInfo.Count - 1; i >= 0; i--)
        //{
        //    if (slotInfo[i].slotObj.activeSelf == false)
        //    {
        //        slotInfo.RemoveAt(i);
        //    }
        //}

        //슬롯 전체 비활성화
        int idx = 0;
        foreach (var _slot in slot)
        {
            slot[idx].transform.GetChild(0).gameObject.SetActive(false);
            slot[idx].GetComponent<Image>().sprite = null;
            slot[idx].gameObject.SetActive(false);

            ++idx;
        }

        //머지카드가 만들어졌을때 인벤토리에 재정렬할 정보 저장
        if (mergeInfoList != null)
        {
            for (int i = 0; i < mergeInfoList.Count; i++)
            {
                SlotCardInfo info = new SlotCardInfo();
                info.mergeCardInfo[0] = mergeInfoList[i][0];
                info.mergeCardInfo[1] = mergeInfoList[i][1];
                info.mergeCardInfo[2] = mergeInfoList[i][2];
                info.mergeCardInfo[3] = mergeInfoList[i][3];
                info.mergeCardInfo[4] = mergeInfoList[i][4];

                slotInfo.Insert(i, info);

                slotInfo[i].isMerged = true;
                slotInfo[i].slotObj = slot[i].gameObject;
                slotInfo[i].slotObj.GetComponent<Image>().sprite = mergeCardImg;
                slotInfo[i].slotObj.gameObject.SetActive(true);
            }
        }

        print(slotInfo.Count);
        //슬롯 첫칸부터 다시 채우기
        for (int i = mergeInfoList.Count; i < slotInfo.Count + mergeInfoList.Count - 1; i++)
        {
            print(slotInfo[i].cardInfo.cardName);
            slotInfo[i].slotObj = slot[i].gameObject;

            slotInfo[i].slotObj.GetComponent<Image>().sprite = slotInfo[i].cardInfo.cardImage;
            slotInfo[i].slotObj.gameObject.SetActive(true);
        }
    }
}
