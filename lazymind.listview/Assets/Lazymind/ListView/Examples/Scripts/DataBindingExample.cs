using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DataBindingExample : MonoBehaviour 
{
    public RectTransform listRectTransform;

    public ObservableList<ExampleListViewData> DataProvider { get; set; }
    public ListView<ExampleListViewData> listView { get; set; }

    public Button addButton;
    public Button removeButton;
    public Button insertButton;
    public Button clearButton;

    private void Start()
    {
        DataProvider = new ObservableList<ExampleListViewData>();
        listView = new ListView<ExampleListViewData>(listRectTransform, DataProvider, ViewHelper.Origin.Top);

        addButton.onClick.AddListener(OnAddButtonClicked);
        removeButton.onClick.AddListener(OnRemoveButtonClicked);
        insertButton.onClick.AddListener(OnInsertButtonClicked);
        clearButton.onClick.AddListener(OnClearButtonClicked);
    }

    void OnAddButtonClicked()
    {
        var prefab = Resources.Load<GameObject>("Prefab/ExampleListItem_Vertical");
        var id = DataProvider.Count;
        var data = new ExampleListViewData(id, prefab, DataProvider);
        data.StringName.Value = "data" + id;

        DataProvider.Add(data);
    }

    void OnInsertButtonClicked()
    {
        var prefab = Resources.Load<GameObject>("Prefab/ExampleListItem_Vertical");
        var id = DataProvider.Count;
        var data = new ExampleListViewData(id, prefab, DataProvider);
        data.StringName.Value = "data" + id;

        DataProvider.Insert(0, data);
    }

    void OnRemoveButtonClicked()
    {
        var data = DataProvider.LastOrDefault();
        if (data != null)
            DataProvider.Remove(data);
    }

    void OnClearButtonClicked()
    {
        DataProvider.Clear();
    }
}
