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

public class Inventory : MonoBehaviour
{
    CardSO.Murtiple getCard;
    //CardSO.Murtiple[] card = new CardSO.Murtiple[15];
    public GameObject[] slot;
    CardController cardController;

    //테스트중 ->
    public class SlotCardInfo
    {
        public GameObject slotObj;
        public CardSO.Murtiple cardInfo;
        public CardSO.Murtiple[] mergeCardInfo = new CardSO.Murtiple[5];
        public bool isPicked = false;
        public bool isMerged = false;
    }
    List<SlotCardInfo> slotInfo = new List<SlotCardInfo>();

    //List<int> _pickedNumbQue = new List<int>();
    //List<CardSO.Murtiple> _pickedCardQue = new List<CardSO.Murtiple>();

    public class SavePickedCard
    {
        public int pickedSlotNumb;
        public CardSO.Murtiple pickedSlotCard;
    }
    List<SavePickedCard> savePickedCards = new List<SavePickedCard>();

    class MergeInfoList
    {
        //public CardSO.Murtiple mergeTypes;
        private CardSO.Murtiple[] mergeInfos = new CardSO.Murtiple[5];
        //private float[] _coeffs = new float[5];

        public CardSO.Murtiple this[int idx]
        {
            get { return mergeInfos[idx]; }
            set { mergeInfos[idx] = value; }
        }
    }
    List<MergeInfoList> mergeInfoList = new List<MergeInfoList>();

    [SerializeField] Sprite mergeCardImg;
    // <- 테스트중

    //int slotCount = 0; //활성 슬롯
    //int maxSlot = 15;
    //GameObject pickedSlotObj;

    ////5개 이상 선택 시, 첫번째 선택 카드 삭제용
    //Queue<int> _pickedQueue = new Queue<int>();
    //int[] pickedCardNumb/* = new int[5]*/;
    //int pickCount = 0;
    //CardSO.Murtiple[] pickedCards;

    private void Awake()
    {
        TestForMurtiCard();
    }

    [SerializeField] CardDropSO cardDropSO;
    void TestForMurtiCard()
    {
        SlotCardInfo info1 = new SlotCardInfo() { cardInfo = cardDropSO.cards[0].card.mul[0] };
        SlotCardInfo info2 = new SlotCardInfo() { cardInfo = cardDropSO.cards[0].card.mul[0] };
        SlotCardInfo info3 = new SlotCardInfo() { cardInfo = cardDropSO.cards[1].card.mul[0] };
        SlotCardInfo info4 = new SlotCardInfo() { cardInfo = cardDropSO.cards[2].card.mul[0] };
        SlotCardInfo info5 = new SlotCardInfo() { cardInfo = cardDropSO.cards[3].card.mul[0] };
        SlotCardInfo info6 = new SlotCardInfo() { cardInfo = cardDropSO.cards[0].card.mul[1] };

        slotInfo.Add(info1);
        slotInfo.Add(info2);
        slotInfo.Add(info3);
        slotInfo.Add(info4);
        slotInfo.Add(info5);
        slotInfo.Add(info6);

        //공격력 1번카드 2개 할당
        //slotInfo[0].cardInfo = cardDropSO.cards[0].card.mul[0];
        //slotInfo[1].cardInfo = cardDropSO.cards[0].card.mul[0];
        //card[0] = cardDropSO.cards[0].card.mul[0];
        //card[1] = cardDropSO.cards[0].card.mul[0];

        //공속 1번카드
        //slotInfo[2].cardInfo = cardDropSO.cards[1].card.mul[0];
        //card[2] = cardDropSO.cards[1].card.mul[0];

        //이속 1번카드
        //slotInfo[3].cardInfo = cardDropSO.cards[2].card.mul[0];
        //card[3] = cardDropSO.cards[2].card.mul[0];

        //총알 1번카드
        //slotInfo[4].cardInfo = cardDropSO.cards[3].card.mul[0];
        //card[4] = cardDropSO.cards[3].card.mul[0];

        //공격력 2번카드
        //slotInfo[5].cardInfo = cardDropSO.cards[0].card.mul[1];
        //card[5] = cardDropSO.cards[0].card.mul[1];

        for (int i = 0; i < slotInfo.Count/*slotCount*/; i++)
        {
            slotInfo[i].slotObj = slot[i].gameObject;
            slotInfo[i].slotObj.GetComponent<Image>().sprite = slotInfo[i].cardInfo.cardImage;

            slotInfo[i].slotObj.gameObject.SetActive(true);

            //slot[i].GetComponent<Image>().sprite = card[i].cardImage;
            //slot[i].gameObject.SetActive(true);
        }


        // -> 테스트 중

        //슬롯 오브젝트 활성화
        //slotInfo[0].slotObj = slot[0].gameObject;
        //slotInfo[0].slotObj.gameObject.SetActive(true);
        //슬롯에 카드정보 저장
        //slotInfo[0].cardInfo = cardDropSO.cards[0].card.mul[0];
        //슬롯에 카드 이미지 할당
        //slotInfo[0].slotObj.GetComponent<Image>().sprite = slotInfo[0].cardInfo.cardImage;
    }

    // Start is called before the first frame update
    void Start()
    {
        cardController = GameObject.Find("CardController").GetComponent<CardController>();
        //card = new CardSO.Murtiple[maxSlot];

        ShowCard();
    }

    void ShowCard()
    {
        getCard = cardController.SendCard();

        if (getCard != null)
        {
            int numb = slotInfo.Count;

            SlotCardInfo info = new SlotCardInfo() { cardInfo = getCard };
            //slotInfo[numb].cardInfo = getCard;
            //card[slotCount] = getCard;
            slotInfo.Add(info);

            slotInfo[numb].slotObj = slot[numb].gameObject;
            slotInfo[numb].slotObj.GetComponent<Image>().sprite = slotInfo[numb].cardInfo.cardImage;
            slotInfo[numb].slotObj.gameObject.SetActive(true);
            //slot[slotCount].gameObject.SetActive(true);
            //slot[slotCount].GetComponent<Image>().sprite = card[slotCount].cardImage;
            //slotCount++;

            getCard = null;
        }
    }

    //5개 선택 제어 
    public void ChooseCard()
    {
        //pickedSlotObj = EventSystem.current.currentSelectedGameObject;

        ////5개 이상 선택시 -> 첫번째거 빼고 마지막거 선택
        //if (_pickedQueue.Count > 4)
        //{
        //    //첫번째로 선택한 카드의 프레임 해제 및 첫번째거 빼기
        //    if (_pickedQueue == null) return;

        //    //프레임 해제
        //    slot[_pickedQueue.Peek()].transform.GetChild(0).gameObject.SetActive(false);

        //    //데이터 빼기
        //    _pickedQueue.Dequeue();
        //}

        ////선택한 슬롯의 프레임 활성화
        //pickedSlotObj.transform.GetChild(0).gameObject.SetActive(true);

        ////pickedSlotNumb[pickCount++] = int.Parse(slotName.name);
        //_pickedQueue.Enqueue(int.Parse(pickedSlotObj.name));




        //-> 테스트중

        GameObject pickedSlotObj = EventSystem.current.currentSelectedGameObject;

        //5개 이상 선택 시,
        if (savePickedCards/*_pickedCardQue*/.Count > 4)
        {
            if (savePickedCards/*_pickedCardQue*/ == null) return;

            //첫번째 선택 카드 프레임 해제, 리스트에서 슬롯 넘버랑 선택 카드 정보 제거, isPicked도 false

            int firstPickedCardNumber = /*_pickedNumbQue[0];*/savePickedCards[0].pickedSlotNumb;
            savePickedCards.RemoveAt(0);
            //_pickedNumbQue.RemoveAt(0);
            //_pickedCardQue.RemoveAt(0);

            slotInfo[firstPickedCardNumber].slotObj.transform.GetChild(0).gameObject.SetActive(false);
            slotInfo[firstPickedCardNumber].isPicked = false;
        }

        //선택한 슬롯의 프레임 활성화
        pickedSlotObj.transform.GetChild(0).gameObject.SetActive(true);

        //선택한 슬롯 번호, 카드 정보 리스트에 저장
        int pickedSlotNumber = int.Parse(pickedSlotObj.name);

        var pickedInfo = new SavePickedCard()
        {
            //선택 슬롯번호
            pickedSlotNumb = pickedSlotNumber,
            //선택 슬롯카드정보
            pickedSlotCard = slotInfo[pickedSlotNumber].cardInfo
        };
        savePickedCards.Add(pickedInfo/*pickedSlotNumber*/);

        //슬롯 선택 여부 반영
        slotInfo[pickedSlotNumber].isPicked = true;

        //선택된 카드 정보 저장
        //_pickedCardQue.Add(slotInfo[pickedSlotNumber].cardInfo);
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
        //_pickedNumbQue.Remove(frameNumb);
        //_pickedCardQue.Remove(frameNumb);
    }

    public void InvenOkButton()
    {
        //SavePickedCard();

        // 선택한 카드가 없으면
        if (savePickedCards == null)
        {
            this.gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            return;
        }

        if (savePickedCards.Count > 5)
        {
            Debug.Log("카드 선택 5개 초과, -> 카드 적용 오류");
            return;
        }

        if (savePickedCards.Count < 5)
        {
            //5개 미만 카드 선택 -> 자동 장착
            //(소모형 카드의 경우, 장착 시 자동 삭제됨.(-> 재장착 불가)
            //ex. 공격200% + 공격200% 장착하면 -> 공격 4배됨
            CardAdaptionCalc();
        }
        if (savePickedCards.Count == 5)
        {
            print("Merge!!");

            //5개 카드 선택 -> 합성 -> 강화된 카드 자동 장착
            //동일한 유형의 카드 합성 시 50%확률로 해당 유형의 1.5배 강화
            //ex. 공격200% + 공격200% + 공속300% + 최대총알1 + hp1증가 -> 공격"600%, 공속300%, 최대총알1, hp1증가
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
    }

    //void SavePickedCard()
    //{
    //    pickedCardNumb = new int[_pickedQueue.Count];
    //    pickedCards = new CardSO.Murtiple[_pickedQueue.Count];

    //    for (int i = 0; i <= slotCount; i++)
    //    {
    //        //프레임이 활성화된 슬롯 번호 저장
    //        if (slot[i].transform.GetChild(0).gameObject.activeSelf)
    //        {
    //            pickedCardNumb[pickCount] = int.Parse(slot[i].name);
    //            pickedCards[pickCount] = card[pickedCardNumb[pickCount] - 1];

    //            pickCount++;
    //        }
    //    }
    //}

    void CardAdaptionCalc()
    {
        //초기화 하고 계산 필요 ->
        InitializedRate();

        for (int i = 0; i < savePickedCards.Count; i++)
        {
            var adapt = savePickedCards[i].pickedSlotCard;

            CoeffType(adapt);
        }
    }

    void CoeffType(CardSO.Murtiple adapt)
    {
        switch (adapt.Atype)
        {
            case CardSO.Murtiple.AttackType.FightPower:
                //GameManager.Instance.M_AttackDamage *= (adapt.increase / 100) + 1;
                GameManager.Instance.coeffFightPower += adapt.increase / 100;
                return;

            case CardSO.Murtiple.AttackType.FightSpeed:
                //GameManager.Instance.AttackSpeed /= (adapt.increase / 100) + 1;
                GameManager.Instance.coeffFightSpeed += adapt.increase / 100;
                return;

            case CardSO.Murtiple.AttackType.MoveSpeed:
                //GameManager.Instance.PlayerSpeed *= (adapt.increase / 100) + 1;
                GameManager.Instance.coeffMoveSpeed += adapt.increase / 100;
                return;

            case CardSO.Murtiple.AttackType.MaxBullet:
                //GameManager.Instance.MaxBullet += adapt.increase;
                GameManager.Instance.coeffMaxBullet += adapt.increase;
                return;
        }
    }


    //증가 계수 초기화
    void InitializedRate()
    {
        GameManager.Instance.coeffFightPower = 1;
        GameManager.Instance.coeffFightSpeed = 1;
        GameManager.Instance.coeffMoveSpeed = 1;
        GameManager.Instance.coeffMaxBullet = 0;

        GameManager.Instance.M_AttackDamage = 5;
        GameManager.Instance.AttackSpeed = 1;
        GameManager.Instance.PlayerSpeed = 5;
        GameManager.Instance.MaxBullet = 7;

        GameManager.Instance.M_AttackDamage *= GameManager.Instance.coeffFightPower;
        GameManager.Instance.AttackSpeed /= GameManager.Instance.coeffFightSpeed;
        GameManager.Instance.PlayerSpeed *= GameManager.Instance.coeffMoveSpeed;
        GameManager.Instance.MaxBullet += GameManager.Instance.coeffMaxBullet;
    }

    void RemoveUsedCard()
    {
        //for (int i = 0; i < _pickedNumbQue/*_pickedQueue*/.Count; i++)
        //{
        //    //slotCount--;
        //    //_pickedQueue.Dequeue();
        //    //card[pickedCardNumb[i] - 1] = null;
        //    //slot[pickedCardNumb[i]].gameObject.SetActive(false);
        //    //slot[pickedCardNumb[i]].transform.GetChild(0).gameObject.SetActive(false);

        //    var numb = _pickedNumbQue[i];

        //    print("선택된 슬롯 번호 없애는 중 -> 현재 제거 중: " + _pickedNumbQue[i]);
        //    //print("slotInfo 안에 든거 " + slotInfo[_pickedNumbQue[i]].cardInfo.cardName);

        //    slotInfo[numb].slotObj.transform.GetChild(0).gameObject.SetActive(false);
        //    slotInfo[numb].slotObj.GetComponent<Image>().sprite = null;
        //    slotInfo[numb].slotObj.gameObject.SetActive(false);

        //    //slotInfo.RemoveAt(numb);
        //    //slotInfo[_pickedNumbQue[i]] = null;
        //}

        //for (int i = 0; i < slotInfo.Count; i++)
        //{
        //    if (slotInfo[i].isPicked)
        //    {
        //        slotInfo[i].isPicked = false;
        //        slotInfo[i].slotObj.transform.GetChild(0).gameObject.SetActive(false);
        //        slotInfo[i].slotObj.GetComponent<Image>().sprite = null;
        //        slotInfo[i].slotObj.gameObject.SetActive(false);

        //        //slotInfo.RemoveAt(i);
        //    }
        //}

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
        //_pickedNumbQue.Clear();
        //_pickedCardQue.Clear();

        //pickedCardNumb = null;
        //pickedCards = null;


        SlotReArrange();
    }

    void SlotReArrange()
    {
        // !!!!! 전체 비활성화 시킨다음에 -> 첫번째부터 채워야할 듯, 원래 있던 자리에서 안없어짐 지금은 !!!!!
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
            var mergeInfos = new MergeInfoList();
            for (int i = 0; i < mergeInfoList.Count; i++)
            {
                //slotInfo[i].cardInfo = mergeInfos[i];
                SlotCardInfo info = new SlotCardInfo();
                for (int j = 0; i < 5; j++)
                {
                    info.mergeCardInfo[i] = mergeInfoList[i][j];
                }
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

            //print("슬롯 정보에 저장된 슬롯이름은 " + slotInfo[i].slotObj.name);
            slotInfo[i].slotObj.GetComponent<Image>().sprite = slotInfo[i].cardInfo.cardImage;
            slotInfo[i].slotObj.gameObject.SetActive(true);
        }
    }

    void MergeCard()
    {
        //똑같이 그대로 다 계산
        CardAdaptionCalc();

        //선택한 카드 내 같은 유형 있는지 확인
        //유형 확인 후 -> 해당 공격 유형에 + 1.5배 적용

        CoeffMerge();

        var cardInfos = new MergeInfoList();
        for (int i = 0; i < savePickedCards.Count; i++)
        {
            cardInfos[i] = savePickedCards[i].pickedSlotCard;
        }
        mergeInfoList.Add(cardInfos);
    }

    void CoeffMerge()
    {
        //선택한 거 저장 리스트에서 카드 정보만 가져오기
        CardSO.Murtiple[] getSaveCards = new CardSO.Murtiple[5];
        int idx = 0;
        foreach (var _cards in savePickedCards)
        {
            getSaveCards[idx] = savePickedCards[idx].pickedSlotCard;
            ++idx;
        }
        //for (int i = 0; i < savePickedCards.Count; i++)
        //{
        //    getSaveCards[i] = savePickedCards[i].pickedSlotCard;
        //}

        //가져온 카드 정보에서 같은 카드만 뽑기
        var findSameCards = getSaveCards.GroupBy(x => x)
            .Where(g => g.Count() > 1)
            .Select(y => y.Key)
            .ToList();

        //계산
        for (int i = 0; i < findSameCards.Count; i++)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) return;

            switch (findSameCards[i].Atype)
            {
                case CardSO.Murtiple.AttackType.FightPower:
                    GameManager.Instance.coeffFightPower *= 1.5f;
                    return;

                case CardSO.Murtiple.AttackType.FightSpeed:
                    GameManager.Instance.coeffFightSpeed *= 1.5f;
                    return;

                case CardSO.Murtiple.AttackType.MoveSpeed:
                    GameManager.Instance.coeffMoveSpeed *= 1.5f;
                    return;

                case CardSO.Murtiple.AttackType.MaxBullet:
                    float curCoeff = GameManager.Instance.MaxBullet;
                    curCoeff *= 1.5f;
                    GameManager.Instance.coeffMaxBullet = (int)curCoeff;
                    return;
            }
        }
    }
}
