using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class HexGrid : MonoBehaviour
{
    public int width = 7;
    public int height = 7;
    public HexCell cellPrefab;

    public Text cellCoordinatesLabelPrefab;
    public Text hintLabelPrefab;

    public Color defaultColor = Color.white;
    public Color defaultFrameColor = Color.white;
    public Color touchedFrameColor = Color.black;

    int count = 0;

    List<HexCell> targetCells;

    HexMesh hexMesh;
    FrameMesh frameMesh;
    Canvas gridCanvas;
    public HexCell[] cells;
    private void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();
        frameMesh = GetComponentInChildren<FrameMesh>();
        targetCells = new List<HexCell>();
        cells = new HexCell[height * width];
        for (int x=0, i=0; x<width;x++)
        {
            for(int z=0;z<height;z++)
            {
                CreatCell(x, z, i++);
            }
        }
    }
    private void Start()
    {
        hexMesh.Triangulate(cells);
        frameMesh.FrameHighLight(cells);
    }
    public HexCell GetCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        //Debug.Log("touched at" + position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        //Debug.Log("exact position" + coordinates.ToString());
        int index = coordinates.Z + coordinates.X * height + coordinates.X / 2;
        return cells[index];
    }
    public void Refresh()
    {
        frameMesh.FrameHighLight(cells);
        hexMesh.Triangulate(cells);
    }
    public void ClearAllFrame()
    {
        frameMesh.ClearAllFrame(cells);
    }
    void CreatCell(int x,int z,int i)
    {
        Vector3 position;
        position.x = x * (HexMetrics.outerRadius * 1.5f);
        position.y = 0.0f;
        position.z = (z + 0.5f * x - x / 2) * (HexMetrics.innerRadius * 2f);
        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;
        cell.frameColor = defaultFrameColor;
        CreatLabels(cell);

        cell.hint.gameObject.SetActive(false);
        if (z > 0)
        {
            cell.SetNeighbor(HexDirection.S, cells[i - 1]);
        }
        if (x > 0)
        {
            if((x & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.NW, cells[i - height]);
                if (z > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - height - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - height]);
                if (z < height - 1)
                {
                    cell.SetNeighbor(HexDirection.NW, cells[i - height + 1]);
                }
            }
        }
    }
    void CreatLabels(HexCell cell)
    {

        //Text coordinates = Instantiate<Text>(cellCoordinatesLabelPrefab);
        //coordinates.rectTransform.SetParent(gridCanvas.transform, false);
        //coordinates.rectTransform.anchoredPosition = new Vector2(cell.transform.position.x, cell.transform.position.z);
        //coordinates.text = cell.coordinates.ToStringOnSeperateLines();

        Text hintLabel = Instantiate<Text>(hintLabelPrefab);
        hintLabel.rectTransform.SetParent(gridCanvas.transform, false);
        hintLabel.rectTransform.anchoredPosition = new Vector2(cell.transform.position.x, cell.transform.position.z);
        hintLabel.text = "";

        cell.hint = hintLabel;
    }
    public void CountNullCellsHint(HexCell cell, int index)
    {
        bool flag = true;
        count = 0;
        targetCells.Clear();
        switch (index)
        {
            case 1:
                for (int i = 0; i < cells.Length; i++)
                {
                    if (cells[i].coordinates.Z == cell.coordinates.Z && cells[i].coordinates.X < cell.coordinates.X)
                    {
                        if (cells[i].cellValue != 0)
                        {
                            targetCells.Add(cells[i]);
                        }
                    }
                }
                for (int i = 1; i < targetCells.Count; i++)
                {
                    if ((targetCells[i].coordinates.X - targetCells[i - 1].coordinates.X) != 1)
                    {
                        flag = false;
                        break;
                    }
                }
                break;
            case 2:
                for (int i = 0; i < cells.Length; i++)
                {
                    if (cells[i].coordinates.X == cell.coordinates.X && cells[i].coordinates.Z < cell.coordinates.Z)
                    {
                        if (cells[i].cellValue != 0)
                        {
                            targetCells.Add(cells[i]);
                        }
                    }
                }
                for (int i = 1; i < targetCells.Count; i++)
                {
                    if ((targetCells[i].coordinates.Z - targetCells[i - 1].coordinates.Z) != 1)
                    {
                        flag = false;
                        break;
                    }
                }
                break;
            case 3:
                for (int i = 0; i < cells.Length; i++)
                {
                    if (cells[i].coordinates.Y == cell.coordinates.Y && cells[i].coordinates.Z < cell.coordinates.Z)
                    {
                        if (cells[i].cellValue != 0)
                        {
                            targetCells.Add(cells[i]);
                        }
                    }
                }
                for (int i = 1; i < targetCells.Count; i++)
                {
                    if ((targetCells[i].coordinates.Z - targetCells[i - 1].coordinates.Z) != 1)
                    {
                        flag = false;
                        break;
                    }
                }
                break;
        }
        for (int i = 0; i < targetCells.Count; i++)
        {
            count += targetCells[i].cellValue;
        }
        if (targetCells.Count > 1)
        {
            if (flag)
            {
                cell.nullHintDetailed = "{" + count.ToString() + "}";
            }
            else
            {
                cell.nullHintDetailed = "-" + count.ToString() + "-";
            }
        }
        else
        {
            flag = true;
            cell.nullHintDetailed = count.ToString();
        }
        if (cell.hexCellStatus.nullDetailedHint == false) cell.hint.text = count.ToString();
        else cell.hint.text = cell.nullHintDetailed;

    }
    public void CountBlueCellsHint(HexCell cell)
    {
        count = 0;
        targetCells.Clear();
        targetCells = new List<HexCell>();
        for(int i = 0; i < cell.neighbors.Length; i++)
        {
            if (cell.neighbors[i] != null)
            {
                targetCells.Add(cell.neighbors[i]);
                if (cell.neighbors[i].neighbors[i] != null)
                {
                    targetCells.Add(cell.neighbors[i].neighbors[i]);
                    if (i< cell.neighbors.Length - 1)
                    {
                        if(cell.neighbors[i].neighbors[i + 1] != null)
                            targetCells.Add(cell.neighbors[i].neighbors[i + 1]);
                    }
                    else targetCells.Add(cell.neighbors[i].neighbors[0]);
                }
            }
        }
        for(int i = 0; i < targetCells.Count; i++)
        {
            if (targetCells[i] != null)
                count += targetCells[i].cellValue;
        }
        cell.hint.text = count.ToString();
    }
    public void CountBlackCellsHint(HexCell cell)
    {
        count = 0;
        targetCells.Clear();
        targetCells = new List<HexCell>(cell.neighbors);
        int maxBlue = 0;
        int tempBlue = 0;
        List<HexCell> tempCells = new List<HexCell>(targetCells);
        bool flag = true;
        for (int i = 0; i < targetCells.Count; i++)
        {
            tempCells.Add(targetCells[i]);
            if (targetCells[i] != null)
            {
                count += targetCells[i].cellValue;
            }
        }
        for(int i = 0; i < tempCells.Count; i++)
        {
            if (tempCells[i] != null && tempCells[i].cellValue != 0)
            {
                tempBlue++;
            }
            else
            {
                if (tempBlue > maxBlue) maxBlue = tempBlue;
                tempBlue = 0;
            }
        }
        if (maxBlue != count) flag = false;
        if (count > 1 && count < 5)
        {
            if (flag)
            {
                cell.blackHintDetailed = "{" + count.ToString() + "}";
            }
            else
            {
                cell.blackHintDetailed = "-" + count.ToString() + "-";
            }
        }
        else
        {
            cell.blackHintDetailed = count.ToString();
        }

        if (cell.hexCellStatus.blackDetailedHint == false) cell.hint.text = count.ToString();
        else cell.hint.text = cell.blackHintDetailed;
    }
    public void Save(BinaryWriter writer)
    {
        for(int i = 0; i < cells.Length; i++)
        {
            cells[i].Save(writer);
        }
    }
    public void Load(BinaryReader reader)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Load(reader);
        }
    }
}
