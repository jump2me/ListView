using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour 
{
    public ObservableList<ExampleListViewData> topDataProvider { get; set; }
    public RectTransform topRectTransform;
    public ListView<ExampleListViewData> topList { get; set; }

    public ObservableList<ExampleListViewData> bottomDataProvider { get; set; }
    public RectTransform bottomRectTransform;
    public ListView<ExampleListViewData> bottomList { get; set; }

    public ObservableList<ExampleListViewData> leftDataProvider { get; set; }
    public RectTransform leftRectTransform;
    public ListView<ExampleListViewData> leftList { get; set; }

    public ObservableList<ExampleListViewData> rightDataProvider { get; set; }
    public RectTransform rightRectTransform;
    public ListView<ExampleListViewData> rightList { get; set; }

    void Start()
    {
        var verticalPrefab = Resources.Load<GameObject>("Prefab/ExampleListItem_Vertical");
        var horizontalPrefab = Resources.Load<GameObject>("Prefab/ExampleListItem_Horizontal");

        // top
        topDataProvider = new ObservableList<ExampleListViewData>();
        topList = new ListView<ExampleListViewData>(topRectTransform, topDataProvider, ViewHelper.Origin.Top, verticalPrefab);
        topDataProvider.Clear(false);

        for (int i = 0, max = 100; i < max; i++)
        {
            var data = new ExampleListViewData(i, topDataProvider);
            data.StringName.Value = "top : " + i;

            topDataProvider.Add(data, i == max - 1);
        }

        // bottom
        bottomDataProvider = new ObservableList<ExampleListViewData>();
        bottomList = new ListView<ExampleListViewData>(bottomRectTransform, bottomDataProvider, ViewHelper.Origin.Bottom, verticalPrefab);
        bottomDataProvider.Clear(false);

        for (int i = 0, max = 100; i < max; i++)
        {
            var data = new ExampleListViewData(i, bottomDataProvider);
            data.StringName.Value = "bottom : " + i;

            bottomDataProvider.Add(data, i == max - 1);
        }

        // left
        leftDataProvider = new ObservableList<ExampleListViewData>();
        leftList = new ListView<ExampleListViewData>(leftRectTransform, leftDataProvider, ViewHelper.Origin.Left, horizontalPrefab);
        leftDataProvider.Clear(false);

        for (int i = 0, max = 100; i < max; i++)
        {
            var data = new ExampleListViewData(i, leftDataProvider);
            data.StringName.Value = "left : " + i;

            leftDataProvider.Add(data, i == max - 1);
        }


        // right
        rightDataProvider = new ObservableList<ExampleListViewData>();
        rightList = new ListView<ExampleListViewData>(rightRectTransform, rightDataProvider, ViewHelper.Origin.Right, horizontalPrefab);

        rightDataProvider.Clear(false);

        for (int i = 0, max = 100; i < max; i++)
        {
            var data = new ExampleListViewData(i, rightDataProvider);
            data.StringName.Value = "right : " + i;

            rightDataProvider.Add(data, i == max - 1);
        }
    }
}
