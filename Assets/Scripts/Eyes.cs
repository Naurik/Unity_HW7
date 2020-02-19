using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour
{
    [SerializeField]
    private Monstr monstr;
  void OnTriggerEnter(Collider other)
  {
      if(other.tag == "Player"){
          monstr.CheckSight();
      }
  }
}
