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

    //�׽�Ʈ�� ->
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
    // <- �׽�Ʈ��

    //int slotCount = 0; //Ȱ�� ����
    //int maxSlot = 15;
    //GameObject pickedSlotObj;

    ////5�� �̻� ���� ��, ù��° ���� ī�� ������
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

        //���ݷ� 1��ī�� 2�� �Ҵ�
        //slotInfo[0].cardInfo = cardDropSO.cards[0].card.mul[0];
        //slotInfo[1].cardInfo = cardDropSO.cards[0].card.mul[0];
        //card[0] = cardDropSO.cards[0].card.mul[0];
        //card[1] = cardDropSO.cards[0].card.mul[0];

        //���� 1��ī��
        //slotInfo[2].cardInfo = cardDropSO.cards[1].card.mul[0];
        //card[2] = cardDropSO.cards[1].card.mul[0];

        //�̼� 1��ī��
        //slotInfo[3].cardInfo = cardDropSO.cards[2].card.mul[0];
        //card[3] = cardDropSO.cards[2].card.mul[0];

        //�Ѿ� 1��ī��
        //slotInfo[4].cardInfo = cardDropSO.cards[3].card.mul[0];
        //card[4] = cardDropSO.cards[3].card.mul[0];

        //���ݷ� 2��ī��
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


        // -> �׽�Ʈ ��

        //���� ������Ʈ Ȱ��ȭ
        //slotInfo[0].slotObj = slot[0].gameObject;
        //slotInfo[0].slotObj.gameObject.SetActive(true);
        //���Կ� ī������ ����
        //slotInfo[0].cardInfo = cardDropSO.cards[0].card.mul[0];
        //���Կ� ī�� �̹��� �Ҵ�
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

    //5�� ���� ���� 
    public void ChooseCard()
    {
        //pickedSlotObj = EventSystem.current.currentSelectedGameObject;

        ////5�� �̻� ���ý� -> ù��°�� ���� �������� ����
        //if (_pickedQueue.Count > 4)
        //{
        //    //ù��°�� ������ ī���� ������ ���� �� ù��°�� ����
        //    if (_pickedQueue == null) return;

        //    //������ ����
        //    slot[_pickedQueue.Peek()].transform.GetChild(0).gameObject.SetActive(false);

        //    //������ ����
        //    _pickedQueue.Dequeue();
        //}

        ////������ ������ ������ Ȱ��ȭ
        //pickedSlotObj.transform.GetChild(0).gameObject.SetActive(true);

        ////pickedSlotNumb[pickCount++] = int.Parse(slotName.name);
        //_pickedQueue.Enqueue(int.Parse(pickedSlotObj.name));




        //-> �׽�Ʈ��

        GameObject pickedSlotObj = EventSystem.current.currentSelectedGameObject;

        //5�� �̻� ���� ��,
        if (savePickedCards/*_pickedCardQue*/.Count > 4)
        {
            if (savePickedCards/*_pickedCardQue*/ == null) return;

            //ù��° ���� ī�� ������ ����, ����Ʈ���� ���� �ѹ��� ���� ī�� ���� ����, isPicked�� false

            int firstPickedCardNumber = /*_pickedNumbQue[0];*/savePickedCards[0].pickedSlotNumb;
            savePickedCards.RemoveAt(0);
            //_pickedNumbQue.RemoveAt(0);
            //_pickedCardQue.RemoveAt(0);

            slotInfo[firstPickedCardNumber].slotObj.transform.GetChild(0).gameObject.SetActive(false);
            slotInfo[firstPickedCardNumber].isPicked = false;
        }

        //������ ������ ������ Ȱ��ȭ
        pickedSlotObj.transform.GetChild(0).gameObject.SetActive(true);

        //������ ���� ��ȣ, ī�� ���� ����Ʈ�� ����
        int pickedSlotNumber = int.Parse(pickedSlotObj.name);

        var pickedInfo = new SavePickedCard()
        {
            //���� ���Թ�ȣ
            pickedSlotNumb = pickedSlotNumber,
            //���� ����ī������
            pickedSlotCard = slotInfo[pickedSlotNumber].cardInfo
        };
        savePickedCards.Add(pickedInfo/*pickedSlotNumber*/);

        //���� ���� ���� �ݿ�
        slotInfo[pickedSlotNumber].isPicked = true;

        //���õ� ī�� ���� ����
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
            print("Merge!!");

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

    //void SavePickedCard()
    //{
    //    pickedCardNumb = new int[_pickedQueue.Count];
    //    pickedCards = new CardSO.Murtiple[_pickedQueue.Count];

    //    for (int i = 0; i <= slotCount; i++)
    //    {
    //        //�������� Ȱ��ȭ�� ���� ��ȣ ����
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
        //�ʱ�ȭ �ϰ� ��� �ʿ� ->
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
        //for (int i = 0; i < _pickedNumbQue/*_pickedQueue*/.Count; i++)
        //{
        //    //slotCount--;
        //    //_pickedQueue.Dequeue();
        //    //card[pickedCardNumb[i] - 1] = null;
        //    //slot[pickedCardNumb[i]].gameObject.SetActive(false);
        //    //slot[pickedCardNumb[i]].transform.GetChild(0).gameObject.SetActive(false);

        //    var numb = _pickedNumbQue[i];

        //    print("���õ� ���� ��ȣ ���ִ� �� -> ���� ���� ��: " + _pickedNumbQue[i]);
        //    //print("slotInfo �ȿ� ��� " + slotInfo[_pickedNumbQue[i]].cardInfo.cardName);

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

        //���� ùĭ���� �ٽ� ä���
        for (int i = mergeInfoList.Count; i < slotInfo.Count + mergeInfoList.Count - 1; i++)
        {
            slotInfo[i].slotObj = slot[i].gameObject;

            //print("���� ������ ����� �����̸��� " + slotInfo[i].slotObj.name);
            slotInfo[i].slotObj.GetComponent<Image>().sprite = slotInfo[i].cardInfo.cardImage;
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
            cardInfos[i] = savePickedCards[i].pickedSlotCard;
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
            getSaveCards[idx] = savePickedCards[idx].pickedSlotCard;
            ++idx;
        }
        //for (int i = 0; i < savePickedCards.Count; i++)
        //{
        //    getSaveCards[i] = savePickedCards[i].pickedSlotCard;
        //}

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
