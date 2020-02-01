using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Turn { None, PlayerOne, PlayerTwo}

public class Game : Singleton<Game>
{
    [Header("Game components")]
    public List<Cell> board;
    public List<Token> playerOneTokens;
    public List<Token> playerTwoTokens;

    public Cell playerOneStart;
    public Cell playerTwoStart;
    public Cell selectedCell;

    public int p1Points = 0;
    public int p2Points = 0;
    public List<Dice> dices;
    public int remainingMoves;
    public Turn turn;
    public bool throwAgain;

    public Cell defaultCell;
    public bool canThrowDices;

    private void Start()
    {
        turn = Turn.PlayerOne;
        canThrowDices = true;
    }

    public void ChangeTurn()
    {
        canThrowDices = true;
        ResetDices();
        LightOffDefaultCell();
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

        selectedCell = defaultCell;
    }

    public void ResetDices()
    {
        foreach (var dice in dices)
        {
            dice.currentValue = -1;
            dice.animator.Rebind();
            dice.ResetPosition();
        }
    }

    public int ThrowDices()
    {
        int result = 0;

        foreach (var dice in dices)
        {
            dice.ThrowDice();
            dice.RollAnimation();
            result += dice.GetValue();
        }

        return result;
    }

    public void Move(Token token)
    {
        Cell lastCell = token.cell;
        
        while (ThereAreMoreMoves())
        {
            Advance(token);
            UpdateMovements(token);
            
        }

        if (token.isPlayerOne)
        {
            lastCell.playerOneToken = null;
            token.cell.playerOneToken = token;
        }
        else
        {
            lastCell.playerTwoToken = null;
            token.cell.playerTwoToken = token;
        }

        if (token.hasToKillToken)
        {
            if (token.isPlayerOne)
            {
                token.cell.playerTwoToken.cell = playerTwoStart;
                StartCoroutine(CrushOpponent(token, token.cell.playerTwoToken, token.cell.transform.GetChild(0)));
                token.cell.playerTwoToken = null;
            }
            else
            {
                token.cell.playerOneToken.cell = playerOneStart;
                StartCoroutine(CrushOpponent(token, token.cell.playerOneToken, token.cell.transform.GetChild(0)));
                token.cell.playerOneToken = null;
            }
        }
        
        if (!throwAgain)
        {
            token.JumpToPosition(token.cell.transform.GetChild(0).position, 0.5f);
            
            if (token.goalReached)
            {
                token.gameObject.SetActive(false);
            }

            if (p1Points == 7)
            {
                //Ha ganado alguien
                Debug.Log("Jugador 1 ha ganado");
            }
            else if (p2Points == 7)
            {
               Debug.Log("Jugador 2 ha ganado"); 
            }
            else
            {
                StartCoroutine(ChangeTurnCoroutine(token));
            }
        }
        else
        {
            LightOffDefaultCell();
            token.JumpToPosition(token.cell.transform.GetChild(0).position, 0.5f);
            GameVisualManager.Instance.RollAgain();
            canThrowDices = true;
            
        }
    }

    public void IlluminateDefaultCell()
    {
        selectedCell = defaultCell;
        selectedCell.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void LightOffDefaultCell()
    {
        selectedCell.transform.GetChild(1).gameObject.SetActive(false);
    }

    IEnumerator ChangeTurnCoroutine(Token token)
    {
        token.JumpToPosition(token.cell.transform.GetChild(0).position, 0.5f);
        yield return new WaitForSeconds(0.5f);
        ChangeTurn();

        if (turn == Turn.PlayerOne)
        {
            GameVisualManager.Instance.PlayerTurn(true);
        }
        else if (turn == Turn.PlayerTwo)
        {
            GameVisualManager.Instance.PlayerTurn(false);
        }
    }
    
    IEnumerator CrushOpponent(Token murderer, Token victim, Transform target)
    {
        murderer.Kill(target.position, 0.5f);
        yield return new WaitForSeconds(0.4f);
        victim.CrushYAxis(0.1f, 0.2f);
        yield return new WaitForSeconds(0.3f);
        victim.gameObject.SetActive(false);
        victim.RestoreScale();
        victim.ResetAll();
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
                    throwAgain = false;
                    break;
                case CellType.Protection:
                    if (!CellIsEmpty(cellToAdvance))
                    {
                        token.cell = cellToAdvance;
                        token.goalReached = false;
                        if (IsLastMovement())
                            token.hasToJumpToken = true;
                        else
                            token.hasToJumpToken = false;
                        token.hasToKillToken = false;
                    }
                    else
                    {
                        token.cell = cellToAdvance;
                        token.goalReached = false;
                        token.hasToJumpToken = false;
                        token.hasToKillToken = false;
                    }
                    throwAgain = false;
                    break;
                case CellType.Throw:
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
                    throwAgain = true;
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
                    throwAgain = false;
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
            if (token.isPlayerOne)
                p1Points++;
            else
                p2Points++;
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
        int pos = board.IndexOf(cell);

        if (token.isPlayerOne)
        {
            if(pos != -1 && board[pos].playerOneToken)
            {
                thereIsToken = true;
            }
        }
        else
        {
            if (pos != -1 && board[pos].playerTwoToken)
            {
                thereIsToken = true;
            }
        }
        return thereIsToken;
    }

    public bool ThereIsEnemyToken(Token token, Cell cell)
    {
        bool thereIsToken = false;
        int pos = board.IndexOf(cell);

        if (token.isPlayerOne)
        {
            if (pos != -1 && board[pos].playerTwoToken)
            {
                thereIsToken = true;
            }
        }
        else
        {
            if (pos != -1 && board[pos].playerOneToken)
            {
                thereIsToken = true;
            }
        }
        return thereIsToken;
    }

    public void SelectUpCell()
    {
        if (selectedCell.upCell != null)
        {
            selectedCell.transform.GetChild(1).gameObject.SetActive(false);
            selectedCell = selectedCell.upCell;
            selectedCell.transform.GetChild(1).gameObject.SetActive(true);
        }
            
    }

    public void SelectLeftCell()
    {
        if (selectedCell.leftCell != null)
        {
            selectedCell.transform.GetChild(1).gameObject.SetActive(false);
            selectedCell = selectedCell.leftCell;
            selectedCell.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    public void SelectDownCell()
    {
        if (selectedCell.downCell != null)
        {
            selectedCell.transform.GetChild(1).gameObject.SetActive(false);
            selectedCell = selectedCell.downCell;
            selectedCell.transform.GetChild(1).gameObject.SetActive(true);
        }
            
    }
    public void SelectRightCell()
    {
        if (selectedCell.rightCell != null)
        {
            selectedCell.transform.GetChild(1).gameObject.SetActive(false);
            selectedCell = selectedCell.rightCell;
            selectedCell.transform.GetChild(1).gameObject.SetActive(true);
        }
        
    }
    
    private bool CheckTokenInCell(Turn turn, Cell cell)
    {
        if (cell != null)
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

    public void SpawnNewToken()
    {
        if (turn == Turn.PlayerOne)
        {
            foreach (var playerOneToken in playerOneTokens)
            {
                if (playerOneToken.cell != null && playerOneToken.cell.type == CellType.Start)
                {
                    StartCoroutine(SpawnTokenCoroutine(playerOneToken, true));
                    break;
                }
                    

            }
        }
        else if (turn == Turn.PlayerTwo)
        {
            foreach (var playerTwoToken in playerTwoTokens)
            {
                if (playerTwoToken.cell != null && playerTwoToken.cell.type == CellType.Start)
                {
                    StartCoroutine(SpawnTokenCoroutine(playerTwoToken, false));
                    break;
                }
                    
                    
            }
        }
    }

    public void SpawnToken(Token token, bool isPlayerOne)
    {
        StartCoroutine(SpawnTokenCoroutine(token,isPlayerOne));
    }

    IEnumerator SpawnTokenCoroutine(Token token, bool isPlayerOne)
    {
        if (isPlayerOne)
        {
            token.transform.position = playerOneStart.transform.GetChild(0).position;
            token.transform.rotation = playerOneStart.transform.GetChild(0).rotation;
        }
        else
        {
            token.transform.position = playerTwoStart.transform.GetChild(0).position;
            token.transform.rotation = playerTwoStart.transform.GetChild(0).rotation;
        }
        
        token.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        Move(token);
    }
}
