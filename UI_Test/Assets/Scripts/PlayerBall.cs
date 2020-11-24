using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBall : MonoBehaviour {

    public float jumpPower = 15;
    public int treeCount;
    public int rockCount;
    public int vineCount;
    public int leafCount;
    public GameManager manager;
    int isJump = 0;
    Rigidbody rigid;

    public void Leaf()
    {
        leafCount++;
        manager.GetLeaf(leafCount);
    }

    void Awake()
    {
        isJump = 0;
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && isJump != 2)
        {
            isJump++;
            rigid.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        }
    }
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        rigid.AddForce(new Vector3(h, 0, v), ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJump = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tree")
        {
            other.gameObject.SetActive(false);
            manager.GetTree(treeCount);
        }
        else if (other.tag == "Rock")
        {
            other.gameObject.SetActive(false);
            manager.GetRock(rockCount);
        }
        else if (other.tag == "Vine")
        {
            other.gameObject.SetActive(false);
            manager.GetVine(vineCount);
        } 

        if(manager.stage == 3)
        {

        }
        else if(manager.TotalTreeCount <= treeCount && manager.TotalRockCount <= rockCount && manager.TotalVineCount <= vineCount)
        {
            treeCount = 0;
            rockCount = 0;
            vineCount = 0;
            if(manager.stage == 0)
            {
                manager.TotalTreeCount = 2;
                manager.TotalRockCount = 2;
                manager.TotalVineCount = 2;
                manager.stage++;
            }else if(manager.stage == 1)
            {
                manager.TotalTreeCount = 1;
                manager.TotalRockCount = 1;
                manager.TotalVineCount = 1;
                manager.stage++;
            }else if(manager.stage == 2)
            {
                manager.TotalTreeCount = 1;
                manager.TotalRockCount = 1;
                manager.TotalLeafCount = 10;
                manager.stage++;
            }
            manager.GetTree(treeCount);
            manager.GetRock(rockCount);
            manager.GetVine(vineCount);
            manager.GetLeaf(leafCount);
            manager.Reset();
        }
    }
}
