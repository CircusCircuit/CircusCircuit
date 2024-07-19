using controller;
using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBuff : MonoBehaviour
{
    static public PlayerBuff Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    GameObject player;

    public bool doBoss3Skill = false;
    bool coolBoss3 = false;

    float OriginPlayerSpeed;
    float OriginAttackSpeed;

    [Header("Base")]
    bool EnemyDetected = false;
    //bool isPlatform = false;


    [Header("SkillTree")]
    bool coolMagic1 = false;
    //bool coolMagic2 = false;
    bool coolMagic3 = false;
    bool coolMagic4 = false;

    bool coolJugg1 = false;
    bool coolJugg2 = false;
    bool coolJugg3 = false;
    bool coolJugg4 = false;

    bool coolAcrob1 = false;
    bool coolAcrob2 = false;
    bool coolAcrob3 = false;
    bool coolAcrob4 = false;



    string curScene;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerBody").gameObject;
        curScene = SceneManager.GetActiveScene().name;
        OriginPlayerSpeed = GameManager.Instance.PlayerSpeed;
        OriginAttackSpeed = GameManager.Instance.AttackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;

        if (curScene != "Tutorial")
        {
            DetectMagicianSkills();
            DetectJugglerSkills();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyDetected = true;
        }
        else EnemyDetected = false;
    }

    public void Magician1Skill()
    {
        if (SaveCardData.Instance.MagicianCard.Contains(1) && !coolMagic1)
        {
            StartCoroutine(Magician1Cool());
        }
        else return;
    }

    IEnumerator Magician1Cool()
    {
        coolMagic1 = true;
        GameManager.Instance.M_AttackDamage = 1.5f;

        yield return new WaitForSecondsRealtime(5);
        GameManager.Instance.M_AttackDamage = 1;

        yield return new WaitForSecondsRealtime(10);
        coolMagic1 = false;
    }


    IEnumerator Magician3Cool(GameObject enemy)
    {
        coolMagic3 = true;
        //Status.EnemyInstance.TakeDamage(0.5f);
        enemy.GetComponent<Status>().TakeDamage(0.5f);

        yield return new WaitForSecondsRealtime(15);
        coolMagic3 = false;
    }

    IEnumerator Juggler3Cool(GameObject enemy)
    {
        coolJugg3 = true;
        //Movement.EnemyMoveInstance.EnemySpeed = 1;
        enemy.GetComponent<Movement>().Move(1);

        yield return new WaitForSecondsRealtime(15);
        coolJugg3 = false;
    }

    public void EnemyDebuffSkill(GameObject enemy)
    {
        if(enemy == null) return;

        if (SaveCardData.Instance.MagicianCard.Contains(3) && !coolMagic3)
        {
            StartCoroutine(Magician3Cool(enemy));
        }
        else if (SaveCardData.Instance.JugglerCard.Contains(3) && !coolJugg3)
        {
            StartCoroutine(Juggler3Cool(enemy));
        }
        else return;
    }

    void DetectMagicianSkills()
    {
        if (SaveCardData.Instance.MagicianCard.Contains(2))
        {
            if (EnemyDetected)
            {
                GameManager.Instance.M_AttackDamage = 1.5f;
            }
            else
            {
                GameManager.Instance.M_AttackDamage = 1;
            }
        }
        //else if (SaveCardData.Instance.MagicianCard.Contains(3) && EnemyDetected)
        //{
        //    if (!coolMagic3)
        //    {
        //        StartCoroutine(Magician3(200));
        //    }
        //}
        //else if (SaveCardData.Instance.MagicianCard.Contains(4) && Input.GetKeyDown(KeyCode.S))
        //{
        //    StartCoroutine(CheckPlatform());
        //}
        else return;
    }

    void DetectJugglerSkills()
    {
        if (SaveCardData.Instance.JugglerCard.Contains(1) && GameManager.Instance.PlayerHp < 2 && !coolJugg1)
        {
            StartCoroutine(Juggler1(10));
        }
        else return;
    }

    public void Magician4()
    {
        StartCoroutine(CheckPlatform());
    }

    //void DetectAcrobatSkills()
    //{
    //    if (SaveCardData.Instance.AcrobatCard.Contains(1) && EnemyCrashed && !coolAcrob1)
    //    {
    //        StartCoroutine(Acrobat1(100, "coolAcrob1"));
    //    }
    //    else if (SaveCardData.Instance.AcrobatCard.Contains(2) && BulletCrashed && !coolAcrob2)
    //    {
    //        StartCoroutine(Acrobat1(100, "coolAcrob2"));
    //    }
    //    else if (SaveCardData.Instance.AcrobatCard.Contains(3) && Input.GetKeyDown(KeyCode.W))
    //    {
    //        StartCoroutine(CheckPlatform());
    //    }
    //    else if (SaveCardData.Instance.AcrobatCard.Contains(4) && Input.GetKeyDown(KeyCode.S))
    //    {
    //        StartCoroutine(CheckPlatform());
    //    }
    //    else return;
    //}
    public void AcrobatSkill(int numb)
    {
        switch (numb)
        {
            case 1:
                if (SaveCardData.Instance.AcrobatCard.Contains(1) && !coolAcrob1)
                {
                    StartCoroutine(Acrobat1(15, "coolAcrob1"));
                }
                return;
            case 2:
                if (SaveCardData.Instance.AcrobatCard.Contains(2) && !coolAcrob2)
                {
                    StartCoroutine(Acrobat1(15, "coolAcrob2"));
                }
                return;
            case 3:
                if (SaveCardData.Instance.AcrobatCard.Contains(3))
                {
                    StartCoroutine(CheckPlatform());
                }
                return;
            case 4:
                if (SaveCardData.Instance.AcrobatCard.Contains(4))
                {
                    StartCoroutine(CheckPlatform());
                }
                return;
        }
    }

    IEnumerator Magician4(int time)
    {
        coolMagic4 = true;

        yield return new WaitForEndOfFrame();

        GameManager.Instance.AttackSpeed += OriginAttackSpeed * 0.5f;
        yield return new WaitForSecondsRealtime(5);

        GameManager.Instance.AttackSpeed -= OriginAttackSpeed * 0.5f;

        yield return new WaitForSecondsRealtime(time);
        coolMagic4 = false;
    }

    IEnumerator Juggler1(int time)
    {
        print("Juggler1 Buff");
        coolJugg1 = true;
        GameManager.Instance.PlayerSpeed += OriginPlayerSpeed * 0.5f;
        yield return new WaitForSecondsRealtime(5);

        GameManager.Instance.PlayerSpeed -= OriginPlayerSpeed * 0.5f;

        yield return new WaitForSecondsRealtime(time);
        coolJugg1 = false;
    }

    IEnumerator Acrobat1(int time, string coolName)
    {
        if (coolName.Equals("coolAcrob1"))
        {
            coolAcrob1 = true;
        }
        else if (coolName.Equals("coolAcrob2"))
        {
            coolAcrob2 = true;
        }
        else if (coolName.Equals("coolAcrob3"))
        {
            coolAcrob3 = true;
        }
        else if (coolName.Equals("coolAcrob4"))
        {
            coolAcrob4 = true;
        }
        yield return new WaitForEndOfFrame();

        GameManager.Instance.PlayerSpeed += OriginPlayerSpeed * 0.5f;
        //print(coolName + "Skill Changed PlayerSpped = " + GameManager.Instance.PlayerSpeed);
        yield return new WaitForSecondsRealtime(2);

        GameManager.Instance.PlayerSpeed -= OriginPlayerSpeed * 0.5f;
        //print(coolName + "Skill Returned PlayerSpped = " + GameManager.Instance.PlayerSpeed);

        yield return new WaitForSecondsRealtime(time);
        //print("SKill End Cool Time");
        if (coolName.Equals("coolAcrob1"))
        {
            coolAcrob1 = false;
        }
        else if (coolName.Equals("coolAcrob2"))
        {
            coolAcrob2 = false;
        }
        else if (coolName.Equals("coolAcrob3"))
        {
            coolAcrob3 = false;
        }
        else if (coolName.Equals("coolAcrob4"))
        {
            coolAcrob4 = false;
        }
    }

    IEnumerator CheckPlatform()
    {
        yield return new WaitForEndOfFrame();

        if (PlayerController.Instance.isBase || PlayerController.Instance.isGround)
        {
            if (SaveCardData.Instance.MagicianCard.Contains(4) && !coolMagic4)
            {
                StartCoroutine(Magician4(15));
            }
            else if (SaveCardData.Instance.AcrobatCard.Contains(3) && !coolAcrob3)
            {
                StartCoroutine(Acrobat1(15, "coolAcrob3"));
            }
            else if (SaveCardData.Instance.AcrobatCard.Contains(4) && !coolAcrob4)
            {
                StartCoroutine(Acrobat1(15, "coolAcrob4"));
            }
            yield return null;
        }
        else { yield return StartCoroutine(CheckPlatform()); }
    }



    public void Juggler2()
    {
        if (SaveCardData.Instance.JugglerCard.Contains(2) && !coolJugg2)
        {
            StartCoroutine(Jug24CoolTime("coolJugg2"));
        }
    }
    public void Juggler4()
    {
        if (SaveCardData.Instance.JugglerCard.Contains(4) && !coolJugg4)
        {
            StartCoroutine(Jug24CoolTime("coolJugg4"));
        }
    }
    IEnumerator Jug24CoolTime(string coolName)
    {
        //print("Jug24CoolTime실행");
        GameManager.Instance.PlayerHp += 0.5f;

        if (coolName.Equals("coolJugg2"))
        {
            coolJugg2 = true;
        }
        else if (coolName.Equals("coolJugg4"))
        {
            coolJugg4 = true;
        }

        yield return new WaitForSecondsRealtime(15);

        if (coolName.Equals("coolJugg2"))
        {
            coolJugg2 = false;
        }
        else if (coolName.Equals("coolJugg4"))
        {
            coolJugg4 = false;
        }
    }




    //하단 보스 스킬 가지고 있을 경우 호출
    public void Boss1()
    {
        int random = Random.Range(1, 8);

        if (GameManager.Instance.CurBulletCount == random)
        {
            GameManager.Instance.M_AttackDamage = 5;
            //다시 1로 만들어 주는 것 추가 필요
        }
    }

    public void Boss2()
    {
        if (GameManager.Instance.getNextWave)
        {
            GameObject.Find("PlayerBody").layer = 0;
            //복귀 코드 추가 필요
        }
    }

    public void Boss3()
    {
        if (EnemyDetected && !coolBoss3)
        {
            //3초간 침묵 효과, 20초 쿨타임
            StartCoroutine(Boss3Cool());
        }
    }

    IEnumerator Boss3Cool()
    {
        coolBoss3 = true;

        doBoss3Skill = true;
        yield return new WaitForSecondsRealtime(3);

        doBoss3Skill = false;
        yield return new WaitForSecondsRealtime(20);

        coolBoss3 = false;
    }
}
