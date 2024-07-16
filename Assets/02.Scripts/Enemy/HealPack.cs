using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPack : MonoBehaviour
{

    float healAmount=0.5f;

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            float currentHP = GameManager.Instance.PlayerHp;
            if(currentHP<4){
                GameManager.Instance.PlayerHp += healAmount;
            }
            //GameObject.Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }

}
