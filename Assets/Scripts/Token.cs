using System;
using System.Collections;
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

    public ParticleSystem starsP;
    public ParticleSystem shieldP;
    public ParticleSystem finishP;
    public ParticleSystem rollDicesP;
    
    public Vector3 ownScale;

    public AnimationCurve JumpCurve;
    
    private Tween currenTween;
    private Sequence sequence;

    private void Start()
    {
        cell.type = CellType.Start;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void DeactivateShield()
    {
        shieldP.Stop();
        shieldP.gameObject.SetActive(false);
    }

    public void ActivateShield()
    {
        shieldP.gameObject.SetActive(true);
        shieldP.Play();
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
        currenTween = transform.DOMove(position, duration);
    }

    public void LookAt(Vector3 target, float duration)
    {
        currenTween = transform.DOLookAt(target, duration);
    }

    public void Kill(Vector3 position, float duration)
    {
        currenTween = transform.DOJump(position,1.5f,1, duration).SetEase(JumpCurve);
    }

    public void CrushYAxis(float finalY,float duration)
    {
        currenTween = transform.DOScaleY(finalY, duration);
    }
    
    public void JumpToPosition(Vector3 position, float duration)
    {
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOJump(position, 1f, 1, duration)).
            Append(transform.DORotate(cell.referencePoint.eulerAngles, 0.5f));
    }

    public void Finish(Vector3 position)
    {
        StartCoroutine(DelayFinish(position));
    }

    IEnumerator DelayFinish(Vector3 position)
    {
        finishP.Play();
        yield return new WaitForSeconds(0.8f);
        JumpToPosition(position, 0.7f);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
    public void FinishVisualContent()
    {
        starsP.Stop();
        shieldP.Stop();
        finishP.Stop();
        rollDicesP.Stop();
        sequence?.Kill();
        currenTween?.Kill();
    }
}
