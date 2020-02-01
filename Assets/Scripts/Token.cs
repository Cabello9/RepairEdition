using System;
using DG.Tweening;
using UnityEngine;

public class Token : MonoBehaviour
{
    public bool isPlayerOne;
    public Cell cell;
    public bool goalReached;
    public bool hasToJumpToken;
    public bool hasToKillToken;
    public bool throwAgain;

    public AnimationCurve JumpCurve;

    private void Start()
    {
        cell.type = CellType.Start;
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
