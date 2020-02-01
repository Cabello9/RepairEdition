﻿using System;
using DG.Tweening;
using UnityEngine;

public class Token : MonoBehaviour
{
    public Vector3 initialPosition;
    public Quaternion initialRotation;
    public bool isPlayerOne;
    public Cell cell;
    public bool goalReached;
    public bool hasToJumpToken;
    public bool hasToKillToken;

    public Vector3 ownScale;

    public AnimationCurve JumpCurve;

    private void Start()
    {
        cell.type = CellType.Start;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void RestoreScale()
    {
        transform.localScale = ownScale;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        gameObject.SetActive(true);
    }

    public void ResetAll()
    {
        goalReached = false;
        hasToKillToken = false;
        hasToJumpToken = false;
    }

    public void MoveToPosition(Vector3 position, float duration)
    {
        transform.DOMove(position, duration);
    }

    public void LookAt(Vector3 target, float duration)
    {
        transform.DOLookAt(target, duration);
    }

    public void Kill(Vector3 position, float duration)
    {
        transform.DOJump(position,1.5f,1, duration).SetEase(JumpCurve);
    }

    public void CrushYAxis(float finalY,float duration)
    {
        transform.DOScaleY(finalY, duration);
    }
    
    public void JumpToPosition(Vector3 position, float duration)
    {
        transform.DOJump(position, 1f, 1, duration);
    }
}
