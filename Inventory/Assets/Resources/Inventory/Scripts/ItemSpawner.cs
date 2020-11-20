using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    int SpawnCount;

    GameObject tree;
    GameObject water;
    GameObject tresh;

    GameObject Player;

    void Start()
    {

        tree = Resources.Load<GameObject>("Inventory/Items/Prefabs/tree") as GameObject;
        water = Resources.Load<GameObject>("Inventory/Items/Prefabs/water") as GameObject;
        tresh = Resources.Load<GameObject>("Inventory/Items/Prefabs/tresh") as GameObject;

        Player = GameObject.FindGameObjectWithTag("Player") as GameObject;
        
        SpawnCount = Random.Range(1, 10);


        for (int i = 0; i < SpawnCount; i++)
            Instantiate(tree, Player.transform.position + new Vector3(Random.Range(-30, 30), 0, Random.Range(-30, 30)), Quaternion.identity);

        for (int i = 0; i < SpawnCount; i++)
            Instantiate(water, Player.transform.position + new Vector3(Random.Range(-30, 30), 0, Random.Range(-30, 30)), Quaternion.identity);

        for (int i = 0; i < SpawnCount; i++)
            Instantiate(tresh, Player.transform.position + new Vector3(Random.Range(-30, 30), 0, Random.Range(-30, 30)), Quaternion.identity);
    }
}
