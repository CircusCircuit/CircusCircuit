using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverNextWave : MonoBehaviour
{
    [SerializeField] GameObject CloseCurtain;
    [SerializeField] Animator anim;
    StageController stageController;

    // Start is called before the first frame update
    void Start()
    {
        stageController = GameObject.FindWithTag("GameController").GetComponent<StageController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.Clear)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }

        if (stageController.isLever)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                //audioSource.PlayOneShot(reverSound);

                //커튼콜 닫히는 애니메이션
                //팝업으로 공격형 카드 3장
                //audioSource.PlayOneShot(curtainCloseSound);
                StartCoroutine(CurtainCall());

                GameManager.Instance.Clear = false;
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }


    IEnumerator CurtainCall()
    {
        CloseCurtain.SetActive(true);
        print("CurtainCall");

        anim.SetBool("play", true);
        yield return new WaitForSeconds(0.01f);

        var length = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);

        GameManager.Instance.getNextWave = true;
        CloseCurtain.SetActive(false);

        //cardController.RandomCard();
        //Cursor.visible = true;
    }
}
