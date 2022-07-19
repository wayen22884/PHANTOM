using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboModify : MonoBehaviour
{
    public Combo combo;
    public Button Add;
    public Button Sub;

    void Start()
    {
        this.Add.onClick.AddListener(() => this.combo.Add(1));
        this.Sub.onClick.AddListener(() => this.combo.Add(-1));
    }
}
