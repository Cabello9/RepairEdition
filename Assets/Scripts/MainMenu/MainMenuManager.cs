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

    private void DoFade(float duration, Action OnFinish)
    {
        Fade.DOFade(1, duration).OnComplete(() => { OnFinish(); });
    }
    

    public void LoadGame()
    {
        DoFade(0.5f, () => { SceneManager.LoadScene("Game");});
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
