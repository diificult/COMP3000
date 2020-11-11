using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    [SerializeField]
    private GameObject[] Players;

    public Camera c;
    [SerializeField]
    private GameObject CurrentPlayer;
    [SerializeField]
    private int CurrentPlayerValue;
    [SerializeField]
    private int NoPlayers = 2;

    [SerializeField]
    private Text DiceRoll;

    void Start()
    {
        CurrentPlayer = Players[0];
    }


    public void EndOfGo()
    {
        CurrentPlayer.GetComponent<Player>().enabled = false;
        CurrentPlayerValue++;
        
        if (CurrentPlayerValue == NoPlayers)
        {
            CurrentPlayerValue = 0;
        }
        CurrentPlayer = Players[CurrentPlayerValue];
        c.GetComponent<CameraMovement>().Target = CurrentPlayer;
        CurrentPlayer.GetComponent<Player>().enabled = true;
        DiceRoll.transform.SendMessage("NewTurn", SendMessageOptions.DontRequireReceiver);
    }

    public int GetPlayerValue()
    {
        return CurrentPlayerValue;
    }

}
