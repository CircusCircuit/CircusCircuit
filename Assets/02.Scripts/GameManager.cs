using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;


    [Header("개발자모드")]
    public bool areYouDev;

    [Header("[Stage_Info]")]
    // [ 스테이지 관련 ]
    public int curStageIndex;
    public bool getNextWave = false;
    [SerializeField] bool isClear;
    [SerializeField] bool isClear1;
    [SerializeField] bool isClear2;
    [SerializeField] bool isClear3;
    [SerializeField] bool isClear4;

    // [ 플레이어 관련 ]
    float hp = 4;
    float attackPoewr = 100;
    int curBulletCount;
    float freeFeather = 50;

    int maxBullet = 7;
    float attackSpeed = 1;
    float playerSpeed = 5;

    // [ 몬스터 관련 ]
    float M_attackedDamage = 5;

    [Header("[Attack_Coeffient]")]
    //계수
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



    //클리어 확인
    public bool Clear
    {
        get { return isClear; }
        set { isClear = value; }
    }
    public bool Clear1
    {
        get { return isClear1; }
        set { isClear1 = value; }
    }
    public bool Clear2
    {
        get { return isClear2; }
        set { isClear2 = value; }
    }
    public bool Clear3
    {
        get { return isClear3; }
        set { isClear3 = value; }
    }
    public bool Clear4
    {
        get { return isClear4; }
        set { isClear4 = value; }
    }
}
