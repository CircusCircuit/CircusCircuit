using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenceController : MonoBehaviour
{
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Cursor.visible = true;
        }
    }

    public void TutorialButton()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void StartButton()
    {
        SceneManager.LoadScene(2);
    }

    public void RestartButton()
    {
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene(0);
    }
}
