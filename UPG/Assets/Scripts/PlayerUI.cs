using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{

    public GameObject UIElement;
    private int noPlayers = 0;


    [SerializeField]
    private List<GameObject> UIs; 


    public void AddPlayer() {
        noPlayers++;
        var UI = Instantiate(UIElement, new Vector3(0, 50 * noPlayers, 0), Quaternion.identity);
        UIs.Add(UI);
        UI.transform.parent = gameObject.transform;
       
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }
     
    // Update is called once per frame
    void Update()
    {
        
    }
}
