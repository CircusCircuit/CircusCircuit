using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    public class MergeCardList
    {
        private CardSO.Murtiple[] mergedInfo = new CardSO.Murtiple[5];
        public CardSO.Murtiple this[int idx]
        {
            get { return mergedInfo[idx]; }
            set { mergedInfo[idx] = value; }
        }
        [SerializeField] Sprite mergeCardImg;
    }

    public class NormalCardList
    {
        public CardSO.Murtiple normalInfo;
    }

    public class OneOfTwoCardList
    {
        public bool isBurfCard = false;
    }

    public class TotalCardManager : MonoBehaviour
    {
        public static List<MergeCardList> mergeCardList = new List<MergeCardList>();
        public static List<NormalCardList> normalCardList = new List<NormalCardList>();
        public static List<OneOfTwoCardList> oneOfTwoCardList = new List<OneOfTwoCardList>();


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("<color=red>테스트용 노멀카드 추가</color>");
                AddNormalCardForTest();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("<color=orange>테스트용 머지카드 추가</color>");
                AddMergeCardForTest();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Debug.Log("<color=yellow>테스트용 택일카드 추가</color>");
                AddOneOfTwoCardForTest();
            }
        }

        [SerializeField] CardDropSO cardDropSO;
        void AddNormalCardForTest()
        {
            NormalCardList info = new NormalCardList();

            info.normalInfo = cardDropSO.cards[0].card.mul[0];
            normalCardList.Add(info);
            info.normalInfo = cardDropSO.cards[1].card.mul[0];
            normalCardList.Add(info);
        }
        void AddMergeCardForTest()
        {
            MergeCardList info = new MergeCardList();
            info[0] = cardDropSO.cards[0].card.mul[0];
            info[1] = cardDropSO.cards[0].card.mul[0];
            info[2] = cardDropSO.cards[1].card.mul[0];
            info[3] = cardDropSO.cards[1].card.mul[0];
            info[4] = cardDropSO.cards[2].card.mul[0];

            mergeCardList.Add(info);
        }
        void AddOneOfTwoCardForTest()
        {

        }
    }
}

