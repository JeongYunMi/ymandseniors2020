using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public Text treeCountText;
    public Text rockCountText;
    public Text vineCountText;
    public int TotalTreeCount;
    public int TotalRockCount;
    public int TotalVineCount;
    public int TotalLeafCount;
    public int stage;

    //public GameObject Test;
    
    void Awake()
    {
        //Instantiate(Test, new Vector3(1, -0.46f, 17.06f), Quaternion.identity);
    }

    public void GetTree(int tree)
    {
        treeCountText.text = "나무 " + tree.ToString() + "/" + TotalTreeCount.ToString();
    }
    public void GetRock(int rock)
    {
        rockCountText.text = "돌맹이 " + rock.ToString() + "/" + TotalRockCount.ToString();
    }
    public void GetVine(int vine)
    {
        if(stage <= 2)
        {
            vineCountText.text = "넝쿨 " + vine.ToString() + "/" + TotalVineCount.ToString();
        }
    }
    public void GetLeaf(int leaf)
    {
        if(stage >= 3)
        {
            vineCountText.text = "나뭇잎 " + leaf.ToString() + "/" + TotalLeafCount.ToString();
        }
    }
    public void Reset()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject.Find("Items").transform.Find("Tree ("+i+")").gameObject.SetActive(true);
            GameObject.Find("Items").transform.Find("Rock ("+i+")").gameObject.SetActive(true);
            GameObject.Find("Items").transform.Find("Vine ("+i+")").gameObject.SetActive(true);
        }
    }
    
    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //        SceneManager.LoadScene(stage);
    //}
}
