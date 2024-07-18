using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Card
{
    public class CardSelectManager : MonoBehaviour
    {
        public static CardSelectManager Instance = null;

        [SerializeField] GameObject SelectUI;
        [SerializeField] Image[] CardUI;
        [SerializeField] Sprite[] CardImg;
        public Sprite[] _CardTypeImg
        {
            get { return CardImg; }
        }
        [SerializeField] GameObject InvenUI;
        int PreCardIdx = 0;

        [Header("CardTypeSprites")]
        [SerializeField] List<Sprite> MagicianLine;
        public List<Sprite> _MagicianLine
        {
            get { return MagicianLine; }
        }
        [SerializeField] List<Sprite> JugglerLine;
        public List<Sprite> _JugglerLine
        {
            get { return JugglerLine; }
        }
        [SerializeField] List<Sprite> AcrobatLine;
        public List<Sprite> _AcrobatLine
        {
            get { return AcrobatLine; }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.DoCardSelect)
            {
                GameManager.Instance.DoCardSelect = false;
                SelectUI.SetActive(true);
                Cursor.visible = true;

                RandomCardDraw();
            }
        }

        void RandomCardDraw()
        {
            int ranNumb = Random.Range(0, 2);

            switch (PreCardIdx)
            {
                //At First Time
                case 0:
                    for (int i = 0; i < CardUI.Length; i++)
                    {
                        CardUI[i].sprite = CardImg[i];
                    }
                    return;

                //Select Magician
                case 1:
                    CardUI[0].sprite = CardImg[1];
                    CardUI[2].sprite = CardImg[2];

                    if (ranNumb == 0) { CardUI[1].sprite = CardImg[1]; }
                    else { CardUI[1].sprite = CardImg[2]; }

                    return;

                //Select Juggler
                case 2:
                    CardUI[0].sprite = CardImg[0];
                    CardUI[2].sprite = CardImg[2];

                    if (ranNumb == 0) { CardUI[1].sprite = CardImg[0]; }
                    else { CardUI[1].sprite = CardImg[2]; }

                    return;

                //Select Acrobat
                case 3:
                    CardUI[0].sprite = CardImg[0];
                    CardUI[2].sprite = CardImg[1];

                    if (ranNumb == 0) { CardUI[1].sprite = CardImg[0]; }
                    else { CardUI[1].sprite = CardImg[1]; }

                    return;

            }
        }

        public void CardButton()
        {
            string getCardImageNumber = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite.name.Substring(1);
            if (getCardImageNumber == "m")
            {
                PreCardIdx = 1;
            }
            else if (getCardImageNumber == "j")
            {
                PreCardIdx = 2;
            }
            else
            {
                PreCardIdx = 3;
            }
            //PreCardIdx = int.Parse(getCardImageNumber);

            GameManager.Instance.SelectedCardType = PreCardIdx;

            this.transform.GetChild(0).gameObject.SetActive(false);
            InvenUI.SetActive(true);
        }


        //public void OnHoverEnter()
        //{
        //    if (this.gameObject.layer == 10)
        //    {
        //        this.gameObject.GetComponent<Animator>().SetBool("", true);
        //        this.gameObject.GetComponent<Button>().enabled = true;

        //        if (this.gameObject.GetComponent<Image>().sprite.name == "card1")
        //        {
        //            this.gameObject.GetComponent<Image>().sprite = ReverseCard[0];
        //            return;
        //        }
        //        else if (this.gameObject.GetComponent<Image>().sprite.name == "card2")
        //        {
        //            this.gameObject.GetComponent<Image>().sprite = ReverseCard[1];
        //            return;
        //        }
        //        else
        //        {
        //            this.gameObject.GetComponent<Image>().sprite = ReverseCard[2];
        //            return;
        //        }
        //    }
        //}

        //public void OnHoverExit()
        //{
        //    if (this.gameObject.layer == 10)
        //    {
        //        this.gameObject.GetComponent<Button>().enabled = false;
        //        this.gameObject.GetComponent<Animator>().SetBool("", false);

        //        if (this.gameObject.GetComponent<Image>().sprite.name == "Rcard1")
        //        {
        //            this.gameObject.GetComponent<Image>().sprite = CardImg[0];
        //            return;
        //        }
        //        else if (this.gameObject.GetComponent<Image>().sprite.name == "Rcard2")
        //        {
        //            this.gameObject.GetComponent<Image>().sprite = CardImg[1];
        //            return;
        //        }
        //        else
        //        {
        //            this.gameObject.GetComponent<Image>().sprite = CardImg[2];
        //            return;
        //        }
        //    }
        //}
    }
}
