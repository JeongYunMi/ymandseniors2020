using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Slider Hp;

    private void Update()
    {
        if(Input.GetKey(KeyCode.H))
        {
            Hp.value -= 0.1f;
        }
    }

}
