using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

public class GameInputManager : Singleton<GameInputManager>
{
    private PlayerController PlayerOneController;
    private PlayerController PlayerTwoController;
    
    public bool canInput;

    private void Start()
    {
        /*PlayerOneController.InitPlayerOne();
        PlayerTwoController.InitPlayerTwo();*/

        /*PlayerOneController.playerDevice = InputManager.Devices[0];
        PlayerTwoController.playerDevice = InputManager.Devices[1];*/
    }

    private void Update()
    {
        if (canInput)
        {
            if(Game.Instance.turn == Turn.PlayerOne)
                CheckPlayerOneInput();
            else if(Game.Instance.turn == Turn.PlayerTwo)
                CheckPlayerTwoInput();
        }
    }

    private void CheckPlayerOneInput()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            Game.Instance.SelectUpCell();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Game.Instance.SelectLeftCell();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Game.Instance.SelectDownCell();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Game.Instance.SelectRightCell();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && Game.Instance.canThrowDices) //Tirar dados
        {
            Game.Instance.canThrowDices = false;
            GameVisualManager.Instance.StartThrowDicesCinematic();
            Game.Instance.throwAgain = false;
        }
        else if (Input.GetKeyDown(KeyCode.E) && !Game.Instance.throwAgain) //Seleccionar
        {
            Game.Instance.SelectCell();
        }
        else if (Input.GetKeyDown(KeyCode.R) && !Game.Instance.throwAgain && Game.Instance.remainingMoves > 0) //Nueva ficha
        {
            Game.Instance.SpawnNewToken();
        }
        
    }

    private void CheckPlayerTwoInput()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            Game.Instance.SelectUpCell();
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            Game.Instance.SelectLeftCell(); 
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            Game.Instance.SelectDownCell();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            Game.Instance.SelectRightCell();
        }
        else if (Input.GetKeyDown(KeyCode.U) && Game.Instance.canThrowDices) //Tirar dados
        {
            Game.Instance.canThrowDices = false;
            GameVisualManager.Instance.StartThrowDicesCinematic();
            Game.Instance.throwAgain = false;
        }
        else if (Input.GetKeyDown(KeyCode.O) && !Game.Instance.throwAgain) //Seleccionar
        {
            Game.Instance.SelectCell();
        }
        else if (Input.GetKeyDown(KeyCode.P) && !Game.Instance.throwAgain && Game.Instance.remainingMoves > 0) //Nueva ficha
        {
            Game.Instance.SpawnNewToken();
        }
    }
}
