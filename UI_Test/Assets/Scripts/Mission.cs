using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour {

    public GameObject player;
    Transform trans;
    public void Start()
    {
        gameObject.SetActive(true);
        trans = GetComponent<Transform>();
        
    }
    void Update()
    {
        if (Vector3.Distance(player.transform.position, trans.transform.position) <= 1)
        {
            if(player.GetComponent<PlayerBall>().leafCount >= 5)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    Destroy(gameObject);
                }     
            }
        }
    }
}
