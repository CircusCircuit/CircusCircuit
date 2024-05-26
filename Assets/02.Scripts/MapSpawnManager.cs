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
        [SerializeField] int row;
        public int Row
        {
            get { return row; }
            set { row = value; }
        }

        [SerializeField] int col;
        public int Col
        {
            get { return col; }
            set { col = value; }
        }
    }
    [SerializeField] List<MapArray> stageArray;

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
        //int rIndex = Random.Range(0, maps.Count);
        //Instantiate(maps[rIndex]);
        //maps.RemoveAt(rIndex);

        //PlayerPos.localPosition = GameObject.FindWithTag("SpawnPoint").transform.position;

        int rIndex = Random.Range(0, dynamicStageArray.Count);
        cam.transform.position = new Vector2(dynamicStageArray[rIndex].Row, dynamicStageArray[rIndex].Col);

        dynamicStageArray.RemoveAt(rIndex);
    }
}
