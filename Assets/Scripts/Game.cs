using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [Header("Game components")]
    public Board board;
    public List<Token> playerOneTokens;
    public List<Token> playerTwoTokens;

    public Cell selectedCell;

    private readonly int numberOfDices = 4;
    public bool playerOneTurn;

    public int remainingMoves;

    public int ThrowDices()
    {
        int result = 0;

        for (int i = 0; i < numberOfDices; i++)
        {
            result += Dice.ThrowDice();
        }

        return result;
    }

    public void Move(Token token)
    {
        while (ThereAreMoreMoves())
        {
            Advance(token);
        }
    }

    private bool ThereAreMoreMoves()
    {
        return remainingMoves > 0;
    }

    private void Advance(Token token)
    {
        if (CanAdvance(token))
        {

        }
    }

    public bool CanAdvance(Token token)
    {
        return (token.isPlayerOne && token.cell.nextCellPlayerOne != null) || (!token.isPlayerOne && token.cell.nextCellPlayerTwo != null);
    }

    public bool ThereIsTeamToken(Token token, Cell cell)
    {
        bool thereIsToken = false;
        if (token.isPlayerOne)
        {

            foreach(Cell c in board.cells)
            {
                //if(cell)
            }
        }
        return thereIsToken;
    }

    public bool ThereIsEnemyToken(Token token, Cell cell)
    {
        return false;
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
