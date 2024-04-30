using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardAdaptManager : MonoBehaviour
{
    public class SavePickedCard
    {
        public int pickedSlotNumb;
        public List<CardSO.Murtiple> pickedSlotCard = new List<CardSO.Murtiple>();
        //public CardSO.Murtiple pickedSlotCard;
    }
    protected List<SavePickedCard> savePickedCards = new List<SavePickedCard>();

    public class MergeInfoList
    {
        private CardSO.Murtiple[] mergeInfos = new CardSO.Murtiple[5];
        public CardSO.Murtiple this[int idx]
        {
            get { return mergeInfos[idx]; }
            set { mergeInfos[idx] = value; }
        }
    }
    protected List<MergeInfoList> mergeInfoList = new List<MergeInfoList>();

    //���� ��� �ʱ�ȭ
    protected void InitializedRate()
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

    protected void CardAdaptionCalc()
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

    protected void CoeffType(CardSO.Murtiple adapt)
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

    protected void CoeffMerge()
    {
        //������ �� ���� ����Ʈ���� ī�� ������ ��������
        CardSO.Murtiple[] getSaveCards = new CardSO.Murtiple[5];
        int idx = 0;
        foreach (var _cards in savePickedCards)
        {
            if (savePickedCards[idx].pickedSlotCard.Count != 5)
            {
                getSaveCards[idx] = savePickedCards[idx].pickedSlotCard[0];
            }
            ++idx;
        }

        //������ ī�� �������� ���� ī�常 �̱�
        var findSameCards = getSaveCards.GroupBy(x => x)
            .Where(g => g.Count() > 1)
            .Select(y => y.Key)
            .ToList();

        print("findSameCards" + findSameCards[0]);

        //���
        for (int i = 0; i < findSameCards.Count; i++)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) return;

            //���⼭ null���� ��
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

    protected void MergeCard()
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
}
