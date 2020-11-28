using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Cinemachine;
using System.Collections;

public class GameController : MonoBehaviour
{

    [SerializeField]
    private GameObject[] Players;

    [SerializeField]
    private GameObject[] RollOrder;

    [SerializeField]
    private Image[] PosIndicators;

    private int[] PreGameRolls;

    public Camera c;
    public GameObject Cine;
    public CinemachineVirtualCamera vcam;

    [SerializeField]
    private GameObject CurrentPlayer;
    [SerializeField]
    private int PlayersGo;
    [SerializeField]
    private int NoPlayers = 2;

    private int TurnNumber = 1;

    private int toRoll;

    private int Coins;

    public GameObject Fader;

    [SerializeField]
    private TextMeshProUGUI DiceRoll;

    [SerializeField]
    private TextMeshProUGUI TurnText;

    [SerializeField]
    private TextMeshProUGUI TurnNumberText;

    [SerializeField]
    private GameObject TurnUI;

    [SerializeField]
    private UnityEvent PositionsDecided;

    public GameObject ViewPosition; 

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
                Color c = Players[i].GetComponent<Player>().PlayerColour;
                c.a = 255;
                PosIndicators[Position].color = c;
                PosIndicators[Position].enabled = false;
                PosIndicators[Position].enabled = true;
            }
            
            PositionsDecided.Invoke();
            StartGame();
        }
      
    } 

    public void StartGame()
    {
        TurnUI.SetActive(true);
        CurrentPlayer = RollOrder[0];
        vcam.Follow = CurrentPlayer.transform;
        vcam.LookAt = CurrentPlayer.transform;
        TurnText.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, CurrentPlayer.GetComponent<Player>().PlayerColour);
        // TurnText.ForceMeshUpdate();
        //TurnText.UpdateFontAsset();
        TurnText.text = "PLAYER " + CurrentPlayer.GetComponent<Player>().PlayerNumber + " STARTS";

        TurnText.enabled = true;
        Invoke("HideTurnText", 2f);
    }


    public void EndOfGo()
    {
        PlayersGo++;
        if (PlayersGo == NoPlayers)
        {
            PlayersGo = 0;
            TurnNumber++;        
            CurrentPlayer = RollOrder[PlayersGo];
            Fader.GetComponent<Fading>().FadeIn();
            StartCoroutine(ViewCamera());
        } else
        {
            CurrentPlayer = RollOrder[PlayersGo];
            ShowPlayerTurn();
        }

       
    }


    public IEnumerator ViewCamera()
    {
        yield return new WaitForSeconds(0.4f);
        vcam.Follow = ViewPosition.transform;
        vcam.LookAt = ViewPosition.transform;
        yield return new WaitForSeconds(0.5f);
        Fader.GetComponent<Fading>().FadeOut();
        TurnNumberText.text = "TURN " + TurnNumber;
        TurnNumberText.enabled = true;   

 
           
         Invoke("ShowPlayerTurn", 2f);
    }


    public void ShowPlayerTurn()
    {
        TurnNumberText.enabled = false;
        vcam.Follow = CurrentPlayer.transform;
        vcam.LookAt = CurrentPlayer.transform;
        TurnText.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, CurrentPlayer.GetComponent<Player>().PlayerColour);
        // TurnText.ForceMeshUpdate();
        //TurnText.UpdateFontAsset();
        TurnText.text = "PLAYER " + CurrentPlayer.GetComponent<Player>().PlayerNumber + " TURN";

        TurnText.enabled = true;
        Invoke("HideTurnText", 2f);
    }

    public void HideTurnText()
    {

        CurrentPlayer.GetComponent<Player>().allowedToRoll();
        TurnText.enabled = false;
        DiceRoll.transform.SendMessage("NewTurn", SendMessageOptions.DontRequireReceiver);
    }

    public int GetPlayerValue()
    {
        return PlayersGo;
    }

}
