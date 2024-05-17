using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawnManager : MonoBehaviour
{
    [SerializeField] List<GameObject> maps;
    [SerializeField] Transform PlayerPos;

    private void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.getNextWave)
        {
            Destroy(GameObject.FindWithTag("Stage"));
            Spawn();

            GameManager.Instance.getNextWave = false;
            
        }
    }

    void Spawn()
    {
        int rIndex = Random.Range(0, maps.Count);
        Instantiate(maps[rIndex]);
        maps.RemoveAt(rIndex);

        PlayerPos.localPosition = GameObject.FindWithTag("SpawnPoint").transform.position;
    }
}
