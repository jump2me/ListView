﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExampleListItem : ListItem<ExampleListViewData>
{
    public Text stringNameText;
    public Button bgButton;

    protected override void Awake()
    {
        base.Awake();

        bgButton.onClick.AddListener(OnBGButtonClicked);
    }

    protected override void Invalidate()
    {
        base.Invalidate();

        //stringNameText.text = Data.StringName.Value;
        stringNameText.text = string.Format("{0},{1}", Data.Min, Data.Max);
    }

    void OnBGButtonClicked()
    {
        Data.StringName.Value = System.DateTime.Now.ToString();
    }
}
