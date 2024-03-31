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

    //5개 이상 선택 시, 첫번째 선택 카드 삭제용
    Queue<int> _pickedQueue = new Queue<int>();
    int[] pickedCardNumb/* = new int[5]*/;
    int pickCount = 0;
    CardSO.Murtiple[] pickedCards;

    //같은 카드 종류 카운트, [0] == 공격력 | [1] == 공속 | [2] == 이속 | [3] == 최대총알
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

        //공격력 1번카드 2개 할당
        card[0] = cardDropSO.cards[0].card.mul[0];
        card[1] = cardDropSO.cards[0].card.mul[0];

        //공속 1번카드
        card[2] = cardDropSO.cards[1].card.mul[0];
        //이속 1번카드
        card[3] = cardDropSO.cards[2].card.mul[0];
        //총알 1번카드
        card[4] = cardDropSO.cards[3].card.mul[0];
        //공격력 2번카드
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

    //5개 선택 제어 
    public void ChooseCard()
    {
        slotName = EventSystem.current.currentSelectedGameObject;

        //5개 이상 선택시 -> 첫번째거 빼고 마지막거 선택
        if (_pickedQueue.Count > 4)
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

    void SavePickedCard()
    {
        pickedCardNumb = new int[_pickedQueue.Count];
        pickedCards = new CardSO.Murtiple[_pickedQueue.Count];

        for (int i = 0; i <= slotCount; i++)
        {
            //프레임이 활성화된 슬롯 번호 저장
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
        //초기화 하고 계산 필요 ->
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
                print("공격력 증가 계수 들어가는지 확인 " + GameManager.Instance.coeffFightPower);
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

        //GameManager.Instance.M_AttackDamage = 5;
        //GameManager.Instance.AttackSpeed = 1;
        //GameManager.Instance.PlayerSpeed = 5;
        //GameManager.Instance.MaxBullet = 7;
    }

    void RemoveUsedCard()
    {
        for (int i = 0; i < _pickedQueue.Count; i++)
        {
            //배열 비우기
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
        //똑같이 그대로 다 계산
        CardAdaptionCalc();

        //선택한 카드 내 같은 유형 있는지 확인
        //유형 확인 후 -> 해당 공격 유형에 + 1.5배 적용

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
