using System.Collections;
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

        stringNameText.text = Data.StringName;
    }

    void OnBGButtonClicked()
    {
        if(Data.DataProvider != null)
            Data.DataProvider.Remove(Data);
    }
}
