using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour
{
    private static int maxValue = 1;
    public int currentValue;
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
