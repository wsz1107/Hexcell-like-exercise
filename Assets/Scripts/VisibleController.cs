using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VisibleController : MonoBehaviour
{
    Toggle toggle;
    HexMapEditor hexMapEditor;
    public int visibleControllerIndex;
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        hexMapEditor = GetComponentInParent<HexMapEditor>();
        toggle.isOn = false;
    }
    private void Start()
    {
        toggle.onValueChanged.AddListener((bool value) => hexMapEditor.SetVisible(value,visibleControllerIndex));
    }
}
