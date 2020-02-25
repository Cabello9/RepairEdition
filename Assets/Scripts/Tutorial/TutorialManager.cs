using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : Singleton<TutorialManager>
{
    public GameObject TutoObjective;
    public GameObject TutoSpecialCells;
    public GameObject TutoMove;
    public GameObject TutoKill;
    public int currentStep;

    public GameObject mainCamera;
    public GameObject tutoCamera;
    
    public GameObject pressFinishTutorials;
    public GameObject pressNextTutorial;
    
    public Image fade;
    public bool canInput;
    
    private bool isTutorialActive;
    
    private void Update()
    {
        if (canInput)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(FinishAllTutorialsCoroutine());
            }
        }
    }
    
    public void OnButtonClicked()
    {
        //Si no está el tutorial activo
        if (!isTutorialActive)
        {
            GameInputManager.Instance.canInput = false;
            isTutorialActive = true;
            tutoCamera.SetActive(true);
            mainCamera.SetActive(false);
            currentStep = -1;
            StartCoroutine(NextTutorialCoroutine());
        }
    }

    private void DoTutorial()
    {
        switch (currentStep)
        {
            case 0:
                TutoObjective.SetActive(true);
                break;
            
            case 1:
                TutoSpecialCells.SetActive(true);
                break;
            
            case 2:
                TutoMove.SetActive(true);
                break;
            
            case 3:
                TutoKill.SetActive(true);
                break;
            
            case 4:
                ReturnToGame();
                break;
        }
    }

    public void NextTutorial(Action onBegin)
    {
        StartCoroutine(NextTutorialCoroutine(onBegin));
    }

    IEnumerator NextTutorialCoroutine(Action onBegin = null)
    {
        pressFinishTutorials.SetActive(false);
        pressNextTutorial.SetActive(false);
        canInput = false;
        fade.DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        
        onBegin?.Invoke();
        currentStep++;
        DoTutorial();
        
        yield return new WaitForSeconds(0.1f);
        fade.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        pressFinishTutorials.SetActive(true);
        pressNextTutorial.SetActive(true);
        canInput = true;
    }

    IEnumerator FinishAllTutorialsCoroutine()
    {
        pressFinishTutorials.SetActive(false);
        pressNextTutorial.SetActive(false);
        canInput = false;
        fade.DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        FinishAllTutorials();
        yield return new WaitForSeconds(0.1f);
        fade.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
    }

    private void FinishAllTutorials()
    {
        TutoObjective.GetComponent<TutoObjective>().FinishTutorial();
        TutoObjective.SetActive(false);
        
        TutoSpecialCells.GetComponent<TutoSpecialCells>().FinishTutorial();
        TutoSpecialCells.SetActive(false);
        
        TutoKill.GetComponent<TutoKill>().FinishTutorial();
        TutoKill.SetActive(false);
        
        TutoMove.GetComponent<TutorialMove>().FinishTutorial();
        TutoMove.SetActive(false);
        
        ReturnToGame();
    }

    private void ReturnToGame()
    {
        GameInputManager.Instance.canInput = true;
        isTutorialActive = false;
        mainCamera.SetActive(true);
        tutoCamera.SetActive(false);
    }
}
