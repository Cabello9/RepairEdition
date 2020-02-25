using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutoKill : MonoBehaviour
{
    public Transform camera;
    public Transform cameraPointReference;
    public Token token1;
    public Token token2;
    public TextMeshProUGUI rollDice;
    public TextMeshProUGUI explanation;
    public Transform murdererCell;
    public Transform victimCell;

    private int currentStep;
    private Coroutine callback;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StopCoroutine(callback);
            DoTutorial();
        }
    }

    private void OnEnable()
    {
        currentStep = 0;
        DoTutorial();
        explanation.text = "Prueba";
    }

    void DoTutorial()
    {
        switch (currentStep)
        {
            case 0:
                currentStep++;
                callback = StartCoroutine(Kill());
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

    IEnumerator Kill()
    {
        SetCamera(cameraPointReference);
        SetToken(token1, murdererCell);
        SetToken(token2, victimCell);
        rollDice.text = "2";
        yield return new WaitForSeconds(3f);
        token1.Kill(victimCell.position, 0.5f);
        yield return new WaitForSeconds(0.4f);
        token1.starsP.Play();
        token2.CrushYAxis(0.1f, 0.2f);
        yield return new WaitForSeconds(0.3f);
        token2.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        Repeat();
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

    private void Repeat()
    {
        token2.RestoreScale();
        token2.ResetAll();
        callback = StartCoroutine(Kill());
    }

    public void FinishTutorial()
    {
        token1.FinishVisualContent();
        token2.FinishVisualContent();
        currentStep = 0;
        StopCoroutine(callback);
    }
}
