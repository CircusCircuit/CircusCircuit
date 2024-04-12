using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

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
            else
            {
                switch (SceneManager.GetActiveScene().buildIndex)
                {
                    case 1:
                        GameManager.Instance.Clear1 = true;
                        return;
                    case 2:
                        GameManager.Instance.Clear2 = true;
                        return;
                    case 3:
                        GameManager.Instance.Clear3 = true;
                        return;
                    case 4:
                        GameManager.Instance.Clear4 = true;
                        return;
                }
            }
        }
    }
}