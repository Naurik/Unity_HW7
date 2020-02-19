using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
   [SerializeField]
    private Light flashLight;
    private bool isOn = true;
    

    private void LightIsOn(){
        if(Input.GetKeyDown(KeyCode.F)){
            isOn = !isOn;
            flashLight.enabled = isOn;
        }
        if(isOn == true){
        }
    }
}

