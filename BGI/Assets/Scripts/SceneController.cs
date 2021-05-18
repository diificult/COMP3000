using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{

    bool isloaded = false;
    bool isUnloaded = true;

    Scene Main;
    public void StackerGameb()
    {
        if (!isloaded)
        {
            //SceneManager.LoadScene("StackerScene", LoadSceneMode.Single);
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("StackerScene");
            Scene Game = SceneManager.GetSceneByName("StackerScene"); 
            SceneManager.SetActiveScene(Game);
            Scene[] scenes = SceneManager.GetAllScenes();
            foreach (Scene s in scenes)
            {
                Debug.Log(s.name);
            }
            GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject Player in Players)
            {
                SceneManager.MoveGameObjectToScene(Player, Game);
            }



            isUnloaded = false;
            isloaded = true;
        }
    }
    public void StackerGame()
    {
        if (!isloaded)
        {
            SceneManager.LoadScene("StackerScene", LoadSceneMode.Additive);
            //SceneManager.
            //SceneManager.SetActiveScene(Game);
            
            
            GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");

           // foreach (GameObject Player in Players)
         //   {
          //      SceneManager.MoveGameObjectToScene(Player, Game);
          //  }
          //  isUnloaded = false;
            isloaded = true;
            //SceneManager.UnloadSceneAsync(Main);
        }

    }

    public void unloadStacker()
    {
        if (!isUnloaded)
        {
            SceneManager.LoadSceneAsync(Main.buildIndex, LoadSceneMode.Single);
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
