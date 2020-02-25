using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutoSpecialCells : MonoBehaviour
{
    public Transform camera;
    public List<Transform> cameraReferencePoints;
    public int currentStep;

    public Token token1;
    public Token token2;

    public Transform initToken1ProtectionCell;
    public Transform initRollAgainCell;
    public Transform rollAgainCell;
    public Transform protectionCell;
    public Transform nextToProtectionCell;

    public Image fade;
    public TextMeshProUGUI explanation;
    public Coroutine callback;

    private void OnEnable()
    {
        currentStep = 0;
        DoTutorial();
        explanation.text = "Al caer en la casilla de Roll Again, podrás volver a tirar los dados";
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
               callback = StartCoroutine(RollAgain());
               break;
           
           case 1:
               currentStep++;
               callback = StartCoroutine(Protection());
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

    IEnumerator RollAgain()
    {
        SetToken(token1,initRollAgainCell);
        SetCamera(cameraReferencePoints[0]);
        token1.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        token1.JumpToPosition(rollAgainCell.transform.position, 0.7f);
        yield return new WaitForSeconds(0.8f);
        token1.rollDicesP.Play();
    }

    IEnumerator Protection()
    {
        fade.DOFade(1, 0.25f);
        yield return new WaitForSeconds(0.25f);
        explanation.text = "Casilla de protección";
        SetToken(token1,initToken1ProtectionCell);
        SetToken(token2,protectionCell);
        SetCamera(cameraReferencePoints[1]);
        token2.ActivateShield();
        yield return new WaitForSeconds(0.1f);
        fade.DOFade(0, 0.25f);
        yield return new WaitForSeconds(2.5f);
        token1.JumpToPosition(nextToProtectionCell.transform.position, 0.8f);
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
        token2.FinishVisualContent();
        StopCoroutine(callback);
        currentStep = 0;
    }
}
