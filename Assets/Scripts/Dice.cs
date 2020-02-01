using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour
{
    private Transform initialPosition;
    private static int maxValue = 1;
    public int currentValue;
    public Animator animator;

    private void Awake()
    {
        initialPosition = transform;
        animator = GetComponent<Animator>();

        initialPosition.transform.position = transform.parent.transform.position;
        initialPosition.transform.rotation = transform.parent.transform.rotation;
    }

    public void ResetPosition()
    {
        transform.parent.transform.position = initialPosition.transform.position;
        transform.parent.transform.rotation = initialPosition.transform.rotation;
    }

    public void ThrowDice()
    {
        SetValue(Random.Range(0, maxValue + 1));
    }

    private void SetValue(int value)
    {
        currentValue = value;
    }

    public void RollAnimation()
    {
        animator.SetInteger("DiceValue",currentValue);
    }

    public int GetValue()
    {
        return currentValue;
    }
    
    
}
