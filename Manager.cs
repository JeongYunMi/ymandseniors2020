using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    float Cooltime;
    int Frame;
    float Second; // 초당 프레임세려고 만듦
    public GameObject Cube;
    private void Awake()
    {
        Frame = 0;
        Instantiate(Cube, new Vector3(0, 1, 0), Quaternion.identity);
        Cooltime = 1.15f;
    }
    void Update()
    {
        Frame++;
        if (Second > 0)
            Second -= Time.deltaTime;
        else
        {
            Debug.Log("현재 초당 프레임:" + Frame);
            Second = 1.0f;
            Frame = 0;
        }

        if (Cooltime > 0)
            Cooltime -= Time.deltaTime;
        else
        {
            Instantiate(Cube, new Vector3(0, 7f, 0), Quaternion.identity);
            Cooltime = 1.15f;
        }
    }
}
