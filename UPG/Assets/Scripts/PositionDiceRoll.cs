using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PositionDiceRoll : MonoBehaviour
{

    private bool diceRolled = false;
    private int Value;

    void Update()
    {
        if (!diceRolled) GenerateNumber();
    }

    private void GenerateNumber()
    {
        GetComponent<TextMeshPro>().text = Random.Range(1, 7).ToString();
    }



    public void DiceRolled(int v)
    {
        diceRolled = true;
        Value = v;
        GetComponent<TextMeshPro>().text = Value.ToString();
    }

    public void done ()
    {
        Destroy(gameObject);
    }
}
