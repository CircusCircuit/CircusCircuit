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

    //5�� �̻� ���� ��, ù��° ���� ī�� ������
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

    //5�� ���� ���� 
    public void ChooseCard()
    {
        slotName = EventSystem.current.currentSelectedGameObject;

        //5�� �̻� ���ý� -> ù��°�� ���� �������� ����
        if (_pickedQueue.Count > 5)
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
            //5�� ī�� ���� -> �ռ� -> ��ȭ�� ī�� �ڵ� ����
            //������ ������ ī�� �ռ� �� 50%Ȯ���� �ش� ������ 1.5�� ��ȭ
            //ex. ����200% + ����200% + ����300% + �ִ��Ѿ�1 + hp1���� -> ����"600%, ����300%, �ִ��Ѿ�1, hp1����
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
            //�������� Ȱ��ȭ�� ���� ��ȣ ����
            if (slot[i].transform.GetChild(0).gameObject.activeSelf)
            {
                pickedCardNumb[i] = int.Parse(slot[i].name) - 1;
            }
        }
    }

    void CardAdaptionCalc()
    {
        //�ʱ�ȭ �ϰ� ��� �ʿ� ->
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

        //��� ī�� ���� ->
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
