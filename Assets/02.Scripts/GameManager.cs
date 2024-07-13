using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;


    [Header("�����ڸ��")]
    public bool areYouDev;

    [Header("[Stage_Info]")]
    // [ �������� ���� ]
    public int curStageIndex;
    public bool getNextWave = false;
    [SerializeField] bool isClear;
    [SerializeField] bool doCardSelect;
    int stageCounter = 0;

    //ī�� ���ð���
    int selectedCardType = 0;
    public int SelectedCardType
    {
        get { return selectedCardType; }
        set { selectedCardType = value; }
    }

    //[SerializeField] bool isClear1;
    //[SerializeField] bool isClear2;
    //[SerializeField] bool isClear3;
    //[SerializeField] bool isClear4;

    // [ �÷��̾� ���� ]
    float hp = 4;
    float attackPoewr = 100;
    int curBulletCount;
    float freeFeather = 50;

    int maxBullet = 7;
    float attackSpeed = 1;
    float playerSpeed = 5;

    // [ ���� ����, PlayerBullet Power ]
    float M_attackedDamage = 1;

    [Header("[Attack_Coeffient]")]
    //���
    public float coeffFightPower = 1;
    public float coeffFightSpeed = 1;
    public float coeffMoveSpeed = 1;
    public int coeffMaxBullet = 0;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
            {
                Destroy(this.gameObject);
            }
        }



        curBulletCount = maxBullet;

        curStageIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void Start()
    {
        if (areYouDev) hp = 1000;
        print("���� �÷��̾� hp " +  hp);
    }

    public float PlayerHp
    {
        get { return hp; }
        set { hp = value; }
    }

    public int CurBulletCount
    {
        get { return curBulletCount; }
        set { curBulletCount = value; }
    }

    public int MaxBullet
    {
        get { return maxBullet; }
        set { maxBullet = value; }
    }

    public float AttackPoewr
    {
        get { return attackPoewr; }
        set { attackPoewr = value; }
    }

    public float AttackSpeed
    {
        get { return attackSpeed; }
        set { attackSpeed = value; }
    }

    public float PlayerSpeed
    {
        get { return playerSpeed; }
        set { playerSpeed = value; }
    }

    public float M_AttackDamage
    {
        get { return M_attackedDamage; }
        set { M_attackedDamage = value; }
    }

    public float FreeFeather
    {
        get { return freeFeather; }
        set { freeFeather = value; }
    }



    //Ŭ���� Ȯ��
    public bool Clear
    {
        get { return isClear; }
        set { isClear = value; }
    }
    public bool DoCardSelect
    {
        get { return doCardSelect; }
        set { doCardSelect = value; }
    }
    public int StageCounter
    {
        get { return stageCounter; }
        set { stageCounter = value; }
    }
}
