using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{

    [Header("Player Data")]
    [SerializeField]
    private List<GameObject> Players = new List<GameObject>();

    [SerializeField]
    private GameObject[] RollOrder;

    [SerializeField]
    private Image[] PosIndicators;

    private int[] PreGameRolls;
   
    public GameObject CurrentPlayer;
    [SerializeField]
    private int PlayersGo;
  
    public int NoPlayers = 0;

    private int TurnNumber = 1;


    private int toRoll;


    public GameObject DefaultLocation;

    public GameObject PlayerJoinController;

    [Header("Camera Data")]
    public Camera c;
    public GameObject Cine;
    public CinemachineVirtualCamera vcam;


    [Header("UI Data")]

    public GameObject Fader;

    [SerializeField]
    private TextMeshProUGUI DiceRoll;

    [SerializeField]
    private TextMeshProUGUI TurnText;

    [SerializeField]
    private TextMeshProUGUI TurnNumberText;

    [SerializeField]
    private GameObject TurnUI;

    public GameObject ViewPosition;

    public Canvas PlayerUI;
    public Canvas JoinUI;


    public void OnEnable()
    {
        Player.OnSendCoins += updatesCoins;

    }

 

    public void GameLoad()
    {
        JoinUI.gameObject.SetActive(false);
        Transform AveragePosition = transform;
        CinemachineTargetGroup group; 
        foreach (GameObject p in Players)
        {
            p.SendMessage("GameStarting", SendMessageOptions.RequireReceiver);
            AveragePosition.position += p.transform.position;
            
        }

        AveragePosition.position = AveragePosition.position / 3;
        vcam.LookAt = AveragePosition;
        vcam.Follow = AveragePosition;

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
        //Order Decided prioritising lower players
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
            Invoke(StartGame(), 1f);
        }
      
    } 

    public void StartGame()
    {
        foreach (GameObject p in Players)
        {
            p.transform.GetChild(0).SendMessage("done", SendMessageOptions.DontRequireReceiver);
        }
            TurnUI.SetActive(true);
        CurrentPlayer = RollOrder[0];
        vcam.Follow = CurrentPlayer.transform;
        vcam.LookAt = CurrentPlayer.transform;
        TurnText.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, CurrentPlayer.GetComponent<Player>().PlayerColour);
        TurnText.text = "PLAYER " + CurrentPlayer.GetComponent<Player>().GetPlayerNo() + " STARTS";

        TurnText.enabled = true;
        Invoke("ShowPlayerUI", 2f);
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
        Invoke("ShowPlayerTurn", 1.2f);
    }


    public void ShowPlayerTurn()
    {
        TurnNumberText.enabled = false;
        vcam.Follow = CurrentPlayer.transform;
        vcam.LookAt = CurrentPlayer.transform;
        TurnText.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, CurrentPlayer.GetComponent<Player>().PlayerColour);
        TurnText.text = "PLAYER " + CurrentPlayer.GetComponent<Player>().GetPlayerNo() + " TURN";
        TurnText.enabled = true;
        Invoke("ShowPlayerUI", 2f);
    }
    private void ShowPlayerUI()
    {
        PlayerUI.enabled = true;
        TurnText.enabled = false;
    }

    public void ButtonClicked()
    {
        Debug.Log("Button Clicked");
    }

    public int GetPlayerValue()
    {
        return PlayersGo;
    }


    public void JoinPlayer(GameObject player)
    {
        NoPlayers++;
        Players.Add(player);
    }
  
}
