using UnityEngine;

public static class Dice
{
    public static int maxValue = 1;

    public static int ThrowDice()
    {
        return Random.Range(0, maxValue + 1);
    }
}
