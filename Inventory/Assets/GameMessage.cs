using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMessage : MonoBehaviour
{
    int Inventory_On_Off;
    Text Message;
    RectTransform rt;
    float cooltime = 0;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        Message = GetComponent<Text>();
        Message.text = "";
    }
    void Update()
    {
        Inventory_On_Off = GameObject.Find("GameManager").GetComponent<GameManager>().Inventory_OnOff;
        if (cooltime > 0)
            cooltime -= Time.deltaTime;
        else
            if(Message.text != "")
                Message.text = "";

    }

    public void Popup_Message(string msg)
    {
        Message.text = msg;
        if (cooltime <= 0)
            if(Inventory_On_Off == 0)
                rt.localPosition = new Vector3(-550, -330, 0);
            else
                rt.localPosition = new Vector3(170, -330, 0);
        else
            rt.localPosition += new Vector3(0, 30, 0);
        cooltime = 1;
    }
}
