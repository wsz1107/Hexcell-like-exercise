using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HexCellHintDetailed : MonoBehaviour
{
    Toggle toggle;
    HexMapEditor hexMapEditor;
    public int index;
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        hexMapEditor = GetComponentInParent<HexMapEditor>();
    }
    private void Start()
    {
        toggle.onValueChanged.AddListener((bool value) => hexMapEditor.ChangeHintToDetailed(value, index));
    }
}
