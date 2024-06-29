using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBuff : MonoBehaviour
{
    [Header("Base")]
    bool EnemyDetected = false;
    bool EnemyCrashed = false;
    bool BulletCrashed = false;
    bool isPlatform = false;


    [Header("SkillTree")]
    bool coolMagic3 = false;
    bool coolMagic4 = false;

    bool coolJugg1 = false;

    bool coolAcrob1 = false;
    bool coolAcrob2 = false;
    bool coolAcrob3 = false;
    bool coolAcrob4 = false;



    string curScene;

    // Start is called before the first frame update
    void Start()
    {
        curScene = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if (curScene != "Tutorial")
        {
            DetectMagicianSkills();
            DetectJugglerSkills();
            DetectAcrobatSkills();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyCrashed = true;
        }
        else if (collision.gameObject.tag == "EnemyBullet")
        {
            BulletCrashed = true;
        }
        else if (collision.gameObject.tag == "Platform")
        {
            isPlatform = true;
        }
        else
        {
            EnemyCrashed = false;
            BulletCrashed = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyDetected = true;
        }
        else EnemyDetected = false;
    }

    void DetectMagicianSkills()
    {
        if (SaveCardData.Instance.MagicianCard.Contains(2) && EnemyDetected)
        {
            GameManager.Instance.AttackPoewr *= 2;
        }
        else if (SaveCardData.Instance.MagicianCard.Contains(3) && EnemyDetected)
        {
            if (!coolMagic3)
            {
                StartCoroutine(Magician3(200));
            }
        }
        else if (SaveCardData.Instance.MagicianCard.Contains(4) && Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(CheckPlatform());
        }
        else return;
    }

    void DetectJugglerSkills()
    {
        if (SaveCardData.Instance.JugglerCard.Contains(1) && GameManager.Instance.PlayerHp < 2 && !coolJugg1)
        {
            StartCoroutine(Juggler1(100));
        }
        else return;
    }

    void DetectAcrobatSkills()
    {
        if (SaveCardData.Instance.AcrobatCard.Contains(1) && EnemyCrashed && !coolAcrob1)
        {
            StartCoroutine(Acrobat1(100, "coolAcrob1"));
        }
        else if (SaveCardData.Instance.AcrobatCard.Contains(2) && BulletCrashed && !coolAcrob2)
        {
            StartCoroutine(Acrobat1(100, "coolAcrob2"));
        }
        else if (SaveCardData.Instance.AcrobatCard.Contains(3) && Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(CheckPlatform());
        }
        else if (SaveCardData.Instance.AcrobatCard.Contains(4) && Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(CheckPlatform());
        }
        else return;
    }

    IEnumerator Magician3(int time)
    {
        coolMagic3 = true;
        GameManager.Instance.PlayerHp += 0.25f;

        yield return new WaitForSecondsRealtime(time);
        coolMagic3 = false;
    }

    IEnumerator Magician4(int time)
    {
        coolMagic4 = true;

        yield return new WaitForEndOfFrame();

        GameManager.Instance.AttackSpeed += GameManager.Instance.AttackSpeed * 0.2f;
        yield return new WaitForSecondsRealtime(5);

        GameManager.Instance.AttackSpeed -= GameManager.Instance.AttackSpeed * 0.2f;

        yield return new WaitForSecondsRealtime(time);
        coolMagic4 = false;
    }

    IEnumerator Juggler1(int time)
    {
        coolJugg1 = true;
        GameManager.Instance.PlayerSpeed += GameManager.Instance.PlayerSpeed * 0.2f;
        yield return new WaitForSecondsRealtime(5);

        GameManager.Instance.PlayerSpeed -= GameManager.Instance.PlayerSpeed * 0.2f;

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

        GameManager.Instance.PlayerSpeed += GameManager.Instance.PlayerSpeed * 0.2f;
        yield return new WaitForSecondsRealtime(2);

        GameManager.Instance.PlayerSpeed -= GameManager.Instance.PlayerSpeed * 0.2f;

        yield return new WaitForSecondsRealtime(time);
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

        if (isPlatform)
        {
            if (SaveCardData.Instance.MagicianCard.Contains(4) && !coolMagic4)
            {
                StartCoroutine(Magician4(100));
            }
            else if (SaveCardData.Instance.AcrobatCard.Contains(3) && !coolAcrob3)
            {
                StartCoroutine(Acrobat1(100, "coolAcrob3"));
            }
            else if (SaveCardData.Instance.AcrobatCard.Contains(4) && !coolAcrob4)
            {
                StartCoroutine(Acrobat1(100, "coolAcrob4"));
            }
            yield return null;
        }
        else { yield return StartCoroutine(CheckPlatform()); }
    }

}
