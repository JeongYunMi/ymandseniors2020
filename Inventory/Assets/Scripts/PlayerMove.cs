using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float x, z;
    Rigidbody rb;
    public float movespeed;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        rb.MovePosition(rb.position + new Vector3(x, 0, z) * movespeed * Time.fixedDeltaTime);
    }
}
