using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoad : MonoBehaviour
{
    public Button saveButton;
    public Button loadButton;
    HexMapEditor hexMapEditor;
    private void Awake()
    {
        //saveButton = GetComponent<Button>();
        //loadButton = GetComponent<Button>();
        hexMapEditor = GetComponentInParent<HexMapEditor>();
    }
    private void Start()
    {
        saveButton.onClick.AddListener(hexMapEditor.Save);
        loadButton.onClick.AddListener(hexMapEditor.Load);
    }
}
