using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Card;
using System.IO;

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

    const int MAXSLOT = 12;

    const int MAGICIAN = 1;
    const int JUGGLER = 2;
    const int ACROBAT = 3;

    int getCardIdx;

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
                MagicTxt[StageCounter - 1].text = StageCounter.ToString();
                return;

            case JUGGLER:
                JugTxt[StageCounter - 1].text = StageCounter.ToString();
                return;

            case ACROBAT:
                AcroTxt[StageCounter - 1].text = StageCounter.ToString();
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
