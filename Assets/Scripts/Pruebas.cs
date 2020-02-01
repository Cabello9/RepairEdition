using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pruebas : MonoBehaviour
{
    public Token token;
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            token.CrushYAxis(0.1f,0.3f);
    }
}
