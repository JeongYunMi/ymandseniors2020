using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public List<GameObject> slots;
    public InventoryManager IM;
    public GameMessage GM;     // 게임 메시지 띄워주기 위해 가져오기
    public void Start()
    {
        gameObject.SetActive(true);
    }
    void Update()
    {
        IM = GameObject.Find("Canvas").transform.Find("Inventory").gameObject.GetComponent<InventoryManager>();
        GM = GameObject.Find("Canvas").transform.Find("GameMessage").gameObject.GetComponent<GameMessage>();
        slots = IM.slots;    // Inventory 하위의 InventoryManager 코드 안의 (public된 변수) slots를 가져옴.

    }
    private void OnTriggerEnter(Collider other) 
    {
        // 태그 별 게임 메시지에 출력할 이름 지정해주기
        string name;
        if (gameObject.tag == "tree")
            name = "나뭇잎";
        else if (gameObject.tag == "water")
            name = "물";
        else if (gameObject.tag == "jabdongsani")
            name = "종류별 잡동사니";
        else if (gameObject.tag == "tresh")
            name = "쓰레기";
        else  // 지정된 이름이 없다면 tag 그대로 지정
            name = gameObject.tag;




        if (other.tag == "Player")     // player와 닿았을때
        {
            int BreakorLeave = 0;      // 파괴여부 결정 변수.  1이면 파괴, 0이면 존재

            // 플레이어가 습득할 tag 지정    
            if (gameObject.tag == "tree")           
                BreakorLeave = Fillslots(0, 1); 
            else if (gameObject.tag == "water")
                BreakorLeave = Fillslots(0, 2);
            else if(gameObject.tag == "tresh")
            {
                BreakorLeave = Fillslots(0, 3);
            }
            else if (gameObject.tag == "jabdongsani")  //  여기서부터는 쓸데없는 아이 jabdongsani, tresh. (인벤토리 꽉차면 못먹는지 확인하기 위해 만듦)
            {
                BreakorLeave = Fillslots(0, 4);
                BreakorLeave = Fillslots(0, 5);
                BreakorLeave = Fillslots(0, 6);
                BreakorLeave = Fillslots(0, 7);
                BreakorLeave = Fillslots(0, 8);
                BreakorLeave = Fillslots(0, 9);
            }

            if (BreakorLeave == 1)
            {
                
                gameObject.SetActive(false);
                GM.Popup_Message(name + "(을)를 얻었습니다.");
            }
            else
            {
                GM.Popup_Message("인벤토리가 꽉 차 " + name + "(을)를 얻을수 없습니다");

            }

        }
    }


    //  슬롯 채우는 함수 (재귀함수로 만듦) 

    //  Fillslots(채울 슬롯의 번호, 채울 물건의 고유 번호){}       ex) 1. 나무  2. 물  3. 쓰레기 ... Etc
    int Fillslots(int num, int will_contain)      
    {
        Slot slot = slots[num].transform.GetComponent<Slot>();
        if (slot.Contained == will_contain) // 담을 것과 내용물이 같다면
        {
            slot.Count += 1;
            slot.txt.text = slot.Count.ToString();
        }
        else
        {
            if (slot.Contained == 0)                  // 슬롯이 비워있다면
            {
                slot.Contained = will_contain;
                slot.Count += 1;
                slot.txt.text = slot.Count.ToString();
            }
            else
            {
                if (num + 1 < 8)                       // 슬롯이 채워져있으면 다음 슬롯 확인
                {
                    return Fillslots(num + 1, will_contain);      // 재귀함수로 처리
                }
                else                                    // 슬롯이 없다면 로그띄우기
                    return 0;   // 다음은 BreakOrLeave에 영향을 준다. 더 채울 슬롯이 없다면 0을 반환하여 아이템 파괴X
            }
        }

        if (slot.Count < 2)        // 수량이 2 이하면 수량 미표시
            slot.txt.text = "";

        return 1;               // 다음은 BreakOrLeave에 영향을 준다. 아이템 파괴O
    }
}
