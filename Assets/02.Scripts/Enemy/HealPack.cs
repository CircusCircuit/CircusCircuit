using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPack : MonoBehaviour
{

    float healAmount=0.5f;

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        //print(collision.gameObject.name);
        if (collision.gameObject.tag == "Player")
        {
            float currentHP = GameManager.Instance.PlayerHp;
            if(currentHP<4){
                GameManager.Instance.PlayerHp += healAmount;
            }
            //GameObject.Destroy(this.gameObject);
            //print(this.gameObject.name);
            this.gameObject.SetActive(false);
        }
    }

}
