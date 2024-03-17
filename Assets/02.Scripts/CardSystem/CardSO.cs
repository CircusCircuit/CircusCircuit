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
        //카드 이름
        public string cardName;
        //카드 이미지
        public Sprite cardImage;
        //확률
        public int weight;
        //증가수치
        public int increase;
    }

    public List<Murtiple> mul = new List<Murtiple>();
}
