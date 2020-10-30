using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DiceRoll : MonoBehaviour
{

    private bool DiceRolled = false;
    public void TurnStart ()
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
            }
        }else
        {
            
        }

    }

    private void GenerateNumber()
    {
        int randomNumber = Random.Range(1, 7);
        GetComponent<Text>().text = randomNumber.ToString();
    }
}

