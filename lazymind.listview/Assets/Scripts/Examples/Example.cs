using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour 
{
    public ObservableList<ExampleListViewData> topDataProvider { get; set; }
    public RectTransform topRectTransform;
    public ListViewModel<ExampleListViewData> topList { get; set; }

    public ObservableList<ExampleListViewData> bottomDataProvider { get; set; }
    public RectTransform bottomRectTransform;
    public ListViewModel<ExampleListViewData> bottomList { get; set; }

    public ObservableList<ExampleListViewData> leftDataProvider { get; set; }
    public RectTransform leftRectTransform;
    public ListViewModel<ExampleListViewData> leftList { get; set; }

    public ObservableList<ExampleListViewData> rightDataProvider { get; set; }
    public RectTransform rightRectTransform;
    public ListViewModel<ExampleListViewData> rightList { get; set; }

    void Start()
    {
        // top
        topDataProvider = new ObservableList<ExampleListViewData>();
        topList = new ListViewModel<ExampleListViewData>(topRectTransform, topDataProvider, ListViewHelper.Origin.Top);

        var prefab = Resources.Load<GameObject>("Prefab/ExampleListItem_Vertical");

        topDataProvider.Clear(false);

        for (int i = 0, max = 100; i < max; i++)
        {
            var data = new ExampleListViewData
            {
                ID = i,

                Width = (int)topRectTransform.rect.width,
                Height = 50,

                Prefab = prefab,
                StringName = "top : " + i,
            };

            topDataProvider.Add(data, i == max - 1);
        }

        // bottom
        bottomDataProvider = new ObservableList<ExampleListViewData>();
        bottomList = new ListViewModel<ExampleListViewData>(bottomRectTransform, bottomDataProvider, ListViewHelper.Origin.Bottom);

        bottomDataProvider.Clear(false);

        for (int i = 0, max = 100; i < max; i++)
        {
            var data = new ExampleListViewData
            {
                ID = i,

                Width = (int)bottomRectTransform.rect.width,
                Height = 50,

                Prefab = prefab,
                StringName = "bottom : " + i,
            };

            bottomDataProvider.Add(data, i == max - 1);
        }

        // left
        leftDataProvider = new ObservableList<ExampleListViewData>();
        leftList = new ListViewModel<ExampleListViewData>(leftRectTransform, leftDataProvider, ListViewHelper.Origin.Left);

        prefab = Resources.Load<GameObject>("Prefab/ExampleListItem_Horizontal");

        leftDataProvider.Clear(false);

        for (int i = 0, max = 100; i < max; i++)
        {
            var data = new ExampleListViewData
            {
                ID = i,

                Width = 150,
                Height = (int)leftRectTransform.rect.height,

                Prefab = prefab,
                StringName = "left : " + i,
            };

            leftDataProvider.Add(data, i == max - 1);
        }


        // right
        rightDataProvider = new ObservableList<ExampleListViewData>();
        rightList = new ListViewModel<ExampleListViewData>(rightRectTransform, rightDataProvider, ListViewHelper.Origin.Right);

        prefab = Resources.Load<GameObject>("Prefab/ExampleListItem_Horizontal");

        rightDataProvider.Clear(false);

        for (int i = 0, max = 100; i < max; i++)
        {
            var data = new ExampleListViewData
            {
                ID = i,

                Width = 150,
                Height = (int)rightRectTransform.rect.height,

                Prefab = prefab,
                StringName = "right : " + i,
            };

            rightDataProvider.Add(data, i == max - 1);
        }
    }
}
