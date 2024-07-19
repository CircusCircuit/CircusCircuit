using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class MapSpawnManager : MonoBehaviour
{
    //[SerializeField] List<GameObject> maps;
    //[SerializeField] Transform PlayerPos;
    [SerializeField] Camera cam;
    [System.Serializable]
    public class MapArray
    {
        [SerializeField] Vector2 mapSpawnPos;
        public Vector2 MapPos
        {
            get { return mapSpawnPos; }
            set { mapSpawnPos = value; }
        }

        [SerializeField] Vector2 playerSpawnPos;
        public Vector2 PlayerPos
        {
            get { return playerSpawnPos; }
            set { playerSpawnPos = value; }
        }

        [SerializeField] Vector2 leverSpawnPos;
        public Vector2 LeverPos
        {
            get { return leverSpawnPos; }
            set { leverSpawnPos = value; }
        }

        [SerializeField] GameObject enemySpawnPos;
        public GameObject EnemyPos
        {
            get { return enemySpawnPos; }
            set { enemySpawnPos = value; }
        }
        [SerializeField] GameObject wave1;
        public GameObject Wave1
        {
            get { return wave1; }
            set { wave1 = value; }
        }

        [SerializeField] GameObject wave2;
        public GameObject Wave2
        {
            get { return wave2; }
            set { wave2 = value; }
        }

        [SerializeField] GameObject healPacks;
        public GameObject HealPacks
        {
            get { return healPacks; }
            set { healPacks = value; }
        }
    }

    int rIndex;

    [SerializeField] List<MapArray> stageArray;
    [SerializeField] GameObject leverObj;
    [SerializeField] GameObject player;

    bool doSpawn = false;
    bool doHealPackSpawn = false;
    //bool wave2Spawn = false;

    GameObject curWaveObj;

    //private bool isClear=false;
    List<MapArray> dynamicStageArray;
    private void Start()
    {
        SoundManager.instance.Play("1_BGM", 0.6f, SoundType.BGM);
        Allocation();
        MapSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.getNextWave)
        {
            doSpawn = false;

            foreach (var stage in stageArray)
            {
                stage.EnemyPos.SetActive(false);
                stage.Wave1.SetActive(false);
                stage.Wave2.SetActive(false);
            }
            MapSpawn();

            GameManager.Instance.getNextWave = false;
        }

        if (doSpawn)
        {
            if (AllWave1EnemiesDefeated())
            {
                ActivateWave2();
                if (!doHealPackSpawn)
                {
                    ActivateHealPacks();
                }


                if (GameManager.Instance.Wave1Clear && AllWave2EnemiesDefeated())
                {
                    GameManager.Instance.Clear = true;
                    GameManager.Instance.Wave1Clear = false;
                    doHealPackSpawn = false;

                    doSpawn = false;
                }
            }
        }

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    MapSpawn();

        //    if (dynamicStageArray.Count <= 0)
        //    {
        //        Allocation();
        //    }
        //}
    }

    void Allocation()
    {
        dynamicStageArray = new List<MapArray>(new MapArray[stageArray.Count]);
        for (int i = 0; i < stageArray.Count; i++)
        {
            dynamicStageArray[i] = stageArray[i];
        }
    }

    void MapSpawn()
    {
        rIndex = Random.Range(0, dynamicStageArray.Count);

        cam.transform.position = dynamicStageArray[rIndex].MapPos;
        leverObj.transform.position = dynamicStageArray[rIndex].LeverPos;
        player.transform.position = dynamicStageArray[rIndex].PlayerPos;
        dynamicStageArray[rIndex].EnemyPos.SetActive(true);

        curWaveObj = dynamicStageArray[rIndex].EnemyPos.gameObject;

        dynamicStageArray[rIndex].Wave1.SetActive(true);
        dynamicStageArray[rIndex].Wave2.SetActive(false);

        dynamicStageArray.RemoveAt(rIndex);

        doSpawn = true;
    }


    bool AllWave1EnemiesDefeated()
    {
        if (/*stageArray[rIndex].Wave1*/curWaveObj.transform.GetChild(0).transform.childCount == 0)
        {
            print("wave1 Å¬¸®¾î");
            return true;
        }
        return false;
    }
    bool AllWave2EnemiesDefeated()
    {
        if (/*stageArray[rIndex].Wave2*/curWaveObj.transform.GetChild(1).transform.childCount == 0)
        {
            return true;
        }
        return false;
    }

    void ActivateWave2()
    {
        curWaveObj.transform.GetChild(1).gameObject.SetActive(true);
        GameManager.Instance.Wave1Clear = true;
    }

    void ActivateHealPacks()
    {
        stageArray[rIndex].HealPacks.SetActive(true);
        doHealPackSpawn = true;
    }
}

