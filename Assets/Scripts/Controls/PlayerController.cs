using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private MyPlayerActions playerActions;

    public bool isPlayerOne;

    private void Start()
    {
        InitPlayerActions();
    }

    private void InitPlayerActions()
    {
        playerActions = new MyPlayerActions();
        
        playerActions.Up.AddDefaultBinding(InputControlType.DPadUp);
        playerActions.Left.AddDefaultBinding(InputControlType.DPadLeft);
        playerActions.Down.AddDefaultBinding(InputControlType.DPadDown);
        playerActions.Right.AddDefaultBinding(InputControlType.DPadRight);
        
        playerActions.Select.AddDefaultBinding(InputControlType.Action1);
        playerActions.Back.AddDefaultBinding(InputControlType.Action2);
        playerActions.SpawnToken.AddDefaultBinding(InputControlType.Action3);
        playerActions.RollDices.AddDefaultBinding(InputControlType.Action4);

        if (isPlayerOne)
        {
            playerActions.Up.AddDefaultBinding(Key.W);
            playerActions.Left.AddDefaultBinding(Key.A);
            playerActions.Down.AddDefaultBinding(Key.S);
            playerActions.Right.AddDefaultBinding(Key.D);
            
            playerActions.Select.AddDefaultBinding(Key.Space);
            playerActions.Back.AddDefaultBinding(Key.J);
            playerActions.SpawnToken.AddDefaultBinding(Key.F);
            playerActions.RollDices.AddDefaultBinding(Key.G);
        }
        else
        {
            playerActions.Up.AddDefaultBinding(Key.UpArrow);
            playerActions.Left.AddDefaultBinding(Key.LeftArrow);
            playerActions.Down.AddDefaultBinding(Key.DownArrow);
            playerActions.Right.AddDefaultBinding(Key.RightArrow);
            
            playerActions.Select.AddDefaultBinding(Key.Return);
            playerActions.Back.AddDefaultBinding(Key.Pad8);
            playerActions.SpawnToken.AddDefaultBinding(Key.Pad4);
            playerActions.RollDices.AddDefaultBinding(Key.Pad5);
        }
        
        
        
        
    }
}
