using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card_Data/Attack_Type")]
public class CardDropSO : ScriptableObject
{
    [System.Serializable]
    public class Cards
    {
        public CardSO card;
        //È®·ü
        public int weight;
    }
    public List<Cards> cards = new List<Cards>();


    private CardSO PickCards()
    {
        int sum = 0;
        foreach(var card in cards)
        {
            sum += card.weight;
        }

        var random = Random.Range(0, sum);

        for (int i = 0; i < cards.Count; i++)
        {
            var card = cards[i];

            if (card.weight > random)
            {
                return cards[i].card;
            }
            else
            {
                random -= card.weight;
            }
        }
        
        return null;
    }


    public CardSO.Murtiple TimesPick()
    {
        //var pick = PickCards().mul[0];
        //int sum = 0;

        var random = Random.Range(0, 100);

        for (int i = 0; i < PickCards().mul.Count; i++)
        {
            var pick = PickCards().mul[i];

            if (pick.weight > random)
            {
                return PickCards().mul[i];
                //fixCard = PickCards().mul[i];
            }
            else
            {
                random -= pick.weight;
            }
        }

        return null;
        //fixCard = null;
    }
}
