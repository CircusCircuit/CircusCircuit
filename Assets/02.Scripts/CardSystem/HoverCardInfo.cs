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
        //getChild�� �ҰŸ� 1�� �ϰ� �� �Լ� �κ����� ������ �־�� �� ��
    }

    public void OnHoverExit()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
}
