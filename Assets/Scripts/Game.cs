using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [Header("Game components")]
    public Board board;
    public List<Token> playerOneTokens;
    public List<Token> playerTwoTokens;

    private readonly int numberOfDices = 4;
    public bool playerOneTurn;
    public Cell selectedCell;


    public int ThrowDices()
    {
        int result = 0;

        for (int i = 0; i < numberOfDices; i++)
        {
            result += Dice.ThrowDice();
        }

        return result;
    }

    public void MoveToken()
    {
        //if(selectedCell.)
    }

    public void SelectUpCell()
    {
        if (selectedCell.upCell != null)
            selectedCell = selectedCell.upCell;
    }

    public void SelectLeftCell()
    {
        if (selectedCell.leftCell != null)
            selectedCell = selectedCell.leftCell;
    }
    public void SelectDownCell()
    {
        if (selectedCell.downCell != null)
            selectedCell = selectedCell.downCell;
    }
    public void SelectRightCell()
    {
        if (selectedCell.rightCell != null)
            selectedCell = selectedCell.rightCell;
    }
}
