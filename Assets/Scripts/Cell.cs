enum CellType{None, Normal, Start, Protection, Finnish}

public class Cell
{
    public Point positionPlayerOne;
    public Point positionPlayerTwo;
    public Cell nextCellPlayerOne;
    public Cell nextCellPlayerTwo;

    public Cell upCell;
    public Cell leftCell;
    public Cell downCell;
    public Cell rightCell;

    public bool playerOneOn;
    public bool playerTwoOn;
}
