using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShieldBehaviour : MonoBehaviour
{
    public float speed;

    private void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}
