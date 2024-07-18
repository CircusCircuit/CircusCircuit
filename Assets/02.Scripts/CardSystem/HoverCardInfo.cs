using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public struct CardInfo
{
    [SerializeField] string NameInfo;
    public string _NameInfo
    {
        get { return NameInfo; }
    }
    [SerializeField] string SummInfo;
    public string _SummInfo
    {
        get { return SummInfo; }
    }
}

public class HoverCardInfo : MonoBehaviour
{
    public static HoverCardInfo Instance = null;

    [SerializeField] RectTransform CardInfoFrame;
    [SerializeField] Text CardNameTxt;
    [SerializeField] Text CardSummTxt;

    [SerializeField] List<CardInfo> MagicCardInfo;
    public List<CardInfo> magicCardInfo
    {
        get { return MagicCardInfo; }
    }
    [SerializeField] List<CardInfo> JuggCardInfo;
    public List<CardInfo> juggCardInfo
    {
        get { return JuggCardInfo; }
    }
    [SerializeField] List<CardInfo> AcroCardInfo;
    public List<CardInfo> acroCardInfo
    {
        get { return AcroCardInfo; }
    }

    //private Camera mainCamera;

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        CardInfoFrame.position = new Vector3(Input.mousePosition.x + 360, Input.mousePosition.y - 120, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mousePosition.x > 1200/*viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1*/)
        {
            CardInfoFrame.position = new Vector3(Input.mousePosition.x - 330, Input.mousePosition.y - 120, 0);
        }
        else
        {
            CardInfoFrame.position = new Vector3(Input.mousePosition.x + 360, Input.mousePosition.y - 120, 0);
        }
    }
}
