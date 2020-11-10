using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    private int GameState = 2;
    // 0 = Pre-Game
    // 1 = Player Choice
    // 2 = Player Dice Roll
    // 3 = Player Moving

    [SerializeField]
    private GameObject[] Players;

    private GameObject CurrentPlayer;
    private int CurrentPlayerValue;
    private int NoPlayers = 2;

    [SerializeField]
    private Text DiceRoll;

    void Start()
    {
        CurrentPlayer = Players[0];
    }


    public void EndOfGo()
    {
        CurrentPlayerValue++;
        if (CurrentPlayerValue == NoPlayers)
        {
            CurrentPlayerValue = 0;
        }
        CurrentPlayer = Players[CurrentPlayerValue];
        DiceRoll.transform.SendMessage("RollStart");
    }

    public int GetPlayerValue()
    {
        return CurrentPlayerValue;
    }

    public void DiceRolled(int Roll)
    {
        CurrentPlayer.transform.SendMessage("MovePlayer", Roll);
    }

}
