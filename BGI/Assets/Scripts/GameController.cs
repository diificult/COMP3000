using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{

    [Header("Player Data")]
    [SerializeField]
    private List<GameObject> Players = new List<GameObject>();

    [SerializeField]
    private GameObject[] RollOrder;

    [SerializeField]
    private Image[] PosIndicators;
    [SerializeField]
    private int[] PreGameRolls;

    public GameObject CurrentPlayer;
    [SerializeField]
    private int[] PlayersReady;

    [SerializeField]
    private int PlayersGo;
    [SerializeField]
    public int NoPlayers = 0;

    private int TurnNumber = 1;
    [SerializeField]
    private int toRoll;

    public GameObject DefaultLocation;

    public GameObject PlayerJoinController;

    public Material[] PlayerMats = new Material[4];

    public GameObject[] PlayerMinigameCaps = new GameObject[4];
    public GameObject[] MiniPosText = new GameObject[4];

   

    [Header("Camera Data")]
    public Camera c;
    public Camera Minigamec;
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
    public Canvas MinigameUI;
    public GameObject MinigameEndUI;

    public TextMeshProUGUI[] MiniReadyText;

    [Header("World & Game Data")]

    [Tooltip("Potential spots. FINAL VALUE WILL NO BE CHOSEN FOR FIRST SPOT! USE FOR SPOT NEAR THE START")]
    public GameObject[] CollectableSpot;

    private GameObject LastSpot;
    [SerializeField]
    private bool GameStarted = false;

    private bool MinigameWaiting = false;

    public GameObject MinigameCameraPoint;

    public GameObject MinigamePlayerPointsParent;

    public int TurnsTilMinigame = 3;

    [Header("Animation Data")]

    public Animator FinishedAni;


    public Material getPlayerColour(int playerNo)
    {
        return PlayerMats[playerNo - 1];
    }


    public void GameLoad()
    {
        JoinUI.gameObject.SetActive(false);
        Transform AveragePosition = transform;
        CinemachineTargetGroup group;
        int i = 0;
        foreach (GameObject p in Players)
        {
            p.SendMessage("GameStarting", SendMessageOptions.RequireReceiver);
            AveragePosition.position += p.transform.position;
            i++;
        }

        AveragePosition.position = AveragePosition.position / i;
        vcam.LookAt = AveragePosition;
        vcam.Follow = AveragePosition;
        PreGameRoll();
        DecideCollectableSpot();
        
    }

    private void PreGameRoll()
    {
        toRoll = NoPlayers;
        PreGameRolls = new int[NoPlayers];
        RollOrder = new GameObject[NoPlayers];
        foreach (GameObject p in Players)
        {
            p.GetComponentInChildren<PositionDiceRoll>().enabled = true;
        }
    }

    public void PreGameRolled(int PlayerNo, int Value)
    {
        toRoll--;
        PreGameRolls[PlayerNo - 1] = Value;
        //Order Decided prioritising lower player numbers
        if (toRoll == 0)
        {

            for (int i = 0; i < PreGameRolls.Length; i++)
            {
                int Position = 0;

                for (int j = 0; j < PreGameRolls.Length; j++)
                {
                    if (i != j)
                    {
                        if (PreGameRolls[j] > PreGameRolls[i])
                        {
                            Position++;
                        }
                        else if (PreGameRolls[j] == PreGameRolls[i])
                        {
                            if (j > i) Position++;
                        }
                    }
                }
                RollOrder[Position] = Players[i];
            }
            Invoke("StartGame", 2f);
        }

    }

    //Called once the players all have rolled.
    public void StartGame()
    {
        TurnsTilMinigame = 3;
        GameStarted = true;
        foreach (GameObject p in Players)
        {
            p.transform.GetChild(0).SendMessage("done", SendMessageOptions.DontRequireReceiver);
        }
        TurnUI.SetActive(true);
        CurrentPlayer = RollOrder[0];
        vcam.Follow = CurrentPlayer.transform;
        vcam.LookAt = CurrentPlayer.transform;
        TurnText.text = "PLAYER " + CurrentPlayer.GetComponent<Player>().GetPlayerNo() + " STARTS";

        TurnText.enabled = true;
        Invoke("ShowPlayerUI", 2f);
    }


    public void EndOfGo()
    {
        PlayersGo++;
        if (PlayersGo == NoPlayers)
        {
            TurnsTilMinigame--;
            if (TurnsTilMinigame == 0)
            {
                LoadMinigameGame();
                TurnsTilMinigame = 3;
            }
            else
            {
                PlayersGo = 0;
                TurnNumber++;
                CurrentPlayer = RollOrder[PlayersGo];
                Fader.GetComponent<Fading>().FadeIn(15.0f);
                StartCoroutine(ViewCamera());
            }
        }
        else
        {
            CurrentPlayer = RollOrder[PlayersGo];
            ShowPlayerTurn();
        }


    }

    public void DecideCollectableSpot()
    {
        int ArraySize = CollectableSpot.Length;
        int spot;

        if (!GameStarted) spot = Random.Range(0, ArraySize-1);
        else
        {
            LastSpot.GetComponent<SpotPointers>().SetSpotType(1);
            do
            {
                spot = Random.Range(0, ArraySize);
            } while (CollectableSpot[spot] == LastSpot);
        }
        LastSpot = CollectableSpot[spot];
        LastSpot.GetComponent<SpotPointers>().SetSpotType(4);
        
    }


    public IEnumerator ViewCamera()
    {
        yield return new WaitForSeconds(0.5f);
        vcam.Follow = ViewPosition.transform;
        vcam.LookAt = ViewPosition.transform;
        yield return new WaitForSeconds(0.8f);
        Fader.GetComponent<Fading>().FadeOut(15.0f);
        TurnNumberText.text = "TURN " + TurnNumber + " || " + TurnsTilMinigame + " turns until minigame";
        TurnNumberText.enabled = true;
        Invoke("ShowPlayerTurn", 1.2f);
    }


    public void ShowPlayerTurn()
    {
        TurnNumberText.enabled = false;
        vcam.Follow = CurrentPlayer.transform;
        vcam.LookAt = CurrentPlayer.transform;
        TurnText.text = "PLAYER " + CurrentPlayer.GetComponent<Player>().GetPlayerNo() + " TURN";
        TurnText.enabled = true;
        Invoke("ShowPlayerUI", 2f);
    }
    private void ShowPlayerUI()
    {
        PlayerUI.enabled = true;
        TurnText.enabled = false;
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

    public List<GameObject> GetPlayers()
    {
        return Players;
    }


    public void LoadMinigameGame()
    {
        Fader.GetComponent<Fading>().FadeIn(15.0f);
        Invoke("LoadMinigame2", 1.5f);
    }

    private void LoadMinigame2()
    {
        gameObject.GetComponent<SceneController>().StackerGame();
        c.enabled = false;
        Minigamec.enabled = true;
        MinigameUI.enabled = true;
        PlayerUI.enabled = false;
        MinigameWaiting = true;
        PlayersReady = new int[NoPlayers];
        foreach (GameObject p in Players)
        {
            p.transform.position = MinigamePlayerPointsParent.gameObject.transform.GetChild(p.GetComponent<Player>().GetPlayerNo() - 1).transform.position;
            p.transform.rotation = MinigamePlayerPointsParent.gameObject.transform.GetChild(p.GetComponent<Player>().GetPlayerNo() - 1).transform.rotation;
             GameObject.Find("Player " + p.GetComponent<Player>().GetPlayerNo() + " Text").GetComponent<PlayerUI>().Hide();
        }
        Invoke("FadeOut", 1.0f);
    }

    // Ready script for the minigame, each player can ready up and only starts once all players are ready
    public void ReadyPlayer(int playerNumber)
    {
        //Checks to make sure there is a minigame waiting and if the player number isnt 0, this is to prevent a bug with the input system.
        if (MinigameWaiting && playerNumber != 0)
        {
            
            switch (PlayersReady[playerNumber - 1])
            {
                case 1:
                    PlayersReady[playerNumber - 1] = 0;
                    MiniReadyText[playerNumber - 1].text = "not ready";
                    break;
                case 0:
                    PlayersReady[playerNumber - 1] = 1;
                    MiniReadyText[playerNumber - 1].text = "ready";
                    bool isready = checkAllReady();
                    if (isready)
                    {
                        MinigameWaiting = false;
                        StartCoroutine(StartMinigame());
                    }
                    break;
                default: break;
            }

        }

    }



    private bool checkAllReady()
    {
        bool check = true;
        foreach (int checkVal in PlayersReady)
        {
            if (checkVal == 0) check = false;
        }

        return check;

    }
    private IEnumerator StartMinigame()
    {
        
        Fader.GetComponent<Fading>().FadeIn(15.0f);
        yield return new WaitForSeconds(0.5f);
        MinigameUI.enabled = false;
        Fader.GetComponent<Fading>().FadeOut(15.0f);
        GetComponent<StackerController>().enabled = true;
        foreach (GameObject p in Players)
        {
            p.GetComponent<StackerGame>().enabled = true;
            
        }

    }

    private void FadeOut()
    {
        Fader.GetComponent<Fading>().FadeOut(15.0f);
    }

    public void EndMinigame(int[] positions)
    {
        FinishedAni.SetTrigger("Trigger");

        Invoke("CallMinigameEndScreen", 2.0f);
        
        int i = 0;
        foreach(int player in positions)
        {
            Debug.Log(i + " , " + player);
            PlayerMinigameCaps[i].SetActive(true);
            PlayerMinigameCaps[i].GetComponent<MeshRenderer>().material = PlayerMats[player - 1];
            MiniPosText[i].SetActive(true);
            GetComponent<StackerController>().enabled = false;
            switch (i)
            {
                case 0:
                    Players[player-1].GetComponent<Player>().CoinChange(10);
                    break;
                case 1:
                    Players[player - 1].GetComponent<Player>().CoinChange(6);
                    break;
                case 2:
                    Players[player - 1].GetComponent<Player>().CoinChange(3);
                    break;
                case 3:
                    Players[player - 1].GetComponent<Player>().CoinChange(1);
                    break;
                default:
                    Players[player - 1].GetComponent<Player>().CoinChange(2);
                    break;
            }
            i++;
        }
    }

    private void CallMinigameEndScreen()
    {
        MinigameEndUI.SetActive(true);
        MinigameEndUI.GetComponent<Animator>().SetTrigger("Show");
        Invoke("ResetTrigger", 1.5f);
        StartCoroutine(ExitMinigame());
    }

    private IEnumerator ExitMinigame()
    {
        yield return new WaitForSeconds(7.5f);
        Fader.GetComponent<Fading>().FadeIn(15.0f);
        yield return new WaitForSeconds(0.5f);
    
        gameObject.GetComponent<SceneController>().unloadStacker();

        c.enabled = true;
        Minigamec.enabled = false;
        foreach (GameObject p in Players)
        {
            Vector3 position = p.GetComponent<Player>().PlayerLocation.transform.position;
            position.y = 0.5f;
            p.transform.position = position;
            GameObject.Find("Player " + p.GetComponent<Player>().GetPlayerNo() + " Text").GetComponent<PlayerUI>().Show();
            p.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        }

        PlayersGo = 0;
        TurnNumber++;
        CurrentPlayer = RollOrder[PlayersGo];        
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(ViewCamera());
        Fader.GetComponent<Fading>().FadeOut(15.0f);
        MinigameEndUI.SetActive(false);

    }

    private void ResetTrigger()
    {
        MinigameEndUI.GetComponent<Animator>().ResetTrigger("Show");
    }


}
