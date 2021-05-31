using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{

    bool isloaded = false;
    bool isUnloaded = true;

    public void StackerGame()
    {
        if (!isloaded)
        {
            SceneManager.LoadScene("StackerScene", LoadSceneMode.Additive);
            isloaded = true;
        }

    }

    public void unloadStacker()
    {
        SceneManager.UnloadSceneAsync("StackerScene", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        isloaded = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
