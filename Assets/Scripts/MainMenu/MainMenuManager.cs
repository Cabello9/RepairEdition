using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Image Fade;
    public Animator introAnimator;
    public float timeToGoToMenu;

    private void Awake()
    {
        DoFade(0f, 0.5f, () => { introAnimator.SetBool("Play", true); });
        Invoke(nameof(OpenMenu), timeToGoToMenu);
    }

    private void DoFade(float value, float duration, Action OnFinish)
    {
        Fade.DOFade(value, duration).OnComplete(() => { OnFinish?.Invoke(); });
    }
    
    private void OpenMenu()
    {
        DoFade(1f, 0.5f, () => 
        { 
            DoFade(0f, 1f, null);
            introAnimator.gameObject.SetActive(false);
        });
        
    }

    public void LoadGame()
    {
        DoFade(1f, 0.5f, () => { SceneManager.LoadScene("Game");});
    }

    public void ExitGame()
    {
        DoFade(1f, 0.5f, () => { Application.Quit(); });
    }
}
