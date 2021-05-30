using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{

    public void Show() {
        gameObject.BroadcastMessage("ShowTMPUI", SendMessageOptions.DontRequireReceiver);
        GetComponent<TextMeshProUGUI>().enabled = true;
    }

    public void Hide()
    {
        gameObject.BroadcastMessage("HideTMPUI", SendMessageOptions.DontRequireReceiver);
        GetComponent<TextMeshProUGUI>().enabled = false;
    }

    public void SetValue(int position, int value)
    {
        transform.GetChild(position).GetComponent<TextMeshProUGUI>().text = value + "";
    }
}
