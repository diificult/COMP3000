using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.Events;



public class DiceRoll : MonoBehaviour
{
    private bool diceRolled = false;
    private int Value;

    void Update()
    {
        if (!diceRolled) GenerateNumber();    
    }

    public void NewTurn()
    {
        GetComponent<Text>().enabled = true;
        diceRolled = false;
    }

    public void DiceRolled(int v)
    {
        diceRolled = true;
        Value = v;
        GetComponent<Text>().text = Value.ToString();
    }

    private void GenerateNumber()
    {
        GetComponent<Text>().text = Random.Range(1, 7).ToString();
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


}

