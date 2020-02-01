using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    public AnimationCurve curve;
    public float duration = 0.2f;

    public void Animate()
    {
        transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), duration).SetEase(curve);
    }
}
