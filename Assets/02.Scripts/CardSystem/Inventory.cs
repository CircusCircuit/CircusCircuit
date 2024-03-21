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
            //5�� �̻� ���� -> ���� �ʱ�ȭ
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
                //5�� ī�� ���� -> �ռ� -> ��ȭ�� ī�� �ڵ� ����
                //������ ������ ī�� �ռ� �� 50%Ȯ���� �ش� ������ 1.5�� ��ȭ
                //ex. ����200% + ����200% + ����300% + �ִ��Ѿ�1 + hp1���� -> ����"600%, ����300%, �ִ��Ѿ�1, hp1����
                MergeCard();

                return;
            }

            //5�� �̸� ī�� ���� -> �ڵ� ����
            //(�Ҹ��� ī���� ���, ���� �� �ڵ� ������.(-> ������ �Ұ�)
            //ex. ����200% + ����200% �����ϸ� -> ���� 4���

        }
    }

    public void UnUseCard()
    {
        pickedSlotNumb[pickCount - 1] = 0;
        pickCount--;
        EventSystem.current.currentSelectedGameObject.SetActive(false);
        //�迭���� ����
    }

    public void InvenOkButton()
    {
        // ������ ī�尡 ������
        if (pickCount == 0)
        {
            this.gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            return;
        }

        GameManager.Instance.IsCardEnhance = true;
        //���� �������� ���� ���� ���������� �̵��ϸ� ���ݰ�� �ʱ�ȭ

        CardAdaptionCalc();

        this.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


        //����Ѱ� ��������!!!
    }

    void CardAdaptionCalc()
    {
        for (int i = 0; i < pickCount; i++)
        {
            var adapt = card[pickedSlotNumb[i] - 1];

            switch (adapt.Atype)
            {
                case CardSO.Murtiple.AttackType.FightPower:
                    //���� ������ ��� ����.
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
