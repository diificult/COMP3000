using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.Events;

[System.Serializable]
public class OnDiceRoll : UnityEvent<int> { }


public class DiceRoll : MonoBehaviour
{

    private bool DiceRolled = false;
    public OnDiceRoll onDiceRoll;
    private int Value;



    public void RollStart()
    {
        DiceRolled = false;
        GetComponent<Text>().enabled = true;
    }

    void Update()
    {
        if (!DiceRolled)
        {
            GenerateNumber();
            if (Input.GetAxis("RollDicePlayer1") > 0)
            {
                DiceRolled = true;
                onDiceRoll.Invoke(Value);
            
            }
        }
        else
        {

        }

    }

    public void ChangeValue (int i)
    {
        Value = i;
        GetComponent<Text>().text = Value.ToString();
    }


    public void HideDice()
    {
        GetComponent<Text>().enabled = false;
    }


    private void GenerateNumber()
    {
        Value = Random.Range(1, 7);
        GetComponent<Text>().text = Value.ToString();
    }
}

