using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public List<Cell> cells;

    public Cell UpCell(Cell currentCell)
    {
        return currentCell.upCell;
    }

    public Cell LeftCell(Cell currentCell)
    {
        return currentCell.leftCell;
    }

    public Cell DownCell(Cell currentCell)
    {
        return currentCell.downCell;
    }

    public Cell RightCell(Cell currentCell)
    {
        return currentCell.rightCell;
    }

}