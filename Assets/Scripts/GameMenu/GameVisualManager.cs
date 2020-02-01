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
        if (isPlayerOne) 
            ConfigPlayerOneTurn();
        else
            ConfigPlayerTwoTurn();

        StartCoroutine(PlayerTurnMovement());

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
        });
    }

    private void ConfigPlayerOneTurn()
    {
        PlayerTurnText.GetComponent<TextMeshProUGUI>().color = PlayerOneColor;
        PlayerTurnText.GetComponent<TextMeshProUGUI>().text = "Turno del Jugador 1";
    }

    private void ConfigPlayerTwoTurn()
    {
        PlayerTurnText.GetComponent<TextMeshProUGUI>().color = PlayerTwoColor;
        PlayerTurnText.GetComponent<TextMeshProUGUI>().text = "Turno del Jugador 2";
    }
}
