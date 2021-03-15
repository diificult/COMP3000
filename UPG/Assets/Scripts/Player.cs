using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;


public class Player : MonoBehaviour
{

    public GameObject PlayerLocation;
    
    private int MovesLeft;

    private bool CanRoll = false;

    private bool PosRolled = false;

    private bool GameStarted = false;

    private bool SplitSpot = false;


    [SerializeField]
    private int PlayerNumber;

    public Color PlayerColour;

    public delegate void SendCoins(int coins, int player);
    public static event SendCoins OnSendCoins;

    private Dictionary<string, GameObject> DirectionChoices = new Dictionary<string, GameObject>();

    private int Coins;
    GameController GameControllerScript;
    DiceRoll DiceRollScript;

    void Start()
    {
        GameControllerScript = GameObject.Find("GameController").GetComponent<GameController>();
        GameControllerScript.JoinPlayer(gameObject);
        PlayerNumber = GameControllerScript.NoPlayers;
        PlayerLocation = GameControllerScript.DefaultLocation;
        GameObject.Find("P" + PlayerNumber + " Joined").GetComponent<TextMeshProUGUI>().enabled = true;
        transform.position = PlayerLocation.transform.GetChild(PlayerNumber).position;
        DiceRollScript = GameObject.Find("Dice Roll").GetComponent<DiceRoll>();
        
    }

    //Controller Input for start game
    public void StartGame()
    {
        if (!GameStarted)
        {
            GameControllerScript = GameObject.Find("GameController").GetComponent<GameController>();
            GameStarted = true;
            GameControllerScript.GameLoad();
        }
    }


    //Game Controller confirms start
    public void GameStarting()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        GameStarted = true;
        Debug.Log("GameStarted");
    }

    public void MainGameStart() { 
    }


    public void allowedToRoll()
    {
        CanRoll = true;
    }

    public void RollDice()
    {
        if (GameStarted)
        {
            if (!PosRolled)
            {
                PosRolled = true;
                int Roll = Random.Range(1, 11);
                GetComponentInChildren<PositionDiceRoll>().DiceRolled(Roll);
                GameControllerScript = GameObject.Find("GameController").GetComponent<GameController>();
                Debug.Log(PlayerNumber +" , " + Roll);
                GameControllerScript.PreGameRolled(PlayerNumber, Roll);
            }
            else if (CanRoll)
            {
                CanRoll = false;
                MovesLeft = Random.Range(1, 11);
                DiceRollScript.GetComponent<DiceRoll>().DiceRolled(MovesLeft);
                if (!CheckSpot())
                {
                    Move();
                }
            }
        }
    }

    private bool CheckSpot()
    {
        if (PlayerLocation.GetComponent<SpotPointers>().SplitSpot)
        {
            SplitSpot = true;
            DirectionChoices = new Dictionary<string, GameObject>();
            foreach (GameObject nextlocation in PlayerLocation.GetComponent<SpotPointers>().nextSpot)
            {
                if (nextlocation.transform.position.z > PlayerLocation.transform.position.z)
                {
                    GameObject.Find("ArrowNorth").GetComponent<Image>().enabled = true;
                    DirectionChoices.Add("n", nextlocation);
                    //North Button
                }
                else if (nextlocation.transform.position.z < PlayerLocation.transform.position.z)
                {
                    //South Button
                    GameObject.Find("ArrowSouth").GetComponent<Image>().enabled = true;
                    DirectionChoices.Add("s", nextlocation);
                }
                else if (nextlocation.transform.position.x > PlayerLocation.transform.position.x)
                {
                    //East Button
                    GameObject.Find("ArrowEast").GetComponent<Image>().enabled = true;
                    DirectionChoices.Add("e", nextlocation);
                }
                else if (nextlocation.transform.position.x < PlayerLocation.transform.position.x)
                {
                    //West Button
                    GameObject.Find("ArrowWest").GetComponent<Image>().enabled = true;
                    DirectionChoices.Add("w", nextlocation);
                }
                

            }
            return true;
        }
        return false;
    }

    private void Move()
    {
            DiceRollScript.ChangeValue(MovesLeft);
            MovesLeft--;
            PlayerLocation = PlayerLocation.GetComponent<SpotPointers>().nextSpot[0];
            Vector3 nextTarget = PlayerLocation.transform.position;
            
            nextTarget.y += 0.5f;
            StartCoroutine(MoveOverSpeed(nextTarget, 4.5f));

           
    }



    public IEnumerator MoveOverSpeed(Vector3 end, float speed)
    {
        while (transform.position != end)
        {
            transform.position = Vector3.MoveTowards(transform.position, end, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        if (MovesLeft <= 0)
        {
            GameControllerScript.EndOfGo();
            landonspot();
        }
        else if (!CheckSpot())
        {
            Move();
        }

    }

    public void RollButton()
    {
        if (GameStarted)
        {
            if (GameControllerScript.CurrentPlayer == gameObject)
            {
                allowedToRoll();
                DiceRollScript.NewTurn();
            }
        }
    }

    public void DirectionButton (string dir)
    {
        //Prevent the player pressing the button if they arent on a split spot
        if (SplitSpot)
        {
            GameObject.Find("ArrowWest").GetComponent<Image>().enabled = false;
            GameObject.Find("ArrowNorth").GetComponent<Image>().enabled = false;
            GameObject.Find("ArrowSouth").GetComponent<Image>().enabled = false;
            GameObject.Find("ArrowEast").GetComponent<Image>().enabled = false;
            SplitSpot = false;
            if (MovesLeft > 0)
            {
                DiceRollScript.ChangeValue(MovesLeft);
                MovesLeft--;
                GameObject NextSpot = DirectionChoices[dir.ToString()];
                PlayerLocation = NextSpot;
                Vector3 nextTarget = NextSpot.transform.position;
                nextTarget.y += 0.5f;
                StartCoroutine(MoveOverSpeed(nextTarget, 4.5f));
            }
            else
            {
                GameControllerScript.EndOfGo();
                landonspot();
            }
        }
    }

    private void landonspot()
    {
        int landed = PlayerLocation.GetComponent<SpotPointers>().SpotType;
    // 0 = Default
    // 1 = Green
    // 2 = Red
    // 3 = Blue
    // 4 = Gold
        switch(landed)
        {
            case 0:
                break;
            case 1:
                CoinChange(5);
                break;
            case 2:
                CoinChange(-3);
                break;
            default:
                break;
        }
    }

    public void CoinChange(int change)
    {
        Coins += change;
        if (Coins < 0) Coins = 0;
      //  CoinText.text = "" + Coins;
    } 

    public int GetPlayerNo()
    {
        return PlayerNumber;
    }

    public void DeviceLost()
    {
        Debug.LogWarning("Warning: Controller Disconnected");
    }

}
