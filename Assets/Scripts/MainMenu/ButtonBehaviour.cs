using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    public AnimationCurve curve;
    public float duration = 0.2f;

    public void OnClick()
    {
        transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), duration).SetEase(curve);
        Invoke(nameof(OnDeselect), duration);
    }

    public void OnSelect()
    {
        transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f);
    }

    public void OnDeselect()
    {
        transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
    }
}
