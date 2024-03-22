using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EnemyRespawnManager : MonoBehaviour
{
    public GameObject waveObject;
    private bool completeFlag =false; 
    // 몬스터 생성 함수
    private void SpawnMonster()
    {
        waveObject.SetActive(true);
    }

    // 매 프레임마다 확인
    void FixedUpdate()
    {
        if (GameObject.FindGameObjectWithTag("Enemy") == null)
        {
            if(!completeFlag){
                SpawnMonster();
                completeFlag =true;
            }
            else{
                GameManager.Instance.Clear1 = true;
            }
        }
    }
}