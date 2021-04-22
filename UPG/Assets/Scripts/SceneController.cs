using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{



    public void StackerGame()
    {
        Scene Game = SceneManager.GetSceneByName("StackerScene");
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        SceneManager.LoadScene(Game.name, LoadSceneMode.Additive);
        foreach (GameObject Player in Players)
        {
            SceneManager.MoveGameObjectToScene(Player, Game);
        }

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
