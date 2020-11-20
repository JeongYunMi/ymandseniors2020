using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOnOFF : MonoBehaviour
{
    GameObject Inventory;     // 인벤토리 저장할
    public int Inventory_OnOff = 0;     // 인벤토리 On/Off

    void Start()
    {
        Inventory = GameObject.Find("Inventory").transform.gameObject;
        Inventory.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
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
