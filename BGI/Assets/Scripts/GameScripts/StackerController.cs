using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StackerController : MonoBehaviour
{

    public TextMeshProUGUI[] FinishedText = new TextMeshProUGUI[4];


    private int[] PlayerWinOrder;
    private List<int> players = new List<int>();

    private int finished = 0;

    int noPlayers;


    private void OnEnable()
    {
        noPlayers = GameObject.FindGameObjectsWithTag("Player").Length;
        for (int i = 1; i <= noPlayers; i++)
        {
            players.Add(i);    
        }
        PlayerWinOrder = new int[players.Count];
    }


    public void PlayerFinished(int player)
    {
        FinishedText[player - 1].enabled = true;
        PlayerWinOrder[finished] = player;
        finished++;
        players.Remove(player);
        if (finished == noPlayers -1)
        {
            
            PlayerWinOrder[finished] = players[0];
        }
        GameObject[] Allplayers = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in Allplayers)
        {
            p.GetComponent<StackerGame>().enabled = false;
        }
        GetComponent<GameController>().EndMinigame(PlayerWinOrder);

    }

    public void isFinished()
    {

    }

}
