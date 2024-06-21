using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCardData : MonoBehaviour
{
    public static SaveCardData Instance = null;

    [SerializeField]
    List<int> magicianCard;
    public List<int> MagicianCard
    {
        get { return magicianCard; }
        set { magicianCard = value; }
    }

    [SerializeField]
    List<int> jugglerCard;
    public List<int> JugglerCard
    {
        get { return jugglerCard; }
        set { jugglerCard = value; }
    }

    [SerializeField]
    List<int> acrobatCard;
    public List<int> AcrobatCard
    {
        get { return acrobatCard; }
        set { acrobatCard = value; }
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(this.gameObject);
            }
        }


        magicianCard = new List<int>();
        jugglerCard = new List<int>();
        acrobatCard = new List<int>();
    }
}
