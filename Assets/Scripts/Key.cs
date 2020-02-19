using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Key : MonoBehaviour
{
    [SerializeField]
    private Door door;
    public void PickUp(){
        door.Unlock();
        Destroy(gameObject);
    }
}
