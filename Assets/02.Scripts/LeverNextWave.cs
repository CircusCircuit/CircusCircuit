using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverNextWave : MonoBehaviour
{
    [SerializeField] GameObject CloseCurtain;
    [SerializeField] Animator anim;
    //StageController stageController;
    [SerializeField] GameObject FKeyUI;
    bool isPlayer = false;
    BoxCollider2D outerColi;

    // Start is called before the first frame update
    void Start()
    {
        //stageController = GameObject.FindWithTag("GameController").GetComponent<StageController>();

        GameManager.Instance.Clear = false;
        transform.GetChild(0).gameObject.SetActive(false);
        //stageController.isLever = false;
        FKeyUI.SetActive(false);

        outerColi = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayer = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayer = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        print("LeverNextWave에서 Clear상태  " + GameManager.Instance.Clear);
        if (GameManager.Instance.Clear)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            outerColi.enabled = true;
        }

        if (isPlayer)
        {
            FKeyUI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                //audioSource.PlayOneShot(reverSound);

                //커튼콜 닫히는 애니메이션
                //팝업으로 공격형 카드 3장
                //audioSource.PlayOneShot(curtainCloseSound);
                StartCoroutine(CurtainCall());

                GameManager.Instance.Clear = false;
                transform.GetChild(0).gameObject.SetActive(false);
                outerColi.enabled = false;
            }
        }
        else
        {
            FKeyUI.SetActive(false);
        }
    }


    IEnumerator CurtainCall()
    {
        CloseCurtain.SetActive(true);

        anim.SetBool("play", true);
        yield return new WaitForSeconds(0.01f);

        var length = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);

        //GameManager.Instance.getNextWave = true;
        CloseCurtain.SetActive(false);

        GameManager.Instance.DoCardSelect = true;
        GameManager.Instance.StageCounter++;

        //cardController.RandomCard();
        //Cursor.visible = true;
    }
}
