using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCheck : MonoBehaviour
{
    GameObject box;

    void Start()
    {
        box = GameObject.Find("box");
    }
    /*
    void OnTriggerEnter2D(Collider2D other)
    {
        //if(other.GetComponent<Collider>().gameObject.layer == 9)
      //  {
            box.GetComponent<ObjectTransfer>().b_transferCheck = false;
            transform.position = box.transform.position;
            Debug.Log("b_transferCheck");
      //  }
       // else
      //  {
         //   box.GetComponent<ObjectTransfer>().b_transferCheck = true;
          //  Debug.Log("b_transferCheck");
       // }
            

    }*/

    void OnTriggerStay2D(Collider2D other)
    {
       // if (other.gameObject.layer == 9)
        //{
            Debug.Log("check");
       // }
    }
}
