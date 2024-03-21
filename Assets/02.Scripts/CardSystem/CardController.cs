using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    public CardDropSO cardDropSO;
    CardSO.Murtiple[] showCard;
    CardSO.Murtiple pickCard;

    GameObject invenObj;
    int cardSetCount = 3;
    [SerializeField] GameObject[] card;

    private void Awake()
    {
        showCard = new CardSO.Murtiple[cardSetCount];
    }

    // Start is called before the first frame update
    void Start()
    {
        invenObj = GameObject.FindGameObjectWithTag("Inventory").transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectCard()
    {
        invenObj.SetActive(true);

        for (int i = 0; i < cardSetCount; i++)
        {
            if (EventSystem.current.currentSelectedGameObject.name == "Card" + (i + 1))
            {
                //print("누른 버튼은 " + EventSystem.current.currentSelectedGameObject.name + " 이고 보내는건 " + showCard[i].cardName);
                pickCard = showCard[i];
            }
        }
    }

    public CardSO.Murtiple SendCard()
    {
        return pickCard;
    }

    public void RandomCard()
    {
        for (int i = 0; i < cardSetCount; i++)
        {
            card[i].GetComponent<Button>().enabled = true;

            showCard[i] = cardDropSO.TimesPick();
            card[i].GetComponent<Image>().sprite = showCard[i].cardImage;
        }
    }
}
