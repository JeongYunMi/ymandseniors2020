using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMessage : MonoBehaviour
{
    int Inventory_On_Off;
    Text Message;
    RectTransform rt;
    public float Time_to_Float;
    float cooltime = 0;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        Message = GetComponent<Text>();
        Message.text = "";

    }
    void Update()
    {
        Inventory_On_Off = GameObject.Find("ItemSpawner_And_InventoryOnOFFManager").GetComponent<InventoryOnOFF>().Inventory_OnOff;
        if (cooltime > 0)
            cooltime -= Time.deltaTime;
        else
            if(Message.text != "")
                Message.text = "";

    }

    public void Popup_Message(string msg)
    {
        // cooltime 이 끝날 시간 지정
        if (Time_to_Float == 0)      // 따로 초기화 되지 않았다면(0이라면) 1로 설정
            Time_to_Float = 1;
        Message.text = msg;
        if (cooltime <= 0)           // 문구가 사라지지 않았는데 또 띄운다면
            if(Inventory_On_Off == 0)
                rt.localPosition = new Vector3(-550, -330, 0);
            else
                rt.localPosition = new Vector3(170, -330, 0);
        else
            rt.localPosition += new Vector3(0, 30, 0);
        cooltime = Time_to_Float;
    }
}
