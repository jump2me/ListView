using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lazymind;

public class ComponentExample : MonoBehaviour {

    public RectTransform list;
    public ObservableList<ExampleListViewData> DataProvider = new ObservableList<ExampleListViewData>();
    public ListView<ExampleListViewData> listView;
    public GameObject prefab;
	void Start () {
        
        listView = new ListView<ExampleListViewData>(list, DataProvider, ViewHelper.Origin.Bottom, prefab);

        for (int i = 0, max = 10; i < max; i++)
        {
            var data = new ExampleListViewData(i, DataProvider);
            data.StringName.Value = "test item no." + i;

            DataProvider.Add(data);
        }
	}
}
