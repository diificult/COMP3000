using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private int[] PlayersReady;

    [SerializeField]
    private int PlayersGo;

    public int NoPlayers = 0;

    private int TurnNumber = 1;

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

    private bool GameStarted = false;

    private bool MinigameWaiting = false;

    public GameObject MinigameCameraPoint;

    public GameObject MinigamePlayerPointsParent;

    [Header("Animation Data")]

    public Animator FinishedAni;

    public void OnEnable()
    {
        Player.OnSendCoins += updatesCoins;

    }

    private void updatesCoins(int a, int b)
    {

    }

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
        DecideCollectableSpot();
        PreGameRoll();
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
              //  Color c = Players[i].GetComponent<Player>().PlayerColour;
             //   c.a = 255;
              //  PosIndicators[Position].color = c;
              //  PosIndicators[Position].enabled = false;
            //    PosIndicators[Position].enabled = true;
            }
            Invoke("StartGame", 1f);
        }

    }

    public void StartGame()
    {
        GameStarted = true;
        foreach (GameObject p in Players)
        {
            p.transform.GetChild(0).SendMessage("done", SendMessageOptions.DontRequireReceiver);
        }
        TurnUI.SetActive(true);
        CurrentPlayer = RollOrder[0];
        vcam.Follow = CurrentPlayer.transform;
        vcam.LookAt = CurrentPlayer.transform;
       // TurnText.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, CurrentPlayer.GetComponent<Player>().PlayerColour);
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
            Fader.GetComponent<Fading>().FadeIn(15.0f);
            StartCoroutine(ViewCamera());
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
        if (GameStarted) spot = Random.Range(0, ArraySize);
        else spot = Random.Range(0, ArraySize - 1);
        Debug.Log("" + spot);
        CollectableSpot[spot].GetComponent<SpotPointers>().SetSpotType(4);
    }


    public IEnumerator ViewCamera()
    {
        yield return new WaitForSeconds(0.5f);
        vcam.Follow = ViewPosition.transform;
        vcam.LookAt = ViewPosition.transform;
        yield return new WaitForSeconds(0.8f);
        Fader.GetComponent<Fading>().FadeOut(15.0f);
        TurnNumberText.text = "TURN " + TurnNumber;
        TurnNumberText.enabled = true;
        Invoke("ShowPlayerTurn", 1.2f);
    }


    public void ShowPlayerTurn()
    {
        TurnNumberText.enabled = false;
        vcam.Follow = CurrentPlayer.transform;
        vcam.LookAt = CurrentPlayer.transform;
      // TurnText.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, CurrentPlayer.GetComponent<Player>().PlayerColour);
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

    public List<GameObject> GetPlayers()
    {
        return Players;
    }


    public void LoadMinigameGame()
    {
        Fader.GetComponent<Fading>().FadeIn(15.0f);
        Invoke("LoadMinigame2", 2.0f);
    }

    private void LoadMinigame2()
    {
        gameObject.GetComponent<SceneController>().StackerGame();
        c.transform.position = new Vector3(1005, 7, -5);
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
        }
        Invoke("FadeOut", 1.0f);
    }

    public void ReadyPlayer(int playerNumber)
    {
        Debug.Log(playerNumber + "");
        if (MinigameWaiting)
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
            Debug.LogError(player);
            PlayerMinigameCaps[i].SetActive(true);
            PlayerMinigameCaps[i].GetComponent<MeshRenderer>().material = PlayerMats[player - 1];
            MiniPosText[i].SetActive(true);
            i++;
        }
    }

    private void CallMinigameEndScreen()
    {
        MinigameEndUI.SetActive(true);
        MinigameEndUI.GetComponent<Animator>().SetTrigger("Show");
        Invoke("ResetTrigger", 1.5f);
        
    }

    private void ResetTrigger()
    {
        MinigameEndUI.GetComponent<Animator>().ResetTrigger("Show");
    }


}
