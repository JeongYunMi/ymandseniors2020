using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameObject Inventory;     // 인벤토리 저장할
    public int Inventory_OnOff = 0;     // 인벤토리 On/Off

    GameObject tree;
    GameObject water;
    GameObject tresh;

    void Start()
    {
        Inventory = GameObject.Find("Inventory").transform.gameObject;
        Inventory.SetActive(false);

        tree = Resources.Load<GameObject>("tree") as GameObject;
        water = Resources.Load<GameObject>("water") as GameObject;
        tresh = Resources.Load<GameObject>("tresh") as GameObject;

        for (int i = 0; i < Random.Range(1, 10); i++)
            Instantiate(tree, new Vector3(Random.Range(-30, 30), 1, Random.Range(-30, 30)), Quaternion.identity);

        for (int i = 0; i < Random.Range(1, 10); i++)
            Instantiate(water, new Vector3(Random.Range(-30, 30), 1, Random.Range(-30, 30)), Quaternion.identity);

        for (int i = 0; i < Random.Range(1, 10); i++)
            Instantiate(tresh, new Vector3(Random.Range(-30, 30), 1, Random.Range(-30, 30)), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if (Inventory_OnOff == 0)
            {
                Inventory.SetActive(true);
                Inventory_OnOff = 1;
            }
            else if (Inventory_OnOff == 1)
            {
                Inventory.SetActive(false);
                Inventory_OnOff = 0;

            }
        }
    }
}
