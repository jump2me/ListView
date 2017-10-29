using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExampleListItem : ListItem<ExampleListViewData>
{
    public Text stringNameText;

    public override void Invalidate()
    {
        base.Invalidate();

        stringNameText.text = Data.StringName;
    }
}
