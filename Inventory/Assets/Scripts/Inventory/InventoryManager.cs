using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    GameObject Inventory;
    GameObject Slot;

    public GameObject Item;

    private int Amount;

    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

    private void Awake()
    {
        Amount = 8;                                               // Slot 생성 용량 (개)
        Inventory = GameObject.Find("Inventory").transform.Find("SlotPlace").gameObject;  // Inventory 하위의 SlotPlace 가져오기(슬롯이 생성될)
        Slot = Resources.Load("Slot") as GameObject;              // Slot 프리팹 가져오기(코드 이용)

        for (int i = 0; i < Amount; i++)
        {
            slots.Add(Instantiate(Slot));                                                           // 슬롯 생성
            slots[i].transform.SetParent(Inventory.transform);                                      // 부모 설정
            slots[i].GetComponent<RectTransform>().localPosition = new Vector2(-295 + 85 * i, 0);   // 생성되는 슬롯의 위치 조정
            slots[i].transform.GetComponent<Slot>().Id = i;                                         // 생성된 슬롯의 Id 설정 (슬롯부분에 Id가 public으로 공개되어 있음)

        }
        GameObject.Find("Inventory").GetComponent<ItemDatabase>().SetDataBase();   // DataBase 내부의 (public 된) 메서드 (데이터베이스 초기화 같은 개념으로 만들었음) (시작될때 호출되면 같이 초기화되서 오류생길까봐?(

    }

}
