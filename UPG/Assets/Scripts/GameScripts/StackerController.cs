using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StackerController : MonoBehaviour
{

    public TextMeshProUGUI[] FinishedText = new TextMeshProUGUI[4];


    private int[] PlayerWinOrder = new int[4];
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

    }

    // Start is called before the first frame update

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
