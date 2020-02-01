using System.Collections;
using System.Collections.Generic;
using InControl;
using MultiplayerWithBindingsExample;
using UnityEngine;

public class MyPlayerActions : PlayerActionSet
{
    public PlayerAction RollDices;
    public PlayerAction Select;
    public PlayerAction SpawnToken;
    public PlayerAction Back;

    public PlayerAction Up, Left, Down, Right;
    public PlayerTwoAxisAction Move;

    public MyPlayerActions()
    {
        Up = CreatePlayerAction("Move Up");
        Left = CreatePlayerAction("Move Left");
        Right = CreatePlayerAction("Move Right");
        Up = CreatePlayerAction("Move Down");
        Move = CreateTwoAxisPlayerAction(Left, Right, Up, Down);

        RollDices = CreatePlayerAction("Roll Dices");
        Select = CreatePlayerAction("Select And Accept");
        SpawnToken = CreatePlayerAction("Spawn Token");
        Back = CreatePlayerAction("Back and Deselect");
    }

}
