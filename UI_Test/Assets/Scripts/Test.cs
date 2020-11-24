using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    public GameObject Leaf;
    public GameObject player;
    Transform trans;
    public void Start()
    {
        gameObject.SetActive(true);
        trans = GetComponent<Transform>();
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        if(Input.GetKeyDown(KeyCode.G))
    //        {
    //            Instantiate(Leaf, new Vector3(trans.position.x - 1, trans.position.y + 5, trans.position.z - 1), Quaternion.identity);
    //        }  
    //    }
    //}
    void Update()
    {
        if(Vector3.Distance(player.transform.position, trans.transform.position) <= 1)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Instantiate(Leaf, new Vector3(trans.position.x - 1, trans.position.y + 5, trans.position.z - 1), Quaternion.identity);
            }
        }
    }
    


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        PlayerBall player = other.GetComponent<PlayerBall>();
    //        Instantiate(Leaf, new Vector3(tr.position.x - 1, tr.position.y + 5, tr.position.z - 1), Quaternion.identity);
    //    }
    //}
}
