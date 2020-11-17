using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{

    [SerializeField]
    private GameObject[] Players;

    [SerializeField]
    private GameObject[] RollOrder;

    private int[] PreGameRolls;

    public Camera c;
    public GameObject Cine;
    //public CinemachineVirtualCamera vcam;

    [SerializeField]
    private GameObject CurrentPlayer;
    [SerializeField]
    private int PlayersGo;
    [SerializeField]
    private int NoPlayers = 2;

    private int toRoll;

    [SerializeField]
    private Text DiceRoll;

    [SerializeField]
    private GameObject TurnUI;

    [SerializeField]
    private UnityEvent PositionsDecided; 

    void Start()
    {
        PreGameRoll();
    }

    private void PreGameRoll()
    {
        toRoll = NoPlayers;
        PreGameRolls = new int[NoPlayers];
        RollOrder = new GameObject[NoPlayers];
        foreach (GameObject p in Players) {
            p.GetComponentInChildren<PositionDiceRoll>().enabled = true;
        }
    }

    public void PreGameRolled(int PlayerNo, int Value)
    {
        toRoll--;
        PreGameRolls[PlayerNo -1] = Value;
        if (toRoll == 0)
        { 
            
            for (int i = 0; i < PreGameRolls.Length; i++)
            {
                int Position=0;
                
                for (int j = 0; j < PreGameRolls.Length; j++)
                {
                    if(i != j)
                    {
                        if (PreGameRolls[j] > PreGameRolls[i])
                        {
                            Position++;
                        } else if (PreGameRolls[j] == PreGameRolls[i])
                        {
                            if (j > i) Position++;
                        }
                    }
                }
                RollOrder[Position] = Players[i];
            }
            PositionsDecided.Invoke();
            StartGame();
        }
      
    } 

    public void StartGame()
    {
        TurnUI.SetActive(true);
        CurrentPlayer = RollOrder[0];
        CurrentPlayer.GetComponent<Player>().allowedToRoll();
    }


    public void EndOfGo()
    {
        PlayersGo++;
        if (PlayersGo == NoPlayers)
        {
            PlayersGo = 0;
        }
        Debug.Log("" + PlayersGo);
        CurrentPlayer = RollOrder[PlayersGo];
        c.GetComponent<CameraMovement>().Target = CurrentPlayer;
        CurrentPlayer.GetComponent<Player>().allowedToRoll();
        //Cine.GetComponent<CinemachineVirtualCamera>().
        DiceRoll.transform.SendMessage("NewTurn", SendMessageOptions.DontRequireReceiver);
    }

    public int GetPlayerValue()
    {
        return PlayersGo;
    }

}
