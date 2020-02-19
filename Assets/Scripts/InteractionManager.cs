using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
public class InteractionManager : MonoBehaviour{

[SerializeField]
private RigidbodyFirstPersonController playerController;
    [SerializeField]
    private Image handImage;
    [SerializeField]
    private GameObject paperPanel;

    private bool hasKey = false;
    private bool hasLight =false;

    [SerializeField]
    private float interactDistance;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private FlashLight flashLight;

    private void Start(){
        // отключаем руку
        handImage.gameObject.SetActive(false);
        //Отключаем панель с запиской
        paperPanel.gameObject.SetActive(false);
    }


    private void Update(){
        Ray ray = new Ray (transform.position, transform.forward);
        RaycastHit raycastHit;
        if(Physics.Raycast(ray, out raycastHit, interactDistance, layerMask)){
            // если рука не отображается
            if(!handImage.gameObject.activeSelf){
                // показать картинку
                handImage.gameObject.SetActive(true);
            }
            Debug.Log("Ray hit object: " + raycastHit.transform.name);
            // если нажата клавиша Е
            if(Input.GetKeyDown(KeyCode.E)){
                // если смотрю на батарейки
                if(raycastHit.transform.tag == "Battery"){
                    // пополнить заряд фонарика
                    flashLight.AddEnergy();
                    // уничтожить батарейки
                    Destroy(raycastHit.transform.gameObject);
                }
                  if(raycastHit.transform.tag == "Paper"){
                      //Включаем панель
                    paperPanel.SetActive(true);
                    //Отключаем игрока
                    playerController.enabled = false;
                }
                //если смотрю на ключ
                   if(raycastHit.transform.tag == "Key"){
                       //вызов метода подбора в ключе
                       Key key = raycastHit.transform.GetComponent<Key>();
                        if(key!=null)
                            {
                                key.PickUp();
                            }
                }
                    if(raycastHit.transform.tag == "Door"){
                        if(hasKey == false){
                            //проиграть звук закрытой двери
                            // AudioSource
                           Door door = raycastHit.transform.GetComponent<Door>();
                            if(door!=null)
                            {
                                door.ChangeDoorState();
                            }
                        }   
                }
                //  if(raycastHit.transform.tag == "Candle"){
                //         if(hasLight == false){
                //             //проиграть звук закрытой двери
                //             // AudioSource
                //            Candle candle = raycastHit.transform.GetComponent<Candle>();
                //             if(hasLight!=null)
                //             {
                //                 hasLight.LightIsOn();
                //             }
                //         }
                
            }
            if(Input.GetKeyDown(KeyCode.Escape)){
                   //Включаем панель
                    paperPanel.SetActive(false);
                    //Отключаем игрока
                    playerController.enabled = true;
            }
        }else{
            //выключаем картинку
            handImage.gameObject.SetActive(false);
        }
    }
}
