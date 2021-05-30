using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class StackerGame : MonoBehaviour
{

    private int[,] board = new int[6, 10];
    public GameObject GameCube;

    public int SpawnHeight = 0;
    private float CubeSize;
    private GameObject CurrentCube;
    private int x, y;

    private int playerNo;

    [Range(-1, 1)] public sbyte MoveDir = 1;

    public int counter = 0;
    private bool moving = false;
    private bool canPlace = true;

    public int xAdjust = 2;
    public int zAdjust = 0;

    public PlayerInput pi;

    public List<GameObject> allBlocks = new List<GameObject>();

    private void FixedUpdate()
    {
        //Move the block a set number of times per second based on the players height
        counter++;
        if (counter * 2 >= (10 - (SpawnHeight)))
        {
            MoveBlock();
            counter = 0;
        }
    }

    void OnEnable()
    {
        //Defines the size of the cube. Object MUST be cube space.
        //Helps decide where the cube should spawn
        CubeSize = GameCube.transform.localScale.y;
        SpawnHeight = 0;
        board = new int[6, 10];
        SpawnBlock();
        moving = true;
        pi.SwitchCurrentActionMap("Stacker");
        playerNo = GetComponent<Player>().GetPlayerNo();
    }

    private void OnDisable()
    {
        foreach(GameObject cube in allBlocks)
        {
            Destroy(cube);
        }
    }

    public void BlockPlace()
    {

        if (canPlace)
        {
            //Adding controller rumble
            var pad = InputSystem.GetDevice<Gamepad>(new InternedString("Player"+ playerNo));
            pad.SetMotorSpeeds(0.15f, 0.80f);

            //Letting the player not move or place any of the blocks
            moving = false;
            canPlace = false;
            //Creates particles
            CurrentCube.SendMessage("PlayParticles", SendMessageOptions.DontRequireReceiver);
            //After some time allow the player to start placing
            Invoke("PlaceAgain", 0.75f);
            //If the height of the block is currently at the bottom, just place the block where it is
            if (SpawnHeight == 0)
            {
                board[x, y] = 1;
                SpawnHeight++;
            }
            //Otherwise if the space below the block is clear place the block there and increase the height of the block
            else if (board[x, y - 1] == 1)
            {

                board[x, y] = 1;
                SpawnHeight++;
            }
            //Otherwise check where the block should place
            else
            {
                CheckBlock(x, SpawnHeight);
            }
            //Check to see if the player has won, if so let the game controller know that the player is finished
            if (SpawnHeight == 10)
            {
                // finished
                pad.SetMotorSpeeds(0.00f, 0.00f);
                GameObject.Find("GameController").SendMessage("PlayerFinished", playerNo, SendMessageOptions.DontRequireReceiver);
                this.enabled = false;
            }

            //Otherwise spawn another block.
            else
            {
                pad.SetMotorSpeeds(0.0f, 0.0f);
                
                Invoke("SpawnBlock", (0.3f - (SpawnHeight / 75.0f)));
            }
        }

    }

    private void PlaceAgain()
    {
        canPlace = true;
    }

    public bool CheckBlock(int x, int y)

    {
        //check to see if block below exists 
        if (board[x, y - 1] == 1)
        {
            //move the block to (x, y)
            Vector3 MovePosition;
            MovePosition.x = (x * CubeSize) + xAdjust;
            MovePosition.y = y * CubeSize;
            MovePosition.z = zAdjust;
            MovePosition += transform.position;
            CurrentCube.transform.position = MovePosition;

            board[x, y] = 1;
            return true;
        }
        //Check to see if we are about to check if we are at the bottom 
        if ((y - 1) == 0)
        {
            //move the block anyways to the bottom (x, 0)
            Vector3 MovePosition;
            MovePosition.x = x * CubeSize + xAdjust;
            MovePosition.y = 0;
            MovePosition.z = zAdjust;
            MovePosition += transform.position;
            CurrentCube.transform.position = MovePosition;

            board[x, 0] = 1;
            return true;
        }
        // otherwise use iteration 
        CheckBlock(x, y - 1);
        return true;
    }

    public void SpawnBlock()
    {
        //Chooses a random x position to place the block at.
        int r = Random.Range(0, 6);
        Vector3 SpawnPosition;
        SpawnPosition.y = SpawnHeight * CubeSize;
        SpawnPosition.x = r * CubeSize + xAdjust;
        SpawnPosition.z = zAdjust;
        SpawnPosition += transform.position;
        CurrentCube = Instantiate(GameCube, SpawnPosition, Quaternion.identity);
        x = r;
        y = SpawnHeight;
        moving = true;
        allBlocks.Add(CurrentCube);
    }

    //Move the block left and right
    private void MoveBlock()
    {
        if (moving == true)
        {
            //Check to see if the block is at the edge
            if (x == 5) MoveDir = -1;
            if (x == 0) MoveDir = 1;
            x += MoveDir;
            CurrentCube.transform.Translate(Vector3.right * MoveDir);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
