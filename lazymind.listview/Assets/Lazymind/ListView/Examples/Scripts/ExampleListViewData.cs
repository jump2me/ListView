using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleListViewData : ListViewData 
{
    public BindProperty<string> StringName { get; set; }
    public ExampleListViewData(int _id, GameObject _prefab, ObservableList<ExampleListViewData> _dataProvider) : base(_id, _prefab)
    {
        StringName = new BindProperty<string>(OnNotifyChange);
        DataProvider = _dataProvider;
    }

    public ObservableList<ExampleListViewData> DataProvider { get; private set; }

    public override string ToString()
    {
        return string.Format("[ExampleListViewData: StringName={0}, DataProvider={1}]", StringName, DataProvider);
    }
}
