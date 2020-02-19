using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{   //Логическая переменная для проверки заблокированной двери
    private bool isLocked;
    //логическая переменная для проверки открыта ли дверь
    private bool isOpen;
    //угол в котором дверь открыта полностью
    private float doorOpenAngle = 90f;
    //угол в котором дверь закрыта полностью
    private float doorClosedAngle=0f;
    //насколько плавно будет открываться дверь
    private float smooth = 2f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {//если дверь заблокирована
    
        if(isLocked)
        {
            //проиграть звук
            return;
        }
        //выйти из метода
           
            //если дверь открыта
        if(isOpen){
            //создаем переменную для хранения координат вращения
            Quaternion targetRotationOpen = Quaternion.Euler(0, doorOpenAngle, 0);
            transform.parent.localRotation = Quaternion.Slerp(transform.parent.localRotation,targetRotationOpen, smooth * Time.deltaTime );
        }
        //если дверь закрыта
        
        else{
            //создаем переменную для хранения координат вращения
            Quaternion targetRotationClosed = Quaternion.Euler(0, doorClosedAngle, 0);
            transform.parent.localRotation = Quaternion.Slerp(transform.parent.localRotation,targetRotationClosed, smooth * Time.deltaTime );
        }
    }
    public void Unlock(){
        isLocked = false;
    }
    public void ChangeDoorState(){
    isOpen = !isOpen;
    }
}

