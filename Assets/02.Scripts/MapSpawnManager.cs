using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        [SerializeField] Vector2 enemySpawnPos;
        public Vector2 EnemyPos
        {
            get { return enemySpawnPos; }
            set { enemySpawnPos = value; }
        }
    }
    [SerializeField] List<MapArray> stageArray;
    [SerializeField] GameObject leverObj;
    [SerializeField] GameObject player;

    List<MapArray> dynamicStageArray;

    private void Start()
    {
        Allocation();
        MapSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.getNextWave)
        {
            MapSpawn();

            GameManager.Instance.getNextWave = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MapSpawn();

            if (dynamicStageArray.Count <= 0)
            {
                Allocation();
            }
        }
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
        int rIndex = Random.Range(0, dynamicStageArray.Count);

        cam.transform.position = dynamicStageArray[rIndex].MapPos;
        leverObj.transform.position = dynamicStageArray[rIndex].LeverPos;
        player.transform.position = dynamicStageArray[rIndex].PlayerPos;
        //player.transform.GetChild(0).localPosition = Vector3.zero;

        dynamicStageArray.RemoveAt(rIndex);
    }
}
