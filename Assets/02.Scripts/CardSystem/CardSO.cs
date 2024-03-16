using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Card_Data/Attack_Card_Info")]
public class CardSO : ScriptableObject
{
    //public string CardName;
    //public Sprite CardImage;

    [System.Serializable]
    public enum AttackType
    {
        FightPower,
        FightSpeed,
        MoveSpeed,
        MaxBullet
    }
    public List<AttackType> Atype = new List<AttackType>();

    [System.Serializable]
    public class Murtiple
    {
        //ī�� �̸�
        public string cardName;
        //ī�� �̹���
        public Sprite cardImage;
        //Ȯ��
        public int weight;
        //������ġ
        public int increase;
    }

    public List<Murtiple> mul = new List<Murtiple>();
}
