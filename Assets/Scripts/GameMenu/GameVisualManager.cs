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

    public List<GameObject> playerOneHouseParts;
    public List<GameObject> playerTwoHouseParts;

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

    IEnumerator UpdatePlayerOneHouseCoroutine(int value)
    {
        playerOneHouseParts[value - 1].SetActive(true);
        yield return null;
        //Efecto de particulas
    }

    IEnumerator UpdatePlayerTwoHouseCoroutine(int value)
    {
        playerTwoHouseParts[value - 1].SetActive(true);
        yield return null;
        //Efecto de particulas
    }

    public void UpdatePlayerOneHouse(int value)
    {
        StartCoroutine(UpdatePlayerOneHouseCoroutine(value));
    }

    public void UpdatePlayerTwoHouse(int value)
    {
        StartCoroutine(UpdatePlayerTwoHouseCoroutine(value));
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
        yield return new WaitForSeconds(2f);
        totalDicesValue = Game.Instance.ThrowDices();
        SetDiceValueHUD(totalDicesValue);
        Game.Instance.remainingMoves = totalDicesValue;
        yield return new WaitForSeconds(3f);
        GoToInitCameraPosition();
        yield return new WaitForSeconds(2f);
        EnableHUD();
        Game.Instance.IlluminateDefaultCell();

        if (totalDicesValue == 0)
        {
            Game.Instance.ChangeTurn();
            LoseTurn();
        }
    }

    public void ChangeTurnVisual()
    {
        diceValue.text = "";

        if (Game.Instance.turn == Turn.PlayerOne)
        {
            staticTurnText.color = PlayerOneColor;
            staticTurnText.text = "Player 1 turn";
        }
        else if (Game.Instance.turn == Turn.PlayerTwo)
        {
            staticTurnText.color = PlayerTwoColor;
            staticTurnText.text = "Player 2 turn";
        }
    }
    
    public void LoseTurn()
    {
        GameInputManager.Instance.canInput = false;
        if (Game.Instance.turn == Turn.PlayerOne)
        {
            PlayerTurnText.GetComponent<TextMeshProUGUI>().color = PlayerTwoColor;
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
        mainCamera.DOMove(diceCameraPoint.position, 1.5f);
    }

    private void GoToInitCameraPosition()
    {
        mainCamera.DOMove(initCameraPoint.position, 1.5f);
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
