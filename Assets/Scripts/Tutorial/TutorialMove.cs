using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialMove : MonoBehaviour
{
    public Transform camera;
    public Token token1;
    public Token token2;
    public Token token3;
    public Transform initMoveCell;
    public Transform finishMoveCell;
    public Transform token2Cell;
    public Transform token3Cell;
    public Transform cameraPointReference;
    public TextMeshProUGUI rollDice;
    public TextMeshProUGUI explanation;

    private Coroutine callback;
    private int currentStep;

    private List<Vector3> initTokenPositions;
    
    private void OnEnable()
    {
        initTokenPositions = new List<Vector3>();
        initTokenPositions.Add(token1.transform.position);
        initTokenPositions.Add(token2.transform.position);
        initTokenPositions.Add(token3.transform.position);
        
        currentStep = 0;
        DoTutorial();
        explanation.text = "Prueba 2";
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
                callback = StartCoroutine(Move());
                break;
            
            case 1:
                FinishTutorial();
                TutorialManager.Instance.NextTutorial(() =>
                {
                    gameObject.SetActive(false);
                });
                break;
        }
    }

    IEnumerator Move()
    {
        rollDice.text = "2";
        SetCamera(cameraPointReference);
        SetToken(token1,initMoveCell);
        SetToken(token2,token2Cell);
        SetToken(token3,token3Cell);
        yield return new WaitForSeconds(1.5f);
        token1.JumpToPosition(finishMoveCell.position, 0.8f);
        yield return new WaitForSeconds(1.5f);
        callback = StartCoroutine(Move());
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
        
        token1.transform.position = initTokenPositions[0];
        token2.transform.position = initTokenPositions[1];
        token3.transform.position = initTokenPositions[2];
        
        StopCoroutine(callback);
        currentStep = 0;
    }
}
