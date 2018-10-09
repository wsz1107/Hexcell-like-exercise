using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NonNullHexCellNoHint : MonoBehaviour
{
    Toggle toggle;
    HexMapEditor hexMapEditor;
    public int nonNullHintIndex;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        hexMapEditor = GetComponentInParent<HexMapEditor>();
    }
    private void Start()
    {
        toggle.onValueChanged.AddListener((bool value)=>hexMapEditor.AddNonNullHintLabel(nonNullHintIndex, value));
        toggle.onValueChanged.AddListener((bool value) => hexMapEditor.SetAllHints());
    }
}
