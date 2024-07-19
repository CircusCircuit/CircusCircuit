using Card;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverManager : MonoBehaviour
{
    public static HoverManager Instance = null;
    GameObject HoverUI;
    Text CardNameTxt;
    Text CardSummTxt;

    const int MAGICIAN = 0;
    const int JUGGLER = 1;
    const int ACROBAT = 2;

    Sprite preCardImg;

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        HoverUI = GameObject.Find("HoverCardCanvas").transform.GetChild(0).gameObject;
        CardNameTxt = HoverUI.transform.GetChild(0).GetComponent<Text>();
        CardSummTxt = HoverUI.transform.GetChild(1).GetComponent<Text>();
    }

    public void OnHoverEnter()
    {
        GameObject hoverObj = this.gameObject;
        Image hoverObjImg = hoverObj.GetComponent<Image>();

        if (/*this.gameObject*/hoverObj.layer == 10)
        {
            int stageCounter = GameManager.Instance.StageCounter;
            preCardImg = hoverObjImg.sprite;

            //hoverObj.GetComponent<Animator>().SetTrigger("doRotate");
            hoverObj.GetComponent<Button>().enabled = true;


            if (/*this.gameObject.GetComponent<Image>()*/hoverObjImg.sprite/*.name == "card1"*/ == CardSelectManager.Instance._CardTypeImg[MAGICIAN])
            {
                /*this.gameObject.GetComponent<Image>()*/
                hoverObjImg.sprite = CardSelectManager.Instance._MagicianLine[stageCounter - 1];
                return;
            }
            else if (/*this.gameObject.GetComponent<Image>()*/hoverObjImg.sprite/*.name == "card2"*/ == CardSelectManager.Instance._CardTypeImg[JUGGLER])
            {
                /*this.gameObject.GetComponent<Image>()*/
                hoverObjImg.sprite = CardSelectManager.Instance._JugglerLine[stageCounter - 1];
                return;
            }
            else
            {
                /*this.gameObject.GetComponent<Image>()*/
                hoverObjImg.sprite = CardSelectManager.Instance._AcrobatLine[stageCounter - 1];
                return;
            }
        }
        if (/*this.gameObject.GetComponent<Image>()*/hoverObjImg.sprite != null)
        {
            HoverUI.SetActive(true);

            if (/*this.gameObject*/hoverObj.tag == "MagicCard")
            {
                CardNameTxt.text = HoverCardInfo.Instance.magicCardInfo[int.Parse(hoverObj.name)]._NameInfo;
                CardSummTxt.text = HoverCardInfo.Instance.magicCardInfo[int.Parse(hoverObj.name)]._SummInfo;
            }
            else if (/*this.gameObject*/hoverObj.tag == "JuggCard")
            {
                CardNameTxt.text = HoverCardInfo.Instance.juggCardInfo[int.Parse(hoverObj.name)]._NameInfo;
                CardSummTxt.text = HoverCardInfo.Instance.juggCardInfo[int.Parse(hoverObj.name)]._SummInfo;
            }
            else if (/*this.gameObject*/hoverObj.tag == "AcroCard")
            {
                CardNameTxt.text = HoverCardInfo.Instance.acroCardInfo[int.Parse(hoverObj.name)]._NameInfo;
                CardSummTxt.text = HoverCardInfo.Instance.acroCardInfo[int.Parse(hoverObj.name)]._SummInfo;
            }
            else Debug.LogError("Can't get Hovering GameObject!");
        }
    }

    public void OnHoverExit()
    {
        if (this.gameObject.layer == 10)
        {
            GameObject hoverObj = this.gameObject;
            Image hoverObjImg = hoverObj.GetComponent<Image>();

            hoverObj.GetComponent<Button>().enabled = false;


            if (preCardImg == CardSelectManager.Instance._CardTypeImg[MAGICIAN])
            {
                hoverObjImg.sprite = CardSelectManager.Instance._CardTypeImg[MAGICIAN];
                return;
            }
            else if (preCardImg == CardSelectManager.Instance._CardTypeImg[JUGGLER])
            {
                hoverObjImg.sprite = CardSelectManager.Instance._CardTypeImg[JUGGLER];
                return;
            }
            else
            {
                hoverObjImg.sprite = CardSelectManager.Instance._CardTypeImg[ACROBAT];
                return;
            }
        }
        HoverUI.SetActive(false);
        CardNameTxt.text = "";
        CardSummTxt.text = "";
    }
}
