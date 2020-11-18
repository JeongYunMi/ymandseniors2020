using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public int Id;        // 고유 번호
    public int Contained; // 내용물의 번호
    public Image img;            // 이미지
    public int Count = 0; // 개수
    public Text txt;            // 개수의 글자

    private void Awake()
    {
        img = transform.GetChild(0).GetComponent<Image>();
        txt = transform.GetChild(1).GetComponent<Text>();
    }
}
