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

        //public bool AllWave1EnemiesDefeated()
        //{
        //    foreach (Transform enemy in wave1.transform)
        //    {
        //        if (enemy.gameObject.activeInHierarchy)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}
        //public bool AllWave2EnemiesDefeated()
        //{
        //    foreach (Transform enemy in wave2.transform)
        //    {
        //        if (enemy.gameObject.activeInHierarchy)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}
    }

    int rIndex;

    [SerializeField] List<MapArray> stageArray;
    [SerializeField] GameObject leverObj;
    [SerializeField] GameObject player;

    bool doSpawn = false;
    bool wave2Spawn = false;

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
            //print("@@@AllWave1EnemiesDefeated@@@ " + AllWave1EnemiesDefeated() + " , rIndex´Â " + rIndex);

            if (AllWave1EnemiesDefeated())
            {
                ActivateWave2();
                ActivateHealPacks();

                if (wave2Spawn && AllWave2EnemiesDefeated())
                {
                    GameManager.Instance.Clear = true;
                    wave2Spawn = false;
                }
            }
            //foreach (var stage in stageArray)
            //{
            //    if (stage.AllWave1EnemiesDefeated())
            //    {
            //        stage.ActivateWave2();
            //        stage.ActivateHealPacks();
            //    }
            //    if (stage.AllWave2EnemiesDefeated()/*&&isClear*/)
            //    {
            //        GameManager.Instance.Clear = true;
            //        //isClear = false;
            //    }
            //}
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

        dynamicStageArray[rIndex].Wave1.SetActive(true);
        dynamicStageArray[rIndex].Wave2.SetActive(false);

        dynamicStageArray.RemoveAt(rIndex);

        doSpawn = true;
    }


    public bool AllWave1EnemiesDefeated()
    {
        if (stageArray[rIndex].Wave1.transform.childCount == 0)
        {
            return true;
        }
        return false;
        //foreach (Transform enemy in dynamicStageArray[rIndex].Wave1.transform)
        //{
        //    if (enemy.gameObject.activeInHierarchy)
        //    {
        //        return false;
        //    }
        //}
        //return true;
    }
    public bool AllWave2EnemiesDefeated()
    {
        if (stageArray[rIndex].Wave2.transform.childCount == 0)
        {
            return true;
        }
        return false;
        //foreach (Transform enemy in dynamicStageArray[rIndex].Wave2.transform)
        //{
        //    if (enemy.gameObject.activeInHierarchy)
        //    {
        //        return false;
        //    }
        //}
        //return true;
    }

    public void ActivateWave2()
    {
        stageArray[rIndex].Wave2.SetActive(true);
        wave2Spawn = true;
    }

    public void ActivateHealPacks()
    {
        stageArray[rIndex].HealPacks.SetActive(true);
    }
}

