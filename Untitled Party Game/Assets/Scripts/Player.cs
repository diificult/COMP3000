using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject PlayerLocation;

    public void MovePlayer(int roll)
    {
        StartCoroutine(doSomething(roll));
    }

    IEnumerator doSomething(int roll)
    {
        for (int i = roll; i > 0; i--)
        {
            Debug.Log(i);
            Vector3 nextTarget = PlayerLocation.GetComponent<SpotPointers>().nextSpot[0].transform.position;
            PlayerLocation = PlayerLocation.GetComponent<SpotPointers>().nextSpot[0];
            nextTarget.y += 0.5f;
            Vector3 pos = transform.position;
            StartCoroutine(MoveOverSpeed(nextTarget, 4f));
            yield return new WaitForSeconds(1f);
        }

    }

    public IEnumerator MoveOverSpeed(Vector3 end, float speed)
    {
        // speed should be 1 unit per second
        while (transform.position != end)
        {
            transform.position = Vector3.MoveTowards(transform.position, end, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }


}
