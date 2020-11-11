using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCan : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            PlayerBall player = other.GetComponent<PlayerBall>();
            player.itemCount++;
            gameObject.SetActive(false);
        }
    }
}
