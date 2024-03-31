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
    CardSO.Murtiple[] card = new CardSO.Murtiple[15];
    public GameObject[] slot;
    CardController cardController;

    int slotCount = 0;
    int maxSlot = 15;
    GameObject slotName;

    //5�� �̻� ���� ��, ù��° ���� ī�� ������
    Queue<int> _pickedQueue = new Queue<int>();
    int[] pickedCardNumb/* = new int[5]*/;
    int pickCount = 0;
    CardSO.Murtiple[] pickedCards;

    //���� ī�� ���� ī��Ʈ, [0] == ���ݷ� | [1] == ���� | [2] == �̼� | [3] == �ִ��Ѿ�
    //int[] cardTypeCount = new int[4];

    private void Awake()
    {
        TestForMurtiCard();
    }

    //[SerializeField] CardDropSO.Cards[] SOcardType;
    [SerializeField] CardDropSO cardDropSO;
    [SerializeField] CardSO cardSO;

    void TestForMurtiCard()
    {
        slotCount = 6;

        //���ݷ� 1��ī�� 2�� �Ҵ�
        card[0] = cardDropSO.cards[0].card.mul[0];
        card[1] = cardDropSO.cards[0].card.mul[0];

        //���� 1��ī��
        card[2] = cardDropSO.cards[1].card.mul[0];
        //�̼� 1��ī��
        card[3] = cardDropSO.cards[2].card.mul[0];
        //�Ѿ� 1��ī��
        card[4] = cardDropSO.cards[3].card.mul[0];
        //���ݷ� 2��ī��
        card[5] = cardDropSO.cards[0].card.mul[1];

        for (int i = 0; i < slotCount; i++)
        {
            slot[i].GetComponent<Image>().sprite = card[i].cardImage;
            slot[i].gameObject.SetActive(true);
        }
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
            card[slotCount] = getCard;

            slot[slotCount].gameObject.SetActive(true);
            slot[slotCount].GetComponent<Image>().sprite = card[slotCount].cardImage;
            slotCount++;

            getCard = null;
        }
    }

    //5�� ���� ���� 
    public void ChooseCard()
    {
        slotName = EventSystem.current.currentSelectedGameObject;

        //5�� �̻� ���ý� -> ù��°�� ���� �������� ����
        if (_pickedQueue.Count > 4)
        {
            //ù��°�� ������ ī���� ������ ���� �� ù��°�� ����
            if (_pickedQueue == null) return;

            //������ ����
            //slot[pickedSlotNumb[0]].transform.GetChild(0).gameObject.SetActive(false);
            slot[_pickedQueue.Peek()].transform.GetChild(0).gameObject.SetActive(false);

            //������ ����
            _pickedQueue.Dequeue();
            //for (int i = 0; i < 4; i++)
            //{
            //    pickedSlotNumb[i] = pickedSlotNumb[i + 1];
            //}
            //pickedSlotNumb[4] = 0;
            //pickCount = 3;
        }

        //������ ������ ������ Ȱ��ȭ
        slotName.transform.GetChild(0).gameObject.SetActive(true);

        //pickedSlotNumb[pickCount++] = int.Parse(slotName.name);
        _pickedQueue.Enqueue(int.Parse(slotName.name));
    }

    public void UnUseCard()
    {
        //�迭���� ����
        //pickedSlotNumb[pickCount - 1] = 0;
        //pickCount--;
        _pickedQueue.Dequeue();
        EventSystem.current.currentSelectedGameObject.SetActive(false);
    }

    public void InvenOkButton()
    {
        SavePickedCard();

        // ������ ī�尡 ������
        if (pickedCardNumb == null)
        {
            this.gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            return;
        }

        if (pickedCardNumb.Length > 5)
        {
            Debug.Log("ī�� ���� 5�� �ʰ�, -> ī�� ���� ����");
            return;
        }

        if (pickedCardNumb.Length < 5)
        {
            //5�� �̸� ī�� ���� -> �ڵ� ����
            //(�Ҹ��� ī���� ���, ���� �� �ڵ� ������.(-> ������ �Ұ�)
            //ex. ����200% + ����200% �����ϸ� -> ���� 4���
            CardAdaptionCalc();
        }
        if (pickedCardNumb.Length == 5)
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

    void SavePickedCard()
    {
        pickedCardNumb = new int[_pickedQueue.Count];
        pickedCards = new CardSO.Murtiple[_pickedQueue.Count];

        for (int i = 0; i <= slotCount; i++)
        {
            //�������� Ȱ��ȭ�� ���� ��ȣ ����
            if (slot[i].transform.GetChild(0).gameObject.activeSelf)
            {
                pickedCardNumb[pickCount] = int.Parse(slot[i].name);
                pickedCards[pickCount] = card[pickedCardNumb[pickCount] - 1];

                pickCount++;
            }
        }
    }

    void CardAdaptionCalc()
    {
        //�ʱ�ȭ �ϰ� ��� �ʿ� ->
        InitializedRate();

        for (int i = 0; i < /*pickCount*/pickedCardNumb.Length; i++)
        {
            //var adapt = card[/*pickedSlotNumb*/pickedCardNumb[i] - 1];
            var adapt = pickedCards[i];

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
                print("���ݷ� ���� ��� ������ Ȯ�� " + GameManager.Instance.coeffFightPower);
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

        //GameManager.Instance.M_AttackDamage = 5;
        //GameManager.Instance.AttackSpeed = 1;
        //GameManager.Instance.PlayerSpeed = 5;
        //GameManager.Instance.MaxBullet = 7;
    }

    void RemoveUsedCard()
    {
        for (int i = 0; i < _pickedQueue.Count; i++)
        {
            //�迭 ����
            slotCount--;
            _pickedQueue.Dequeue();
            card[pickedCardNumb[i] - 1] = null;
            slot[pickedCardNumb[i]].gameObject.SetActive(false);
            slot[pickedCardNumb[i]].transform.GetChild(0).gameObject.SetActive(false);
        }

        pickedCardNumb = null;
        //cardTypeCount = null;
    }

    void MergeCard()
    {
        //�Ȱ��� �״�� �� ���
        CardAdaptionCalc();

        //������ ī�� �� ���� ���� �ִ��� Ȯ��
        //���� Ȯ�� �� -> �ش� ���� ������ + 1.5�� ����

        var findSameCards = card.GroupBy(x => x)
            .Where(g => g.Count() > 1)
            .Select(y => y.Key)
            .ToList();

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
