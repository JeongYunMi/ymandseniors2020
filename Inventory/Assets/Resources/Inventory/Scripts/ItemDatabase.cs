using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<GameObject> slots;
    public InventoryManager IM;
    public void SetDataBase()
    {
        IM = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        slots = IM.slots;    // Inventory 하위의 InventoryManager 코드 안의 (public된 변수) slots를 가져옴.

        // slots[4].transform.GetComponent<Slot>().Id = 123;  // 잘 가져와졌는지 초기화용
    }
    public void Update()       
    {                               // slot 이미지 변경!!
        for (int i = 0; i < 8; i++)
        {
            Slot slot = slots[i].transform.GetComponent<Slot>();
            if (slot.Contained == 0)      // 포함물이 없다면 (0)
                slot.img.sprite = Resources.Load<Sprite>("None") as Sprite; // 이미지 변경
            else if (slot.Contained == 1) // 포함물이 1(나무)이면
                slot.img.sprite = Resources.Load<Sprite>("Inventory/Items/Image/leaf") as Sprite; // 이미지 변경
            else if (slot.Contained == 2) // 포함물이 2(물)이면
                slot.img.sprite = Resources.Load<Sprite>("Inventory/Items/Image/water") as Sprite; // 이미지 변경
            // 아래는 테스트를 위해 임시로 추가해줌
            else if (slot.Contained == 3) // 포함물이 3(쓰레기)이면
                slot.img.sprite = Resources.Load<Sprite>("Inventory/Items/Image/tresh") as Sprite; // 이미지 변경
            else if (slot.Contained > 3) // 포함물이 3보다 크면
                if(slot.Contained % 2 == 1)
                    slot.img.sprite = Resources.Load<Sprite>("Inventory/Items/Image/greensquare") as Sprite; // 이미지 변경
                else
                    slot.img.sprite = Resources.Load<Sprite>("Inventory/Items/Image/bluesquare") as Sprite; // 이미지 변경

        }


    }

}
