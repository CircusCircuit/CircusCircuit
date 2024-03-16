using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public CardDropSO card;
    public GameObject slot;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShowCard()
    {
        if (card.TimesPick() != null)
        {
            
        }
    }
}
