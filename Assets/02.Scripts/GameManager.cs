using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    GameObject openCanvas;

   
    // [ �������� ���� ]
    [SerializeField] bool isClear1;
    [SerializeField] bool isClear2;

    // [ �÷��̾� ���� ]
    float hp = 4;
    float attackPoewr = 100;
    int curBulletCount;
    float freeFeather = 50;

    int maxBullet = 7;
    float attackSpeed = 1;
    float playerSpeed = 5;

    // [ ���� ���� ]
    float M_attackedDamage = 5;

    //���
    public float coeffFightPower = 1;
    public float coeffFightSpeed = 1;
    public float coeffMoveSpeed = 1;
    public int coeffMaxBullet = 0;


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
