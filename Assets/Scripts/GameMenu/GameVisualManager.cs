using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameVisualManager : Singleton<GameVisualManager>
{
    public Image Fade;
    public Transform PlayerTurnText;

    public List<Transform> TurnScreenReferencePoints;
    public float TurnMoveDuration;

    public Color PlayerOneColor;
    public Color PlayerTwoColor;

    public bool isPlayerOneTurn = true;

    public Transform mainCamera;
    public Transform initCameraPoint;
    public Transform diceCameraPoint;
    public GameObject hud;
    public TextMeshProUGUI diceValue;
    public TextMeshProUGUI staticTurnText;

    private float time;
    public float timeBeforeRoll;
    public bool needRoll;

    public Image controls;
    public Sprite player1Controls;
    public Sprite player2Controls;

    [Header("Audio Manager")]
    public AudioManager audioManager;

    public GameObject victoryPanel;
    public TextMeshProUGUI winner;

    public House redHouse;
    public House blueHouse;

    private void Start()
    {
        DoFade(0, () => { PlayerTurn(isPlayerOneTurn); });
    }

    public void DoFade(float endValue, Action OnFinish)
    {
        Fade.DOFade(endValue, 2.5f).OnComplete(() => { OnFinish?.Invoke(); });
    }

    public void PlayerTurn(bool isPlayerOne)
    {
        DisableHUD();
        GameInputManager.Instance.canInput = false;
        if (isPlayerOne) 
            ConfigPlayerOneTurn();
        else
            ConfigPlayerTwoTurn();

        StartCoroutine(PlayerTurnMovement());

    }

    public void UpdatePlayerOneHouse(int value)
    {
        redHouse.CurrentBuildingPart = value;
        redHouse.RepairPart();
    }

    public void UpdatePlayerTwoHouse(int value)
    {
        blueHouse.CurrentBuildingPart = value;
        blueHouse.RepairPart();
    }

    private void Update()
    {
        if (needRoll)
        {
            if (time >= timeBeforeRoll)
            {
                time = 0;
                ConfigRollDiceAdvice();
                StartCoroutine(PlayerTurnMovement());
            }

            time += Time.deltaTime;
        }
    }

    private void ConfigRollDiceAdvice()
    {
        if (Game.Instance.turn == Turn.PlayerOne)
        {
            PlayerTurnText.GetComponent<TextMeshProUGUI>().color = PlayerOneColor;
        }
        else if (Game.Instance.turn == Turn.PlayerTwo)
        {
            PlayerTurnText.GetComponent<TextMeshProUGUI>().color = PlayerTwoColor;
        }
        
        PlayerTurnText.GetComponent<TextMeshProUGUI>().text = "Roll Dices";
    }

    IEnumerator PlayerTurnMovement()
    {
        PlayerTurnText.position = TurnScreenReferencePoints[0].position;
        PlayerTurnText.gameObject.SetActive(true);    
        PlayerTurnText.DOMove(TurnScreenReferencePoints[1].position, TurnMoveDuration);
        yield return new WaitForSeconds(TurnMoveDuration*2);
        PlayerTurnText.DOMove(TurnScreenReferencePoints[2].position, TurnMoveDuration).OnComplete(() =>
        {
            PlayerTurnText.gameObject.SetActive(true);
            GameInputManager.Instance.canInput = true;
            ChangeTurnVisual();
            needRoll = true;
            EnableHUD();
        });
    }

    private void ConfigPlayerOneTurn()
    {
        PlayerTurnText.GetComponent<TextMeshProUGUI>().color = PlayerOneColor;
        PlayerTurnText.GetComponent<TextMeshProUGUI>().text = "Player 1 turn";
    }

    private void ConfigPlayerTwoTurn()
    {
        PlayerTurnText.GetComponent<TextMeshProUGUI>().color = PlayerTwoColor;
        PlayerTurnText.GetComponent<TextMeshProUGUI>().text = "Player 2 turn";
    }

    public void StartThrowDicesCinematic()
    {
        StartCoroutine(ThrowDicesCoroutine());
    }

    IEnumerator ThrowDicesCoroutine()
    {
        int totalDicesValue;
        
        Game.Instance.ResetDices();
        DisableHUD();
        GoToDiceCameraPosition();
        yield return new WaitForSeconds(1f);
        totalDicesValue = Game.Instance.ThrowDices();
        SetDiceValueHUD(totalDicesValue);
        Game.Instance.remainingMoves = totalDicesValue;
        StartCoroutine(nameof(DiceRollCoroutine));
        yield return new WaitForSeconds(2f);
        GoToInitCameraPosition();
        yield return new WaitForSeconds(0.5f);
        EnableHUD();
        Game.Instance.IlluminateDefaultCell();

        if (totalDicesValue == 0)
        {
            LoseTurn();
            Game.Instance.ChangeTurn();
        }
    }

    IEnumerator DiceRollCoroutine()
    {
        yield return new WaitForSeconds(0.8f);
        audioManager.PlayDiceRoll();
    }

    public void ChangeTurnVisual()
    {
        diceValue.text = "";

        if (Game.Instance.turn == Turn.PlayerOne)
        {
            staticTurnText.color = PlayerOneColor;
            staticTurnText.text = "Player 1 turn";
            controls.sprite = player1Controls;
        }
        else if (Game.Instance.turn == Turn.PlayerTwo)
        {
            staticTurnText.color = PlayerTwoColor;
            staticTurnText.text = "Player 2 turn";
            controls.sprite = player2Controls;
        }
    }
    
    public void LoseTurn()
    {
        GameInputManager.Instance.canInput = false;
        if (Game.Instance.turn == Turn.PlayerOne)
        {
            PlayerTurnText.GetComponent<TextMeshProUGUI>().color = PlayerOneColor;
        }
        else if (Game.Instance.turn == Turn.PlayerTwo)
        {
            PlayerTurnText.GetComponent<TextMeshProUGUI>().color = PlayerTwoColor;
        }
        
        PlayerTurnText.GetComponent<TextMeshProUGUI>().text = "You lose your turn";
        
        StartCoroutine(PlayerTurnMovement());
    }

    public void RollAgain()
    {
        GameInputManager.Instance.canInput = false;
        if (Game.Instance.turn == Turn.PlayerOne)
        {
            PlayerTurnText.GetComponent<TextMeshProUGUI>().color = PlayerOneColor;
        }
        else if (Game.Instance.turn == Turn.PlayerTwo)
        {
            PlayerTurnText.GetComponent<TextMeshProUGUI>().color = PlayerTwoColor;
        }
        
        PlayerTurnText.GetComponent<TextMeshProUGUI>().text = "Roll dices again";
        
        StartCoroutine(PlayerTurnMovement());
    }

    private void GoToDiceCameraPosition()
    {
        mainCamera.DOMove(diceCameraPoint.position, 0.5f);
    }

    private void GoToInitCameraPosition()
    {
        mainCamera.DOMove(initCameraPoint.position, 0.5f);
    }

    private void DisableHUD()
    {
        hud.SetActive(false);
    }

    private void EnableHUD()
    {
        hud.SetActive(true);
    }

    private void SetDiceValueHUD(int value)
    {
        diceValue.text = value.ToString();
    }
}
