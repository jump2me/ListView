﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileExample : MonoBehaviour 
{
    public RectTransform tileListRectTransform;
    public ObservableList<ExampleListViewData> DataProvider { get; set; }

    public TileView<ExampleListViewData> tileView { get; set; }

    private void Start()
    {
        DataProvider = new ObservableList<ExampleListViewData>();
        tileView = new TileView<ExampleListViewData>(tileListRectTransform, DataProvider, 5);

        var prefab = Resources.Load<GameObject>("Prefab/ExampleListItem_Tile");

        DataProvider.Clear(false);

        for (int i = 0, max = 107; i < max; i++)
        {
            var data = new ExampleListViewData
            {
                ID = i,

                Prefab = prefab,
                StringName = "top : " + i,

                DataProvider = DataProvider,
            };

            DataProvider.Add(data, i == max - 1);
        }
    }
}