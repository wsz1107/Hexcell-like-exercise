using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NullHexCellHintTogglesController : MonoBehaviour
{
    Toggle toggle;
    HexMapEditor hexMapEditor;
    public int nullHintIndex;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        hexMapEditor = GetComponentInParent<HexMapEditor>();
    }
    private void Start()
    {
        toggle.onValueChanged.AddListener((bool value) => hexMapEditor.AddNullHintLabel(nullHintIndex ,value));
    }
}
