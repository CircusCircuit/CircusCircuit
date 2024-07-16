using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverCardInfo : MonoBehaviour/*, IPointerEnterHandler*/
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    Debug.Log("Enter");
    //}

    public void OnHoverEnter()
    {
        Debug.Log("Hover GameObject Name: " + this.gameObject.name);
        this.transform.GetChild(0).gameObject.SetActive(true);
        //getChild로 할거면 1로 하고 이 함수 인벤마다 가지고 있어야 할 듯
    }

    public void OnHoverExit()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
}
