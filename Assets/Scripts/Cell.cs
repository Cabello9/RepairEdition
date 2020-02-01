enum CellType{None, Start, Protection, Finnish}

public class Cell
{
    public Point position;
    public Cell nextCasillaPlayerOne;
    public Cell nextCasillaPlayerTwo;

    public bool playerOneOn;
    public bool playerTwoOn;
}
