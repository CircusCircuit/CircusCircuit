using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Card;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeckUIManager : MonoBehaviour
{
    [Header("UIBlock")]
    [SerializeField] GameObject MagicBlock;
    [SerializeField] GameObject AcroBlock;
    [SerializeField] GameObject JugBlock;
    [SerializeField] GameObject CloseCurtain;
    [SerializeField] Animator anim;

    TextMeshProUGUI[] MagicTxt;
    TextMeshProUGUI[] AcroTxt;
    TextMeshProUGUI[] JugTxt;

    Image[] MagicImage;
    Image[] JuggImage;
    Image[] AcroImage;

    const int MAXSLOT = 12;

    const int MAGICIAN = 1;
    const int JUGGLER = 2;
    const int ACROBAT = 3;

    int getCardIdx;

    [Header("Sprites")]
    [SerializeField] Sprite[] MagicSprites;
    [SerializeField] Sprite[] JuggSprites;
    [SerializeField] Sprite[] AcroSprites;


    private void Awake()
    {
        MagicTxt = new TextMeshProUGUI[MAXSLOT];
        AcroTxt = new TextMeshProUGUI[MAXSLOT];
        JugTxt = new TextMeshProUGUI[MAXSLOT];

        for (int i = 0; i < MAXSLOT; i++)
        {
            MagicTxt[i] = MagicBlock.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>();
            AcroTxt[i] = AcroBlock.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>();
            JugTxt[i] = JugBlock.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>();
        }



        MagicImage = new Image[MAXSLOT];
        JuggImage = new Image[MAXSLOT];
        AcroImage = new Image[MAXSLOT];

        for (int i = 0; i < MAXSLOT; i++)
        {
            MagicImage[i] = MagicBlock.transform.GetChild(i).GetComponent<Image>();
            JuggImage[i] = JugBlock.transform.GetChild(i).GetComponent<Image>();
            AcroImage[i] = AcroBlock.transform.GetChild(i).GetComponent<Image>();
        }
    }

    private void OnEnable()
    {
        int StageCounter = GameManager.Instance.StageCounter;

        //print(GameManager.Instance.SelectedCardType);
        switch (GameManager.Instance.SelectedCardType)
        {
            case 0:
                Debug.LogError("Can't get selectedCardType");
                return;

            case MAGICIAN:
                if (StageCounter - 1 > MagicImage.Length - 1)
                {
                    MagicTxt[StageCounter - 1].text = StageCounter.ToString();
                }
                else
                {
                    MagicImage[StageCounter - 1].sprite = MagicSprites[StageCounter - 1];
                }
                return;

            case JUGGLER:
                if (StageCounter - 1 > JuggImage.Length - 1)
                {
                    JugTxt[StageCounter - 1].text = StageCounter.ToString();
                }
                else
                {
                    JuggImage[StageCounter - 1].sprite = JuggSprites[StageCounter - 1];
                }
                return;

            case ACROBAT:
                if (StageCounter - 1 > AcroImage.Length - 1)
                {
                    AcroTxt[StageCounter - 1].text = StageCounter.ToString();
                }
                else
                {
                    AcroImage[StageCounter - 1].sprite = AcroSprites[StageCounter - 1];
                }
                return;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(CurtainCall());
        }
    }

    IEnumerator CurtainCall()
    {
        CloseCurtain.SetActive(true);

        anim.Rebind();
        anim.SetBool("play", true);
        yield return new WaitForSeconds(0.01f);

        var length = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);

        CloseCurtain.SetActive(false);
        this.gameObject.SetActive(false);

        GameManager.Instance.getNextWave = true;
        Cursor.visible = false;
    }
}
