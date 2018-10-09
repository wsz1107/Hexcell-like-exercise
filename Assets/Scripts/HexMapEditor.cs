using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class AllToggles
{
    public Toggle nullHexCellToggle;
    public Toggle nullNoHintToggle;
    public Toggle leftBelowHintToggle;
    public Toggle belowHintToggle;
    public Toggle rightBelowHintToggle;
    public Toggle nullDetailedHintToggle;

    public Toggle blueHexCellToggle;
    public Toggle blueInvisibleToggle;
    public Toggle blueNoHintToggle;

    public Toggle blackHexCellToggle;
    public Toggle blackInvisibleToggle;
    public Toggle blackNoHintToggle;
    public Toggle blackDetailedHintToggle;
}

public class HexMapEditor : MonoBehaviour
{
    public HexGrid hexGrid;

    public Color[] colors;
    private Color activeColor;
    public Color defaultColor;
    public Color defaultFrameColor;
    public Color touchedFrameColor;
    public Color invisibleColor;
    public GameObject[] panels;
    public AllToggles allToggles;

    private HexCell lastCell = null;
    private HexCell thisCell = null;

    string detailHint;
    string oldHint;
    bool status;
    

    ToggleGroup toggleGroup;
    /// <summary>
    /// At the beginning, roll up all the panels and keep the three toggles off until player changes it. 
    /// The two detailed hint toggles are disactive unless the "noHint" toggles are off. 
    /// </summary>
    private void Awake()
    {
        toggleGroup = GetComponentInChildren<ToggleGroup>();
        toggleGroup.SetAllTogglesOff();
        allToggles.nullDetailedHintToggle.interactable = false;
        allToggles.blackDetailedHintToggle.interactable = false;

        for (int i = 0; i < panels.Length; i++)
        {
            GameObject panel = panels[i];
            panel.SetActive(false);
        }
    }
    /// <summary>
    /// Detect the mouse click every frame. 
    /// </summary>
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput();
        }
    }
    /// <summary>
    /// Make a invisible ray between the point of mouse clicking and camera. 
    /// Find out the cell which has been clicked (by the method "GetCell" in HexGrid). 
    /// Then put the exact cell in the "EditCell" method".
    /// </summary>
    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            EditCell(hexGrid.GetCell(hit.point));
        }
    }
    /// <summary>
    /// This method means to determine 3 situation when player click a cell. 
    /// First, no other cell has been clicked before this click. 
    /// Second, click the cell which had been clicked to cancel it. 
    /// Third, a cell had been clicked and the player clicked another one. Cancel the last one and pick the new one. 
    /// </summary>
    /// <param name="cell"></param>
    void EditCell(HexCell cell)
    {
        if (!cell.touchedOrNot)
        {
            thisCell = cell;
            BindToToggle(cell);
            if (lastCell != null && lastCell.touchedOrNot)
            {
                lastCell.frameColor = defaultFrameColor;
                lastCell.touchedOrNot = false;
            }
            cell.frameColor = touchedFrameColor;
            hexGrid.Refresh();
            cell.touchedOrNot = true;
            lastCell = cell;
        }
        else
        {
            thisCell = null;
            lastCell = null;
            cell.frameColor = defaultFrameColor;
            hexGrid.Refresh();
            cell.touchedOrNot = false;
        }
    }
    
    /// <summary>
    /// Change the color of cells to show which kind of cell they are. Disacive all the hint texts at the same time. 
    /// </summary>
    public void SetColor(int index, bool value)
    {
        if (thisCell != null && thisCell == lastCell) 
        {
            if (value)
            {
                thisCell.color = colors[index];
                thisCell.hint.gameObject.SetActive(false);
                hexGrid.Refresh();
                switch (index)
                {
                    case 0:
                        thisCell.hexCellStatus.nullHexCell = true;
                        thisCell.hexCellStatus.blueHexCell = false;
                        thisCell.hexCellStatus.blackHexCell = false;
                        thisCell.cellValue = 0;
                        break;
                    case 1:
                        thisCell.hexCellStatus.blueHexCell = true;
                        thisCell.hexCellStatus.nullHexCell = false;
                        thisCell.hexCellStatus.blackHexCell = false;
                        thisCell.cellValue = 1;
                        break;
                    case 2:
                        thisCell.hexCellStatus.blackHexCell = true;
                        thisCell.hexCellStatus.nullHexCell = false;
                        thisCell.hexCellStatus.blueHexCell = false;
                        thisCell.cellValue = 0;
                        break;
                }
            }
            else
            {
                switch (index)
                {
                    case 0:
                        allToggles.nullNoHintToggle.isOn = true;
                        allToggles.leftBelowHintToggle.isOn = false;
                        allToggles.belowHintToggle.isOn = false;
                        allToggles.rightBelowHintToggle.isOn = false;
                        allToggles.nullDetailedHintToggle.isOn = false;
                        allToggles.nullDetailedHintToggle.interactable = false;
                        thisCell.hexCellStatus.nullHexCell = false;
                        break;
                    case 1:
                        allToggles.blueInvisibleToggle.isOn = false;
                        allToggles.blueNoHintToggle.isOn = true;
                        thisCell.hexCellStatus.blueHexCell = false;
                        break;
                    case 2:
                        allToggles.blackInvisibleToggle.isOn = false;
                        allToggles.blackNoHintToggle.isOn = true;
                        allToggles.blackDetailedHintToggle.isOn = false;
                        allToggles.blackDetailedHintToggle.interactable = false;
                        thisCell.hexCellStatus.blackHexCell = false;
                        break;
                }
            }
        }
    }
    /// <summary>
    /// The event of changing null hexcell's hint. 
    /// Receive the index from NullHexCellHintToggle. Set the localEulerAngles. Then active the Text gameobject. Finally, recount the number of hint. 
    /// The 'thisCell == lastCell' statement will distinguish the toggle clicked initiatively or passively. 
    /// </summary>
    /// <param name="index">index of null cell hint types</param>
    /// <param name="value"></param> 
    public void AddNullHintLabel(int index, bool value)
    {
        if (value)
        {
            thisCell.hint.color = new Color(0, 0, 0);
            if (thisCell != null && thisCell == lastCell) 
            {
                switch (index)
                {
                    case 0:
                        thisCell.hint.gameObject.SetActive(false);
                        thisCell.hexCellStatus.nullNoHint = true;
                        thisCell.hexCellStatus.leftBelowHint = false;
                        thisCell.hexCellStatus.belowHint = false;
                        thisCell.hexCellStatus.rightBelowHint = false;
                        allToggles.nullDetailedHintToggle.isOn = false;
                        allToggles.nullDetailedHintToggle.interactable = false;
                        break;
                    case 1:
                        thisCell.hint.transform.localEulerAngles = new Vector3(0, 0, -45);
                        thisCell.hint.gameObject.SetActive(true);
                        thisCell.hexCellStatus.nullNoHint = false;
                        thisCell.hexCellStatus.leftBelowHint = true;
                        thisCell.hexCellStatus.belowHint = false;
                        thisCell.hexCellStatus.rightBelowHint = false;
                        allToggles.nullDetailedHintToggle.interactable = true;
                        break;
                    case 2:
                        thisCell.hint.transform.localEulerAngles = new Vector3(0, 0, 0);
                        thisCell.hint.gameObject.SetActive(true);
                        thisCell.hexCellStatus.nullNoHint = false;
                        thisCell.hexCellStatus.leftBelowHint = false;
                        thisCell.hexCellStatus.belowHint = true;
                        thisCell.hexCellStatus.rightBelowHint = false;
                        allToggles.nullDetailedHintToggle.interactable = true;
                        break;
                    case 3:
                        thisCell.hint.transform.localEulerAngles = new Vector3(0, 0, 45);
                        thisCell.hint.gameObject.SetActive(true);
                        thisCell.hexCellStatus.nullNoHint = false;
                        thisCell.hexCellStatus.leftBelowHint = false;
                        thisCell.hexCellStatus.belowHint = false;
                        thisCell.hexCellStatus.rightBelowHint = true;
                        allToggles.nullDetailedHintToggle.interactable = true;
                        break;
                }
                SetHint(thisCell);
            }
        }
    }
    /// <summary>
    /// This event will change blue and black hexcell's hint when the blue/black hexcell's 'nohint' toggle is changed. 
    /// Receive the index from NonNullHexCellNoHintToggle. Then active the Text gameobject. Finally, recount the number of hint. 
    /// The 'thisCell == lastCell' statement will distinguish the toggle clicked initiatively or passively. 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="value"></param>
    public void AddNonNullHintLabel(int index, bool value)
    {
        if (thisCell != null && thisCell == lastCell)
        {
            if (index == 0)
            {
                if (!value)
                {
                    thisCell.hint.gameObject.SetActive(true);
                    thisCell.hint.color = new Color(255, 255, 255);
                    thisCell.hexCellStatus.blueNoHint = false;
                    SetHint(thisCell);
                }
                else
                {
                    thisCell.hint.gameObject.SetActive(false);
                    thisCell.hexCellStatus.blueNoHint = true;
                }
            }
            if (index == 1)
            {
                if (!value)
                {
                    thisCell.hint.gameObject.SetActive(true);
                    allToggles.blackDetailedHintToggle.interactable = true;
                    thisCell.hint.color = new Color(255, 255, 255);
                    thisCell.hexCellStatus.blackNoHint = false;
                    SetHint(thisCell);
                }
                else
                {
                    thisCell.hint.gameObject.SetActive(false);
                    allToggles.blackDetailedHintToggle.isOn = false;
                    allToggles.blackDetailedHintToggle.interactable = false;
                    thisCell.hexCellStatus.blackNoHint = true;
                }
            }
        }
    }
    /// <summary>
    /// This event will change a blue/black hexcell into an invisible one when the invisible toggle is changed. 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="index"></param>
    public void SetVisible(bool value, int index)
    {
        if (thisCell != null && thisCell == lastCell)
        {
            if (value)
            {
                thisCell.color = invisibleColor;
                hexGrid.Refresh();
                if (index == 1)
                    thisCell.hexCellStatus.blueInvisible = true;
                else if(index==2)
                    thisCell.hexCellStatus.blackInvisible = true;
            }
            else
            {
                if (thisCell.hexCellStatus.blueInvisible && thisCell.hexCellStatus.blackInvisible)
                {
                    thisCell.color = colors[index];
                    hexGrid.Refresh();
                    if (index == 1)
                        thisCell.hexCellStatus.blueInvisible = false;
                    else if (index == 2)
                        thisCell.hexCellStatus.blackInvisible = false;
                }
            }
        }
    }
    /// <summary>
    /// This method will change the toggles on the MapEditor depending on the status of the cell, which is clicked.  
    /// </summary>
    /// <param name="cell"></param>
    void BindToToggle(HexCell cell)
    {
        if (cell.hexCellStatus.nullHexCell) allToggles.nullHexCellToggle.isOn = true;
        else allToggles.nullHexCellToggle.isOn = false;
        if (cell.hexCellStatus.nullNoHint) allToggles.nullNoHintToggle.isOn = true;
        else allToggles.nullNoHintToggle.isOn = false;
        if (cell.hexCellStatus.leftBelowHint) allToggles.leftBelowHintToggle.isOn = true;
        else allToggles.leftBelowHintToggle.isOn = false;
        if (cell.hexCellStatus.belowHint) allToggles.belowHintToggle.isOn = true;
        else allToggles.belowHintToggle.isOn = false;
        if (cell.hexCellStatus.rightBelowHint) allToggles.rightBelowHintToggle.isOn = true;
        else allToggles.rightBelowHintToggle.isOn = false;
        if (cell.hexCellStatus.nullDetailedHint) allToggles.nullDetailedHintToggle.isOn = true;
        else allToggles.nullDetailedHintToggle.isOn = false;

        if (cell.hexCellStatus.blueHexCell) allToggles.blueHexCellToggle.isOn = true;
        else allToggles.blueHexCellToggle.isOn = false;
        if (cell.hexCellStatus.blueInvisible) allToggles.blueInvisibleToggle.isOn = true;
        else allToggles.blueInvisibleToggle.isOn = false;
        if (cell.hexCellStatus.blueNoHint) allToggles.blueNoHintToggle.isOn = true;
        else allToggles.blueNoHintToggle.isOn = false;

        if (cell.hexCellStatus.blackHexCell) allToggles.blackHexCellToggle.isOn = true;
        else allToggles.blackHexCellToggle.isOn = false;
        if (cell.hexCellStatus.blackInvisible) allToggles.blackInvisibleToggle.isOn = true;
        else allToggles.blackInvisibleToggle.isOn = false;
        if (cell.hexCellStatus.blackNoHint) allToggles.blackNoHintToggle.isOn = true;
        else allToggles.blackNoHintToggle.isOn = false;
        if (cell.hexCellStatus.blackDetailedHint) allToggles.blackDetailedHintToggle.isOn = true;
        else allToggles.blackDetailedHintToggle.isOn = false;
    }
    /// <summary>
    /// This event will change null/black hexcell's hint from simple to detailed. 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="value"></param>
    /// 
    public void ChangeHintToDetailed(bool value, int index)
    {
        if (thisCell != null && thisCell == lastCell)
        {
            if (index == 0)
            {
                if (value)
                {
                    oldHint = thisCell.hint.text;
                    thisCell.hint.text = thisCell.nullHintDetailed; ;
                    thisCell.hexCellStatus.nullDetailedHint = true;

                }
                else
                {
                    thisCell.hint.text = oldHint;
                    thisCell.hexCellStatus.nullDetailedHint = false;
                }
            }
            else
            {
                if (value)
                {
                    oldHint = thisCell.hint.text;
                    thisCell.hint.text = thisCell.blackHintDetailed;
                    thisCell.hexCellStatus.blackDetailedHint = true;

                }
                else
                {
                    thisCell.hint.text = oldHint;
                    thisCell.hexCellStatus.blackDetailedHint = false;
                }
            }
        }
    }
    /// <summary>
    /// These two methods will check the status of the hexcell and invoke the method to count the number of hint.
    /// </summary>
    /// <param name="cell"></param>
    void SetHint(HexCell cell)
    {
        if (cell.hexCellStatus.nullHexCell == true)
        {
            if (cell.hexCellStatus.leftBelowHint == true)
            {
                hexGrid.CountNullCellsHint(cell, 1);
            }
            if (cell.hexCellStatus.belowHint == true)
            {
                hexGrid.CountNullCellsHint(cell, 2);
            }
            if (cell.hexCellStatus.rightBelowHint == true)
            {
                hexGrid.CountNullCellsHint(cell, 3);
            }
        }
        if (cell.hexCellStatus.blueHexCell == true)
        {
            hexGrid.CountBlueCellsHint(cell);
        }
        if (cell.hexCellStatus.blackHexCell == true)
        {
            hexGrid.CountBlackCellsHint(cell);
        }
    }
    public void SetAllHints()
    {
        for (int i = 0; i < hexGrid.cells.Length; i++)
        {
            SetHint(hexGrid.cells[i]);
        }
    }
    public void DefaultSubToggles()
    {
        allToggles.nullNoHintToggle.isOn = true;
        allToggles.leftBelowHintToggle.isOn = false;
        allToggles.belowHintToggle.isOn = false;
        allToggles.rightBelowHintToggle.isOn = false;
        allToggles.nullDetailedHintToggle.isOn = false;
        allToggles.nullDetailedHintToggle.interactable = false;

        allToggles.blueInvisibleToggle.isOn = false;
        allToggles.blueNoHintToggle.isOn = true;

        allToggles.blackInvisibleToggle.isOn = false;
        allToggles.blackNoHintToggle.isOn = true;
        allToggles.blackDetailedHintToggle.isOn = false;
        allToggles.blackDetailedHintToggle.interactable = false;
    }
    public void Save()
    {
        string path = Path.Combine(Application.persistentDataPath, "test.map");
        using (
            BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create))
            )
        {
            hexGrid.Save(writer);
        }
    }
    public void Load()
    {
        string path = Path.Combine(Application.persistentDataPath, "test.map");
        using (
            BinaryReader reader = new BinaryReader(File.OpenRead(path))
            ) { hexGrid.Load(reader); }

    }
    public void SetHexCellTypeIndex(int index)
    {
         
    }
}
