using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    int CameraMode = 0;

    //0 = No Player Control Pan; 1 = Follows Active Player; 2 = Player Controlled Pan

    public GameObject Target; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Target.transform.position + new Vector3(0, 4, -5), 0.5f);
    }
}
