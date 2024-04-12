using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class SlotMachineController : MonoBehaviour
{
    [SerializeField] Animator curtian;
    [SerializeField] GameObject[] slotObject;

    bool isRotated = false;

    int[] rotateCount = new int[3];

    int saveNumb;
    int[] number = new int[3];

    IEnumerator RotateSlot()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        for (int i = 2;  i >= 0; i--)
        {
            yield return StartCoroutine(Rotation(i));
        }

        checkNumber();
    }

    IEnumerator Rotation(int idx)
    {
        int rand = Random.Range(0, 11);

        for (int i = 0; i < (5 + rand) * 2; i++)
        {
            slotObject[idx].transform.localPosition -= new Vector3(0, 420f, 0);
            if (slotObject[idx].transform.localPosition.y < 420f)
            {
                slotObject[idx].transform.localPosition += new Vector3(0, 4200f, 0);
            }
            yield return new WaitForSecondsRealtime(0.05f);
        }

        rotateCount[idx] = (5 + rand) * 2;
    }

    void checkNumber()
    {
        print("checking");

        for (int i = 2; i >= 0; i--)
        {
            Calculation(i);

            number[i] = saveNumb + 1;
            if (saveNumb + 1 == 10)
            {
                number[i] = 0;
            }
        }

        string numb = string.Join(", ", number);

        IncreaseFeather(numb);
    }

    void Calculation(int idx)
    {
        switch (rotateCount[idx--] % 10)
        {
            case 0:
                saveNumb = 9;
                return;
            case 1:
                saveNumb = 8;
                return;
            case 2:
                saveNumb = 7;
                return;
            case 3:
                saveNumb = 6;
                return;
            case 4:
                saveNumb = 5;
                return;
            case 5:
                saveNumb = 4;
                return;
            case 6:
                saveNumb = 3;
                return;
            case 7:
                saveNumb = 2;
                return;
            case 8:
                saveNumb = 1;
                return;
            case 9:
                saveNumb = 0;
                return;
        }
    }

    void IncreaseFeather(string number)
    {
        int rate = int.Parse(number);
        print(rate);

        if (rate < 300)
        {
            int random = Random.Range(0, 10);
            GameManager.Instance.FreeFeather += random;

            if (rate == 000 || rate == 111 || rate == 222)
            {
                GameManager.Instance.FreeFeather += 10;
            }
        }
        else if (rate >= 300 && rate < 600)
        {
            int random = Random.Range(10, 20);
            GameManager.Instance.FreeFeather += random;

            if (rate == 333 || rate == 444 || rate == 555)
            {
                GameManager.Instance.FreeFeather += 10;
            }
        }
        else if (rate >= 600 && rate < 900)
        {
            int random = Random.Range(20, 30);
            GameManager.Instance.FreeFeather += random;

            if (rate == 666 || rate == 777 || rate == 888)
            {
                GameManager.Instance.FreeFeather += 10;
            }
        }
        else if (rate >= 900 && rate < 1000)
        {
            int random = Random.Range(30, 40);
            GameManager.Instance.FreeFeather += random;

            if (rate == 999)
            {
                GameManager.Instance.FreeFeather += 10;
            }
        }
        else Debug.Log("IncreaseFeather()_rate_Number_Error");

        StartCoroutine(GoToStage());
    }

    public void SlotClick()
    {
        if (isRotated) { return; }
        StartCoroutine(RotateSlot());
        isRotated = true;
    }

    IEnumerator GoToStage()
    {
        curtian.SetBool("close", true);

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(GameManager.Instance.curStageIndex + 1);
    }
}
