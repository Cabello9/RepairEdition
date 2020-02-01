using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Turn { None, PlayerOne, PlayerTwo}

public class Game : Singleton<Game>
{
    [Header("Game components")]
    public Board board;
    public List<Token> playerOneTokens;
    public List<Token> playerTwoTokens;

    public Cell playerOneStart;
    public Cell playerTwoStart;
    public Cell selectedCell;

    public int p1Points = 0;
    public int p2Points = 0;
    private readonly int numberOfDices = 4;
    public int remainingMoves;
    public Turn turn;
    public bool throwAgain;

    private void Start()
    {
        turn = Turn.PlayerOne;
    }

    public void ChangeTurn()
    {
        switch (turn)
        {
            case Turn.None:
                turn = Turn.PlayerOne;
                break;
            case Turn.PlayerOne:
                turn = Turn.PlayerTwo;
                break;
            case Turn.PlayerTwo:
                turn = Turn.PlayerOne;
                break;
        }
    }

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
            UpdateMovements(token);
        }
        if (token.hasToKillToken)
        {
            if (token.isPlayerOne)
            {
                token.cell.playerTwoToken.cell = playerTwoStart;
            }
            else
            {
                token.cell.playerOneToken.cell = playerOneStart;
            }
        }
        else if (!token.throwAgain)
        {
            ChangeTurn();
        }
    }

    private bool ThereAreMoreMoves()
    {
        return remainingMoves > 0;
    }

    private void Advance(Token token)
    {
        Cell cellToAdvance = NextCell(token);

        if (CanAdvance(token))
        {
            switch (cellToAdvance.type)
            {
                case CellType.Finish:
                    token.cell = cellToAdvance;
                    token.goalReached = true;
                    token.hasToJumpToken = false;
                    token.hasToKillToken = false;
                    token.throwAgain = false;
                    break;
                case CellType.Protection:
                    if (!CellIsEmpty(cellToAdvance))
                    {
                        token.cell = cellToAdvance;
                        token.goalReached = false;
                        token.hasToJumpToken = true;
                        token.hasToKillToken = false;
                    }
                    else
                    {
                        token.cell = cellToAdvance;
                        token.goalReached = false;
                        token.hasToJumpToken = false;
                        token.hasToKillToken = false;
                    }
                    token.throwAgain = true;
                    break;
                case CellType.Throw:
                    token.throwAgain = true;
                    break;
                case CellType.Normal:
                    if (CellIsEmpty(cellToAdvance))
                    {
                        token.cell = cellToAdvance;
                        token.goalReached = false;
                        token.hasToJumpToken = false;
                        token.hasToKillToken = false;
                    }
                    else if(ThereIsTeamToken(token, cellToAdvance))
                    {
                        token.cell = cellToAdvance;
                        token.goalReached = false;
                        if (IsLastMovement())
                        {
                            token.hasToJumpToken = true;
                        }
                        else
                        {
                            token.hasToJumpToken = false;
                        }
                        token.hasToKillToken = false;
                    }
                    else if(ThereIsEnemyToken(token, cellToAdvance))
                    {
                        token.cell = cellToAdvance;
                        token.goalReached = false;
                        token.hasToJumpToken = false;
                        if (IsLastMovement())
                        {
                            token.hasToKillToken = true;
                        }
                        else
                        {
                            token.hasToKillToken = false;
                        }
                    }
                    token.throwAgain = false;
                    break;
                case CellType.Start:
                    break;
                case CellType.None:
                default:
                    break;

            }
        }
    }

    public bool IsLastMovement()
    {
        return remainingMoves == 1;
    }

    public void UpdateMovements(Token token)
    {
        if (token.goalReached)
        {
            remainingMoves = 0;
        }
        else if (!token.hasToJumpToken)
        {
            remainingMoves--;
        }
    }

    public Cell NextCell(Token token)
    {
        if (token.isPlayerOne)
            return token.cell.nextCellPlayerOne;
        else
            return token.cell.nextCellPlayerTwo;
    }

    public bool CanAdvance(Token token)
    {
        return (token.isPlayerOne && token.cell.nextCellPlayerOne != null) || (!token.isPlayerOne && token.cell.nextCellPlayerTwo != null);
    }

    public bool CellIsEmpty(Cell cell)
    {
        return !cell.playerOneToken && !cell.playerTwoToken;
    }

    public bool ThereIsTeamToken(Token token, Cell cell)
    {
        bool thereIsToken = false;
        int pos = board.cells.IndexOf(cell);

        if (token.isPlayerOne)
        {
            if(pos != -1 && board.cells[pos].playerOneToken)
            {
                thereIsToken = true;
            }
        }
        else
        {
            if (pos != -1 && board.cells[pos].playerTwoToken)
            {
                thereIsToken = true;
            }
        }
        return thereIsToken;
    }

    public bool ThereIsEnemyToken(Token token, Cell cell)
    {
        bool thereIsToken = false;
        int pos = board.cells.IndexOf(cell);

        if (token.isPlayerOne)
        {
            if (pos != -1 && board.cells[pos].playerTwoToken)
            {
                thereIsToken = true;
            }
        }
        else
        {
            if (pos != -1 && board.cells[pos].playerOneToken)
            {
                thereIsToken = true;
            }
        }
        return thereIsToken;
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
