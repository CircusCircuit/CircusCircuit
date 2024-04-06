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
    public GameObject[] slot;
    CardController cardController;

    //�׽�Ʈ�� ->
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

    public class SavePickedCard
    {
        public int pickedSlotNumb;
        public List<CardSO.Murtiple> pickedSlotCard = new List<CardSO.Murtiple>();
        //public CardSO.Murtiple pickedSlotCard;
    }
    List<SavePickedCard> savePickedCards = new List<SavePickedCard>();

    class MergeInfoList
    {
        private CardSO.Murtiple[] mergeInfos = new CardSO.Murtiple[5];

        public CardSO.Murtiple this[int idx]
        {
            get { return mergeInfos[idx]; }
            set { mergeInfos[idx] = value; }
        }
    }
    List<MergeInfoList> mergeInfoList = new List<MergeInfoList>();

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

        //SlotCardInfo mInfo = new SlotCardInfo();
        //for (int i = 0; i < 5; i++)
        //{
        //    mInfo.cardInfo.Add(slotInfo[i].cardInfo[0]);
        //}

        //slotInfo.Add(mInfo);


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

    //5�� ���� ���� 
    public void ChooseCard()
    {
        GameObject pickedSlotObj = EventSystem.current.currentSelectedGameObject;

        //5�� �̻� ���� ��,
        if (savePickedCards.Count > 4)
        {
            if (savePickedCards == null) return;

            //ù��° ���� ī�� ������ ����, ����Ʈ���� ���� �ѹ��� ���� ī�� ���� ����, isPicked�� false

            int firstPickedCardNumber = savePickedCards[0].pickedSlotNumb;
            savePickedCards.RemoveAt(0);

            slotInfo[firstPickedCardNumber].slotObj.transform.GetChild(0).gameObject.SetActive(false);
            slotInfo[firstPickedCardNumber].isPicked = false;
        }

        //������ ������ ������ Ȱ��ȭ
        pickedSlotObj.transform.GetChild(0).gameObject.SetActive(true);

        //������ ���� ��ȣ, ī�� ���� ����Ʈ�� ����
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
            //���� ���Թ�ȣ
            pickedInfo.pickedSlotNumb = pickedSlotNumber;
            //���� ����ī������
            pickedInfo.pickedSlotCard.Add(slotInfo[pickedSlotNumber].cardInfo[0]);
        }
        savePickedCards.Add(pickedInfo);

        //���� ���� ���� �ݿ�
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
        //SavePickedCard();

        // ������ ī�尡 ������
        if (savePickedCards == null)
        {
            this.gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            return;
        }

        if (savePickedCards.Count > 5)
        {
            Debug.Log("ī�� ���� 5�� �ʰ�, -> ī�� ���� ����");
            return;
        }

        if (savePickedCards.Count < 5)
        {
            //5�� �̸� ī�� ���� -> �ڵ� ����
            //(�Ҹ��� ī���� ���, ���� �� �ڵ� ������.(-> ������ �Ұ�)
            //ex. ����200% + ����200% �����ϸ� -> ���� 4���
            CardAdaptionCalc();
        }
        if (savePickedCards.Count == 5)
        {
            //print("Merge!!");

            //5�� ī�� ���� -> �ռ� -> ��ȭ�� ī�� �ڵ� ����
            //������ ������ ī�� �ռ� �� 50%Ȯ���� �ش� ������ 1.5�� ��ȭ
            //ex. ����200% + ����200% + ����300% + �ִ��Ѿ�1 + hp1���� -> ����"600%, ����300%, �ִ��Ѿ�1, hp1����
            MergeCard();
        }

        //���� ���� ���� ���
        GameManager.Instance.M_AttackDamage *= GameManager.Instance.coeffFightPower;
        GameManager.Instance.AttackSpeed /= GameManager.Instance.coeffFightSpeed;
        GameManager.Instance.PlayerSpeed *= GameManager.Instance.coeffMoveSpeed;
        GameManager.Instance.MaxBullet += GameManager.Instance.coeffMaxBullet;

        //��� ī�� ���� ->
        RemoveUsedCard();

        this.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        Cursor.visible = false;
    }

    void CardAdaptionCalc()
    {
        //�ʱ�ȭ �ϰ� ��� �ʿ� ->
        InitializedRate();

        for (int i = 0; i < savePickedCards.Count; i++)
        {
            if (savePickedCards[i].pickedSlotCard.Count == 5)
            {
                int idx = 0;
                foreach (var card in savePickedCards[i].pickedSlotCard)
                {
                    var adapt = savePickedCards[i].pickedSlotCard[idx++];
                    CoeffType(adapt);
                    CoeffMerge();
                }
            }
            else
            {
                var adapt = savePickedCards[i].pickedSlotCard[0];
                CoeffType(adapt);
            }
        }
    }

    void CoeffType(CardSO.Murtiple adapt)
    {
        switch (adapt.Atype)
        {
            case CardSO.Murtiple.AttackType.FightPower:
                GameManager.Instance.coeffFightPower += adapt.increase / 100;
                return;

            case CardSO.Murtiple.AttackType.FightSpeed:
                GameManager.Instance.coeffFightSpeed += adapt.increase / 100;
                return;

            case CardSO.Murtiple.AttackType.MoveSpeed:
                GameManager.Instance.coeffMoveSpeed += adapt.increase / 100;
                return;

            case CardSO.Murtiple.AttackType.MaxBullet:
                GameManager.Instance.coeffMaxBullet += adapt.increase;
                return;
        }
    }


    //���� ��� �ʱ�ȭ
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
        // !!!!! ��ü ��Ȱ��ȭ ��Ų������ -> ù��°���� ä������ ��, ���� �ִ� �ڸ����� �Ⱦ����� ������ !!!!!
        //��Ȱ��ȭ�� slotInfo�� ���빰 ����, foreach�� �ȿ����� list ���� �ȵ�.
        for (int i = slotInfo.Count - 1; i >= 0; i--)
        {
            if (slotInfo[i].slotObj.activeSelf == false)
            {
                slotInfo.RemoveAt(i);
            }
        }

        //���� ��ü ��Ȱ��ȭ
        int idx = 0;
        foreach (var _slot in slot)
        {
            slot[idx].transform.GetChild(0).gameObject.SetActive(false);
            slot[idx].GetComponent<Image>().sprite = null;
            slot[idx].gameObject.SetActive(false);

            ++idx;
        }

        //����ī�� ���� ����
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

        //���� ùĭ���� �ٽ� ä���
        for (int i = mergeInfoList.Count; i < slotInfo.Count + mergeInfoList.Count - 1; i++)
        {
            slotInfo[i].slotObj = slot[i].gameObject;

            slotInfo[i].slotObj.GetComponent<Image>().sprite = slotInfo[i].cardInfo[0].cardImage;
            slotInfo[i].slotObj.gameObject.SetActive(true);
        }
    }

    void MergeCard()
    {
        //�Ȱ��� �״�� �� ���
        CardAdaptionCalc();

        //������ ī�� �� ���� ���� �ִ��� Ȯ��
        //���� Ȯ�� �� -> �ش� ���� ������ + 1.5�� ����

        CoeffMerge();

        var cardInfos = new MergeInfoList();
        for (int i = 0; i < savePickedCards.Count; i++)
        {
            cardInfos[i] = savePickedCards[i].pickedSlotCard[0];
        }
        mergeInfoList.Add(cardInfos);
    }

    void CoeffMerge()
    {
        //������ �� ���� ����Ʈ���� ī�� ������ ��������
        CardSO.Murtiple[] getSaveCards = new CardSO.Murtiple[5];
        int idx = 0;
        foreach (var _cards in savePickedCards)
        {
            getSaveCards[idx] = savePickedCards[idx].pickedSlotCard[0];
            ++idx;
        }

        //������ ī�� �������� ���� ī�常 �̱�
        var findSameCards = getSaveCards.GroupBy(x => x)
            .Where(g => g.Count() > 1)
            .Select(y => y.Key)
            .ToList();

        //���
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
