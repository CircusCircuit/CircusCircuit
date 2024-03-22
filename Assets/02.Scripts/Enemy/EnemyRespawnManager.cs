using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyRespawnManager : MonoBehaviour
{
    public List<GameObject> monsterObject; // 리스폰할 몬스터 오브젝트
    public GameObject waveObject;
    private List<GameObject> monsters = new List<GameObject>(); // 현재 존재하는 몬스터들의 목록

    // 몬스터 생성 함수
    private void SpawnMonster()
    {

        waveObject.SetActive(true);
        
    }

    // 모든 몬스터를 확인하고, 다 죽었는지 검사하는 함수
    // private bool AreMonstersDead()
    // {
    //     foreach (GameObject monster in monsters)
    //     {
    //         if (monster.activeSelf)
    //         {
    //             // 살아있는 몬스터가 하나라도 있으면 false 반환
    //             return false;
    //         }
    //     }
    //     // 모든 몬스터가 죽었으면 true 반환
    //     return true;
    // }

    // 매 프레임마다 확인
    void Update()
    {
  
        if (GameManager.Instance.Clear1 == true)
        {
            // 새로운 몬스터 생성
            SpawnMonster();
        }
    }
}