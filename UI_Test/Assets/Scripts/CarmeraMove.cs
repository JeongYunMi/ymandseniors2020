using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarmeraMove : MonoBehaviour {

    Transform playerTransform;
    Vector3 Offset;
    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Offset = transform.position - playerTransform.position;
    }
    void LateUpdate()//연산을다하고 마지막에 업데이트 하는것들
    {   //transform 이동 관련
        transform.position = playerTransform.position + Offset;
    }
}
