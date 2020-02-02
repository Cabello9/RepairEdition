using System.Collections.Generic;
using UnityEngine;

public enum CellType{None, Normal, Start, Protection, Finish, Throw}

public class Cell : MonoBehaviour
{
    public CellType type;

    public Cell nextCellPlayerOne;
    public Cell nextCellPlayerTwo;

    public Material playerOneMaterial;
    public Material playerTwoMaterial;
    public List<MeshRenderer> meshRenderers;

    [Header("Neighbors")]
    public Cell upCell;
    public Cell leftCell;
    public Cell downCell;
    public Cell rightCell;

    //public bool playerOneOn;
    public Token playerOneToken;
    //public bool playerTwoOn;
    public Token playerTwoToken;

    public void ChangeMaterial(Turn turn)
    {
        if (turn == Turn.PlayerOne)
        {
            foreach(MeshRenderer m in meshRenderers)
            {
                m.material = playerOneMaterial;
            }
        }
        else
        {
            foreach(MeshRenderer m in meshRenderers)
            {
                m.material = playerTwoMaterial;
            }
        }
    }
}
