using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockItem : MonoBehaviour {
    public void Start()
    {
        gameObject.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerBall player = other.GetComponent<PlayerBall>();
            player.rockCount++;
            gameObject.SetActive(false);
        }
    }
}
