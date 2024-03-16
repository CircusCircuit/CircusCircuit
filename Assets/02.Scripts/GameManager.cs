using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    [SerializeField]
    bool isClear1;

    public bool Clear1
    {
        get { return isClear1; }
    }


    // Update is called once per frame
    void Update()
    {
        DetectClear();
    }

    //Ŭ���� ���� �˻�
    void DetectClear()
    {

    }
}
