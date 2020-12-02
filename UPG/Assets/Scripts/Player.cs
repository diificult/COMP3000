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

    public OnDiceRoll onDiceRoll;

   // [SerializeField]
    public int PlayerNumber;

    public Color PlayerColour;

    public TextMeshProUGUI CoinText;

    private int Coins;

    public void allowedToRoll()
    {
        CanRoll = true;
    }



    public void Update()
    {

        //Potentially inefficency here
        if (!PosRolled)
        {
            if (Input.GetAxis("RollDicePlayer" + PlayerNumber) > 0)
            {
                PosRolled = true;
                int Roll = Random.Range(1, 7);
                GetComponentInChildren<PositionDiceRoll>().DiceRolled(Roll);
                onPosDiceRoll.Invoke(PlayerNumber, Roll);


            }
        }
        else if (CanRoll)
        {
            if (Input.GetAxis("RollDicePlayer" + PlayerNumber) > 0)
            {
                CanRoll = false;
                MovesLeft = Random.Range(1, 7);
                onDiceRoll.Invoke(MovesLeft);
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

}
