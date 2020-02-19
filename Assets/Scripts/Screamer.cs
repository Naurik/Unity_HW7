using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screamer : MonoBehaviour
{
    [SerializeField]
    private Transform screamerImage;

    [SerializeField]
    private AudioSource audioScream;
    private float destroyTime = 3f;

    private void Start(){
        // отключаем картинку с самого начала
        screamerImage.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            // show image
            screamerImage.gameObject.SetActive(true);
            // play sound
            audioScream.Play();
            // уничтожаем триггер
            Destroy(gameObject, destroyTime);
            // уничтожаем картинку
            Destroy(screamerImage.gameObject, destroyTime);
        }
    }
}
