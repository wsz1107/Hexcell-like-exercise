using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class HexCellStatus
{
    public bool nullHexCell;
    public bool nullNoHint;
    public bool leftBelowHint;
    public bool belowHint;
    public bool rightBelowHint;
    public bool nullDetailedHint;

    public bool blueHexCell;
    public bool blueInvisible;
    public bool blueNoHint;

    public bool blackHexCell;
    public bool blackInvisible;
    public bool blackNoHint;
    public bool blackDetailedHint;
}


public class HexCell : MonoBehaviour
{
    [SerializeField]
    public HexCell[] neighbors;
    public HexCoordinates coordinates;
    public HexCellStatus hexCellStatus;

    public Color frameColor;
    public Color defaultFrameColor;
    public Color color;

    //public Text hexcellCoordinates;
    public Text hint;

    public string nullHintDetailed;
    public string blackHintDetailed;

    //public bool consecutive;

    public bool touchedOrNot;

    public int cellValue;

    private void Awake()
    {
        touchedOrNot = false;

        cellValue = 0;

        hexCellStatus.nullHexCell = false;
        hexCellStatus.nullNoHint = false;
        hexCellStatus.leftBelowHint = false;
        hexCellStatus.belowHint = false;
        hexCellStatus.rightBelowHint = false;
        hexCellStatus.nullDetailedHint = false;

        hexCellStatus.blueHexCell = false;
        hexCellStatus.blueInvisible = false;
        hexCellStatus.blueNoHint = true;

        hexCellStatus.blackHexCell = false;
        hexCellStatus.blackInvisible = false;
        hexCellStatus.blackNoHint = true;
        hexCellStatus.blackDetailedHint = false;
    }
    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }
    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }
    public void Save(BinaryWriter writer)
    {
        //writer.Write()
    }
    public void Load(BinaryReader reader)
    {

    }

}
