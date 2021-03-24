using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotPointers : MonoBehaviour
{

    public bool SplitSpot = false;

    [SerializeField]
    private int SpotType = 0;
    // 0 = Default
    // 1 = Green
    // 2 = Red
    // 3 = Blue
    // 4 = Gold

    public Material[] SpotMaterials;

    public GameObject[] nextSpot = new GameObject[1];

    public GameObject[] prevSpot = new GameObject[1];

    public GameObject[] splitArrows;


     void Start()
    {
        SpotType = Random.Range(0, 3);
        gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = SpotMaterials[SpotType];
    }

    public void SetSpotType(int value) {
        SpotType = value;
        gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = SpotMaterials[SpotType];
        
    }

    public int GetSpotType() {
        return SpotType;
    }

}
