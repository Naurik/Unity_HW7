using UnityEngine;
using UnityEngine.UI;

public class FlashLight : MonoBehaviour
{
    [SerializeField]
    private Light flashLight;

    [SerializeField]
    private float maxBatteryLife, lightDrain;

    [SerializeField]
    private Image batteryIndicator;

    private float curBatteryLife;
    private bool isOn = true;

    private void Start(){
        // заполняем заряд максимальным значением на старте
        curBatteryLife = maxBatteryLife;
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.F)){
            isOn = !isOn;
            flashLight.enabled = isOn;
        }
        if(isOn == true){
            if(curBatteryLife > 0){
                curBatteryLife -= lightDrain * Time.deltaTime;
                flashLight.intensity = curBatteryLife;
                Vector3 batterySize = new Vector3(curBatteryLife / maxBatteryLife,1,1);
                batteryIndicator.transform.localScale = batterySize;
            }else{
                // если заряд вышел, выключить фонарик
                curBatteryLife = 0;
                isOn = false;
                flashLight.enabled = false;
            }
        }
    }

    public void AddEnergy(){
        curBatteryLife = maxBatteryLife;
    }
}
