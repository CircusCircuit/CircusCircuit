using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static CardDropSO;

public class CardController : MonoBehaviour
{
    private AudioSource audioSource; // AudioSource ÄÄÆ÷³ÍÆ®
    [SerializeField] AudioClip cardSelectSFX;

    public CardDropSO cardDropSO;
    CardSO.Murtiple[] showCard;
    CardSO.Murtiple pickCard;

    GameObject invenObj;
    int cardSetCount = 3;
    [SerializeField] GameObject[] card;

    int randNum = 0;

    private void Awake()
    {
        showCard = new CardSO.Murtiple[cardSetCount];
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        invenObj = GameObject.FindGameObjectWithTag("Inventory").transform.GetChild(0).gameObject;
    }

    public void SelectCard()
    {
        audioSource.PlayOneShot(cardSelectSFX);
        invenObj.SetActive(true);

        for (int i = 0; i < cardSetCount; i++)
        {
            if (EventSystem.current.currentSelectedGameObject.name == "Card" + (i + 1))
            {
                pickCard = showCard[i];

                GameManager.Instance.FreeFeather -= FeatherDeburf(pickCard);
                StageController.Instance.CheckFetherCondition();
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

    int FeatherDeburf(CardSO.Murtiple growth)
    {
        switch (growth.number)
        {
            case 1:
                randNum = Random.Range(0, 5 + 1);
                return randNum;

            case 2:
                randNum = Random.Range(0, 10 + 1);
                return randNum;

            case 3:
                randNum = Random.Range(0, 20 + 1);
                return randNum;
        }

        return randNum;
    }
}
