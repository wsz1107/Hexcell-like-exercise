using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    Toggle toggle;
    public GameObject panel;
    public int colorIndex;
    HexMapEditor hexMapEditor;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        hexMapEditor = GetComponentInParent<HexMapEditor>();
    }
    private void Start()
    {
        toggle.onValueChanged.AddListener((bool value) => ActivePanel(panel, value));
        toggle.onValueChanged.AddListener((bool value) => hexMapEditor.SetColor(colorIndex, value));
        toggle.onValueChanged.AddListener((bool value) => hexMapEditor.SetAllHints());
    }
    void ActivePanel(GameObject panel,bool value)
    {
        if (value) panel.SetActive(true);
        else
            panel.SetActive(false);
    }
}
