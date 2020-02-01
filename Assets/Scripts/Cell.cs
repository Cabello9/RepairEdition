﻿using UnityEngine;

public enum CellType{None, Normal, Start, Protection, Finnish}

public class Cell
{
    public CellType type;

    public Cell nextCellPlayerOne;
    public Cell nextCellPlayerTwo;

    [Header("Neighbors")]
    public Cell upCell;
    public Cell leftCell;
    public Cell downCell;
    public Cell rightCell;

    public bool playerOneOn;
    public Token playerOneToken;
    public bool playerTwoOn;
    public Token playerTwoToken;
}
