using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public GameObject Target; 

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Target.transform.position + new Vector3(0, 4, -5), 0.5f) ;
    }
}
