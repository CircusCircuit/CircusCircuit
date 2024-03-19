using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenceController : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void RestartButton()
    {
        Time.timeScale = 1;
    }

    public void InvenOkButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        this.gameObject.SetActive(false);
    }
}
