using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[System.Serializable]
public class OnValueChange : UnityEvent<int> { }

[System.Serializable]
public class OnDiceRoll : UnityEvent<int> { }
[System.Serializable]
public class OnPosDiceRoll : UnityEvent<int, int> { }

public class Player : MonoBehaviour
{

    public GameObject PlayerLocation;

    public UnityEvent OnMoveComplete;

    public OnValueChange onValueChange;

    public OnPosDiceRoll onPosDiceRoll;

    private int MovesLeft;

    private bool CanRoll = false;

    private bool PosRolled = false;

    private bool GameStarted = false;

    public OnDiceRoll onDiceRoll;

    [SerializeField]
    private int PlayerNumber;

    public Color PlayerColour;

    public TextMeshProUGUI CoinText;

    public TextMeshProUGUI DiceText;

    private int Coins;
    GameController GameControllerScript;

    void Start()
    {
        GameControllerScript = GameObject.Find("GameController").GetComponent<GameController>();
        GameControllerScript.JoinPlayer(gameObject);
        PlayerNumber = GameControllerScript.NoPlayers;
        PlayerLocation = GameControllerScript.DefaultLocation;
        GameObject.Find("P" + PlayerNumber + " Joined").GetComponent<TextMeshProUGUI>().enabled = true;
        transform.position = PlayerLocation.transform.GetChild(PlayerNumber).position;
        DiceText = GameObject.Find("Dice Roll").GetComponent<TextMeshProUGUI>();
        
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
                int Roll = Random.Range(1, 7);
                GetComponentInChildren<PositionDiceRoll>().DiceRolled(Roll);
                GameControllerScript = GameObject.Find("GameController").GetComponent<GameController>();
                Debug.Log( PlayerNumber +" , " + Roll);
                GameControllerScript.PreGameRolled(PlayerNumber, Roll);
            }
            else if (CanRoll)
            {
                CanRoll = false;
                MovesLeft = Random.Range(1, 7);
                DiceText.text = MovesLeft.ToString();
                Move();
            }
        }
    }

    private void Move()
    {

        onValueChange.Invoke(MovesLeft);
        if (MovesLeft > 0)
        {
            MovesLeft--;
            Vector3 nextTarget = PlayerLocation.GetComponent<SpotPointers>().nextSpot[0].transform.position;
            PlayerLocation = PlayerLocation.GetComponent<SpotPointers>().nextSpot[0];
            nextTarget.y += 0.5f;
            StartCoroutine(MoveOverSpeed(nextTarget, 4.5f));
        }
        else
        {
            OnMoveComplete.Invoke();
            landonspot();
        }
    }



    public IEnumerator MoveOverSpeed(Vector3 end, float speed)
    {
        while (transform.position != end)
        {
            transform.position = Vector3.MoveTowards(transform.position, end, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        if (PlayerLocation.GetComponent<SpotPointers>().SplitSpot)
        {

        }
        Move();

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
        CoinText.text = "" + Coins;
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
