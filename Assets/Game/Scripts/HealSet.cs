using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSet : MonoBehaviour
{
    private float rotateSpeed=20.0f;
    
    void FixedUpdate()
    {
        Vector3 direction=new Vector3(0,1,0);
        transform.Rotate(direction*rotateSpeed*Time.deltaTime);
    }
}
