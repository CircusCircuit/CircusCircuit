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

    [SerializeField] bool isClear1;
    [SerializeField] bool isClear2;

    float hp = 4;
    float attackPoewr = 100;
    int curBulletCount = 7;
    int maxBullet = 7;
    float bulletForce = 15;
    float playerSpeed = 5;

    bool isCardEnhance = false;



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

    public float BulletForce
    {
        get { return bulletForce; }
        set { bulletForce = value; }
    }

    public float PlayerSpeed
    {
        get { return playerSpeed; }
        set { playerSpeed = value; }
    }

    public bool IsCardEnhance
    {
        get { return isCardEnhance; }
        set { isCardEnhance= value; }
    }





    //클리어 확인
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
}
