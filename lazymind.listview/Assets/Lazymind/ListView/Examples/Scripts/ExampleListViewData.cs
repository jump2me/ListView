using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleListViewData : ListViewData 
{
    public BindProperty<string> StringName { get; set; }
    public ExampleListViewData(int _id, ObservableList<ExampleListViewData> _dataProvider, string _prefabName = "") : base(_id, _prefabName)
    {
        StringName = new BindProperty<string>(OnNotifyChange);
        DataProvider = _dataProvider;
    }

    public ObservableList<ExampleListViewData> DataProvider { get; private set; }
}
