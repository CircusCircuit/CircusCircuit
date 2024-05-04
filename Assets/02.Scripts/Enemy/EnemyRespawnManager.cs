using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class EnemyRespawnManager : MonoBehaviour
{
    public GameObject waveObject1;
    public GameObject waveObject2;

    public GameObject respawnUi1;
    public GameObject respawnUi2;
    private bool completeFlag =false; 
    private bool startFlag =false; 
    // 몬스터 생성 함수
    private void SpawnMonster1()
    {
        respawnUi1.SetActive(false);
        waveObject1.SetActive(true);
        startFlag = true;
    }
    private void SpawnMonster2()
    {
        respawnUi2.SetActive(false);
        waveObject2.SetActive(true);
        startFlag = true;

    }

    private void SpawnUi1(){
        respawnUi1.SetActive(true);
        Invoke("SpawnMonster1", 1.5f);
    }
    private void SpawnUi2(){
        respawnUi2.SetActive(true);
        Invoke("SpawnMonster2", 1.5f);
    }
    void Start(){
        SpawnUi1();
    }
    // 매 프레임마다 확인
    void FixedUpdate()
    {
        if (GameObject.FindGameObjectWithTag("Enemy") == null && startFlag == true)
        {
            if(!completeFlag){
                startFlag = false;
                completeFlag =true;
                SpawnUi2();
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