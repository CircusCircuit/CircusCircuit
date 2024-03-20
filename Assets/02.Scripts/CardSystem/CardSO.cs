using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Card_Data/Attack_Card_Info")]
public class CardSO : ScriptableObject
{
    [System.Serializable]
    public class Murtiple
    {
        //공격카드 타입
        public enum AttackType
        {
            FightPower,
            FightSpeed,
            MoveSpeed,
            MaxBullet
        }
        public AttackType Atype;

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
