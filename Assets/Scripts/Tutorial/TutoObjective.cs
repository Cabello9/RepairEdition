using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TutoObjective : MonoBehaviour
{
    public Token token1;
    public Transform camera;
    public Transform cameraPointReference;
    public Transform initTokenCellPosition;
    public TextMeshProUGUI explanation;
    public TextMeshProUGUI diceText;
    public Transform finishPosition;
    public House house;

    private Coroutine callback;
    private int currentStep;
    private Tween diceTween;

    private void OnEnable()
    {
        currentStep = 0;
        diceText.text = "3";
        DoTutorial();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StopCoroutine(callback);
            DoTutorial();
        }
    }

    private void DoTutorial()
    {
        switch (currentStep)
        {
            case 0:
                currentStep++;
                callback = StartCoroutine(DiceValue());
                break;
            
            case 1:
                diceTween.Kill();
                currentStep++;
                callback = StartCoroutine(Explain());
                break;
            
            case 2:
                FinishTutorial();
                TutorialManager.Instance.NextTutorial(() =>
                {
                    gameObject.SetActive(false);
                });
                break;
        }    
    }
    
    IEnumerator DiceValue()
    {
        explanation.text = "Arriba a la derecha aparecerá el número de movimientos disponibles.";
        diceTween = diceText.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.5f).SetLoops(-1, LoopType.Yoyo);
        diceTween.Play();
        yield return null;
    }

    IEnumerator Explain()
    {
        SetCamera(cameraPointReference);
        SetToken(token1,initTokenCellPosition);
        explanation.text = "El objetivo es llevar a los 7 goblins hasta el final para reparar la casa";
        yield return new WaitForSeconds(3f);
        token1.Finish(finishPosition.position);
        yield return new WaitForSeconds(0.8f);
        house.CurrentBuildingPart = 4;
        house.RepairPart();
    }
    
    private void SetCamera(Transform reference)
    {
        camera.position = reference.position;
        camera.rotation = reference.rotation;
    }

    private void SetToken(Token token, Transform reference)
    {
        token.transform.position = reference.position;
        token.transform.rotation = reference.rotation;
    }

    public void FinishTutorial()
    {
        token1.FinishVisualContent();
        StopCoroutine(callback);
        currentStep = 0;
    }

}
