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



    public void TurnStart()
    {
        DiceRolled = false;
    }

    void Update()
    {
        if (!DiceRolled)
        {
            GenerateNumber();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DiceRolled = true;
                onDiceRoll.Invoke(Value);
                Invoke("HideDice", 2f);
            }
        }
        else
        {

        }

    }

    private void HideDice()
    {
        GetComponent<Text>().enabled = false;
    }


    private void GenerateNumber()
    {
        Value = Random.Range(1, 7);
        GetComponent<Text>().text = Value.ToString();
    }
}

