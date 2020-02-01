using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

public class GameInputManager : MonoBehaviour
{
    private PlayerController PlayerOneController;
    private PlayerController PlayerTwoController;

    public bool isPlayerOneTurn;

    private void Start()
    {
        PlayerOneController.InitPlayerOne();
        PlayerTwoController.InitPlayerTwo();

        PlayerOneController.playerDevice = InputManager.Devices[0];
        PlayerTwoController.playerDevice = InputManager.Devices[1];
    }

    private void Update()
    {
        if(isPlayerOneTurn)
            CheckPlayerOneInput();
        else
            CheckPlayerTwoInput();
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
        else if (Input.GetKeyDown(KeyCode.Q)) //Tirar dados
        {
            GameVisualManager.Instance.StartThrowDicesCinematic();
        }
        else if (Input.GetKeyDown(KeyCode.E)) //Seleccionar
        {
            Game.Instance.SelectCell();
        }
        else if (Input.GetKeyDown(KeyCode.R)) //Nueva ficha
        {
            
        }
        
    }

    private void CheckPlayerTwoInput()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            
        }
        else if (Input.GetKeyDown(KeyCode.U)) //Tirar dados
        {
            
        }
        else if (Input.GetKeyDown(KeyCode.O)) //Seleccionar
        {
            
        }
        else if (Input.GetKeyDown(KeyCode.P)) //Nueva ficha
        {
            
        }
    }
}
