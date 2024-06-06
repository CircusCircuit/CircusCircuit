using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenceController : MonoBehaviour
{
    int buildIdx = 0;

    private void Start()
    {
        buildIdx = SceneManager.GetActiveScene().buildIndex;

        if (buildIdx == 0)
        {
            Cursor.visible = true;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && buildIdx == 0)
        {
            SceneManager.LoadScene(buildIdx + 1);
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
