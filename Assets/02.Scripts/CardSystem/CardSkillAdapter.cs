using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSkillAdapter : MonoBehaviour
{
    const int MAGICIAN = 1;
    const int JUGGLER = 2;
    const int ACROBAT = 3;

    private void OnEnable()
    {
        switch (GameManager.Instance.SelectedCardType)
        {
            case MAGICIAN:
                SaveCardData.Instance.MagicianCard.Add(GameManager.Instance.StageCounter);
                return;

            case JUGGLER:
                SaveCardData.Instance.JugglerCard.Add(GameManager.Instance.StageCounter);
                return;

            case ACROBAT:
                SaveCardData.Instance.AcrobatCard.Add(GameManager.Instance.StageCounter);
                return;
        }
    }
}
