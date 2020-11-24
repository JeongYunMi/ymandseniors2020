using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour {
    void Awake()
    {
        //player = GetComponent<PlayerBall>(); 
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerBall>().Leaf();
            Destroy(gameObject);
        }
    }
}
