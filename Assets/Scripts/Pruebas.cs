using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Pruebas : MonoBehaviour
{
    public Token blueToken;
    public Token redToken;
    public Transform targetCasilla;
    public Transform initCasilla;
    public float duration;
    public float waitDuration;
    public float crushDuration;

    private void Start()
    {
        blueToken.transform.position = initCasilla.position;
        redToken.transform.localScale = new Vector3(1, 1, 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(CrushOpponent());
        
        if(Input.GetKeyDown(KeyCode.Q))
            Reset();
    }

    IEnumerator CrushOpponent()
    {
        blueToken.Kill(targetCasilla.position, 0.5f);
        yield return new WaitForSeconds(0.4f);
        redToken.CrushYAxis(0.1f, 0.2f);
    }

    private void Reset()
    {
        blueToken.transform.position = initCasilla.position;
        redToken.transform.localScale = new Vector3(1, 1, 1);

    }
}
