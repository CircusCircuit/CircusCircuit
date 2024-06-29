using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Card
{
    public class CardSelectManager : MonoBehaviour
    {
        [SerializeField] GameObject SelectUI;
        [SerializeField] Image[] CardUI;
        [SerializeField] Sprite[] CardImg;
        [SerializeField] GameObject InvenUI;
        int PreCardIdx = 0;

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
            string getCardImageNumber = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite.name.Replace("card", "");
            PreCardIdx = int.Parse(getCardImageNumber);

            GameManager.Instance.SelectedCardType = PreCardIdx;
            
            this.transform.GetChild(0).gameObject.SetActive(false);
            InvenUI.SetActive(true);
        }
    }
}
