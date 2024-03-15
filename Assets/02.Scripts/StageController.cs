using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StageController : MonoBehaviour
{
    [SerializeField] GameObject Lever;
    Animator anim;
    bool isLever;
    Button cardUI1, cardUI2, cardUI3;

    public CardDropSO card;

    private void Awake()
    {
        cardUI1 = Lever.transform.GetChild(1).GetChild(2).gameObject.GetComponent<Button>();
        cardUI2 = Lever.transform.GetChild(1).GetChild(3).gameObject.GetComponent<Button>();
        cardUI3 = Lever.transform.GetChild(1).GetChild(4).gameObject.GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.Clear1)
        {
            CreateLever();
        }

        Interaction();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Lever")
        {
            isLever = true;
        }
    }

    void Interaction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            //커튼콜 닫히는 애니메이션
            //팝업으로 공격형 카드 3장
            StartCoroutine(CurtainCall());
        }
    }

    void CreateLever()
    {
        Lever.gameObject.SetActive(true);
        anim = Lever.transform.GetChild(1).gameObject.GetComponent<Animator>();
    }

    IEnumerator CurtainCall()
    {
        print("CurtainCall");

        anim.SetBool("play", true);
        yield return new WaitForSeconds(0.01f);

        var length = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);

        cardUI1.enabled = true;
        cardUI2.enabled = true;
        cardUI3 .enabled = true;
    }

    public void SelectCard()
    {
        print(card.TimesPick().cardName);

        GameObject Inven = Lever.transform.GetChild(2).gameObject;
        Inven.SetActive(true);

        Inven.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = card.TimesPick().cardImage;
    }
}
