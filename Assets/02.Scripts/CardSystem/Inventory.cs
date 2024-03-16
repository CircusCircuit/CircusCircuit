using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    CardSO.Murtiple getCard;
    CardSO.Murtiple[] card;
    public GameObject[] slot;
    public StageController stageController;

    int slotCount = 0;
    int maxSlot = 15;


    // Start is called before the first frame update
    void Start()
    {
        card = new CardSO.Murtiple[maxSlot];

        ShowCard();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShowCard()
    {
        getCard = stageController.SendCard();
        
        if (getCard != null)
        {
            card[slotCount] = getCard;

            slot[slotCount].gameObject.SetActive(true);
            slot[slotCount].GetComponent<Image>().sprite = card[slotCount].cardImage;
            slotCount++;

            getCard = null;
        }
    }
}
