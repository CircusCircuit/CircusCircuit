using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    GameObject openCanvas;

   
    // [ 스테이지 관련 ]
    [SerializeField] bool isClear1;
    [SerializeField] bool isClear2;

    // [ 플레이어 관련 ]
    float hp = 4;
    float attackPoewr = 100;
    int curBulletCount;
    int maxBullet = 7;
    //float bulletForce = 15;
    float attackSpeed = 1;
    float playerSpeed = 5;

    // [ 몬스터 관련 ]
    float M_attackedDamage = 5;

    // [ 덱 관련 ]
    bool isCardEnhance = false;


    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);

        curBulletCount = maxBullet;

        openCanvas = GameObject.Find("OpenCanvas").gameObject;
    }

    private void Start()
    {
        StartCoroutine(CloseOpenCurtain());
    }
    
    IEnumerator CloseOpenCurtain()
    {
        yield return new WaitForSeconds(1.5f);

        Destroy(openCanvas);
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

    //public float BulletForce
    //{
    //    get { return bulletForce; }
    //    set { bulletForce = value; }
    //}

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

    public bool IsCardEnhance
    {
        get { return isCardEnhance; }
        set { isCardEnhance= value; }
    }



    public float M_AttackDamage
    {
        get { return M_attackedDamage; }
        set { M_attackedDamage = value; }
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
