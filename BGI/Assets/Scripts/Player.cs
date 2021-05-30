using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Player : MonoBehaviour
{

    public GameObject PlayerLocation;
    
    private int MovesLeft;

    private bool CanRoll = false;

    private bool PosRolled = false;

    private bool GameStarted = false;

    private bool SplitSpot = false;

    private bool currentlyRolling = false;

    private bool CheckingDiamondPurchase = false;

    private bool test = false;


    private PlayerUI ui;
    public Canvas PlayerUI;

    [SerializeField]
    private int PlayerNumber;

    public Material PlayerColour;


    private Dictionary<string, GameObject> DirectionChoices = new Dictionary<string, GameObject>();

    private int Coins;
    private int Diamonds;


    GameController GameControllerScript;
    DiceRoll DiceRollScript;

    void Start()
    {
        
        GameControllerScript = GameObject.Find("GameController").GetComponent<GameController>();
        GameControllerScript.JoinPlayer(gameObject);

        PlayerNumber = GameControllerScript.NoPlayers;
        PlayerLocation = GameControllerScript.DefaultLocation;

        InputSystem.SetDeviceUsage(InputSystem.GetDevice<Gamepad>().device, "Player"+PlayerNumber);

        GameObject.Find("P" + PlayerNumber + " Joined").GetComponent<TextMeshProUGUI>().enabled = true;
        transform.position = PlayerLocation.transform.GetChild(PlayerNumber).position;
        DiceRollScript = GameObject.Find("Dice Roll").GetComponent<DiceRoll>();

        PlayerColour = GameControllerScript.getPlayerColour(PlayerNumber);
        GetComponent<MeshRenderer>().material = PlayerColour;
        ui = GameObject.Find("Player " + PlayerNumber + " Text").GetComponent<PlayerUI>();
        ui.Show();

        PlayerUI = GameObject.Find("PlayerUI").GetComponent<Canvas>();

    }



    //Controller Input for start game
    public void Ready()
    {
        if (!GameStarted)
        {
            GameControllerScript = GameObject.Find("GameController").GetComponent<GameController>();
            GameStarted = true;
            GameControllerScript.GameLoad();
        } else
        {
            GameControllerScript = GameObject.Find("GameController").GetComponent<GameController>();
            Debug.Log("Calling with " + PlayerNumber);
            GameControllerScript.ReadyPlayer(PlayerNumber);
        }
    }


    //Game Controller confirms start
    public void GameStarting()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        GameStarted = true;
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
                GameControllerScript.PreGameRolled(PlayerNumber, Roll);
            }
            else if (CanRoll)
            {
                CanRoll = false;
                MovesLeft = Random.Range(1, 25);
                DiceRollScript.GetComponent<DiceRoll>().DiceRolled(MovesLeft);
                if (!CheckSpot())
                {
                    Move();
                }
            } else if (CheckingDiamondPurchase)
            {
                CheckingDiamondPurchase = false;
                if (Coins >= 10)
                {
                    
                    AddDiamonds();
                    CoinChange(-10);
                    GameControllerScript.DecideCollectableSpot();
                    GameObject.Find("DiamondPurchaseUI").GetComponent<Canvas>().enabled = false;
                    if (MovesLeft > 0) Move();
                    if (MovesLeft <= 0)
                    {
                        DiceRollScript.GetComponent<DiceRoll>().EndRoll();
                        GameControllerScript.EndOfGo();
                        landonspot();
                    }
                } else
                {
                    GameObject.Find("DiamondPurchaseUI").GetComponent<Canvas>().enabled = false;
                    if (MovesLeft > 0) Move();
                    if (MovesLeft <= 0)
                    {
                        DiceRollScript.GetComponent<DiceRoll>().EndRoll();
                        GameControllerScript.EndOfGo();
                        landonspot();
                    }
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
        } else if (PlayerLocation.GetComponent<SpotPointers>().GetSpotType() == 4)
        {
            CheckingDiamondPurchase = true;
            GameObject.Find("DiamondPurchaseUI").GetComponent<Canvas>().enabled = true;
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
            DiceRollScript.GetComponent<DiceRoll>().EndRoll();
            GameControllerScript.EndOfGo();
            currentlyRolling = false;
            landonspot();
        }
        else if (!CheckSpot())
        {
            Move();
        }

    }


    //Roll dice button action
    //Once the player wants to roll the dice run this code.
    public void RollButton()
    {
        if (GameStarted)
        {
            //Makes sure it is the players turn and that they arnt already currently rolling
            if (GameControllerScript.CurrentPlayer == gameObject && !currentlyRolling)
            {
                currentlyRolling = true;
                allowedToRoll();
                DiceRollScript.NewTurn();
                PlayerUI.enabled = false;
            }
        }
    }

    public void DirectionButton (string dir)
    {
        //Prevent the player pressing the button if they arent on a split spot
        if (SplitSpot)
        {
            //Disables all arrows 
            GameObject.Find("ArrowWest").GetComponent<Image>().enabled = false;
            GameObject.Find("ArrowNorth").GetComponent<Image>().enabled = false;
            GameObject.Find("ArrowSouth").GetComponent<Image>().enabled = false;
            GameObject.Find("ArrowEast").GetComponent<Image>().enabled = false;
            SplitSpot = false;
            //Makes sure that there are moves left, but this shouldn't be an issue as it should run in next go anyways
            if (MovesLeft > 0)
            {
                //Continues the player in the direction.
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
        int landed = PlayerLocation.GetComponent<SpotPointers>().GetSpotType();
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
        ui.SetValue(2, Coins);
    } 

    public void AddDiamonds()
    {
        Diamonds++;
        ui.SetValue(3, Diamonds);
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
