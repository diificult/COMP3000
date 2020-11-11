using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnValueChange : UnityEvent<int> { }

[System.Serializable]
public class OnDiceRoll : UnityEvent<int> { }

public class Player : MonoBehaviour
{

    public GameObject PlayerLocation;

    public UnityEvent OnMoveComplete;

    public OnValueChange onValueChange;

    private int MovesLeft;

    private bool DiceRolled = false;

    public OnDiceRoll onDiceRoll;

    [SerializeField]
    private int PlayerNumber;

    private void OnEnable()
    {
        DiceRolled = false;
    }

    public void Update()
    {
        if (!DiceRolled)
        {
            
            if (Input.GetAxis("RollDicePlayer"+PlayerNumber) > 0)
            {
                DiceRolled = true;
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
        }
    }



    public IEnumerator MoveOverSpeed(Vector3 end, float speed)
    {
        while (transform.position != end)
        {
            transform.position = Vector3.MoveTowards(transform.position, end, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        Move();

    }


}
