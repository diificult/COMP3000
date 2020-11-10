using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnValueChange : UnityEvent<int> { }

public class Player : MonoBehaviour
{

    public GameObject PlayerLocation;

    public UnityEvent OnMoveComplete;

    public OnValueChange onValueChange;

    private int MovesLeft;

    public void MovePlayer(int roll)
    {
        MovesLeft = roll;
        Move();
        
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
