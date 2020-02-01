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
    private List<Dice> dices;
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

        foreach (var dice in dices)
        {
            dice.ThrowDice();
            result += dice.GetValue();
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
                StartCoroutine(CrushOpponent(token, token.cell.playerTwoToken, token.cell.transform.GetChild(0)));
            }
            else
            {
                token.cell.playerOneToken.cell = playerOneStart;
                StartCoroutine(CrushOpponent(token, token.cell.playerOneToken, token.cell.transform.GetChild(0)));
            }
        }
        else if (!token.throwAgain)
        {
            StartCoroutine(ChangeTurnCoroutine(token));
        }
    }

    IEnumerator ChangeTurnCoroutine(Token token)
    {
        token.JumpToPosition(token.cell.transform.GetChild(0).position, 0.5f);
        yield return new WaitForSeconds(0.5f);
        ChangeTurn();
    }
    
    IEnumerator CrushOpponent(Token murderer, Token victim, Transform target)
    {
        murderer.Kill(target.position, 0.5f);
        yield return new WaitForSeconds(0.4f);
        victim.CrushYAxis(0.1f, 0.2f);
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
    
    private bool CheckTokenInCell(Turn turn, Cell cell)
    {
        if (turn == Turn.PlayerOne)
        {
            if (cell.playerOneToken != null)
                return true;
            
            return false;
        }

        if(turn == Turn.PlayerTwo)
        {
            if (cell.playerTwoToken != null)
                return true;
            
            return false;
        }

        return false;
    }

    public void SelectCell()
    {
        if (CheckTokenInCell(turn, selectedCell))
        {
            if (turn == Turn.PlayerOne)
            {
                Move(selectedCell.playerOneToken);
            }
            else if (turn == Turn.PlayerTwo)
            {
                Move(selectedCell.playerTwoToken);
            }
        }
    }
}
