using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class TileView<TYPE> : ListViewBase<TYPE> where TYPE : ListViewData
{
    public int Column { get; set; }

    public TileView(RectTransform _rectTransform, ObservableList<TYPE> _dataProvider, int _column = 1)
    {
        base.Init(_rectTransform, _dataProvider);

        ViewComponent.SetAnchor(ViewHelper.Origin.Top);
        Column = _column;
    }

    protected override void CalcContentCoordination()
    {
        var maxY = 0;
        for (int i = 0, max = DataProvider.Count; i < max; i++)
        {
            var data = DataProvider.Items[i];
            data.X = (i % Column) * data.Width;
            data.Y = (i / Column) * data.Height;

            data.Min = data.Y;
            data.Max = data.Y + data.Height;

            maxY = data.Max;
        }

        ViewComponent.SetContentRectTransform(ViewComponent.Content.sizeDelta.x, maxY);

        base.CalcContentCoordination();
    }

    protected override void OnScroll(Vector2 _value)
    {
        var top = _value.y;
        var bottom = (top + ViewComponent.RectTransform.rect.height);

        var items = DataProvider.Items;
        var firstIndex = items.FindIndex(e => e.IsWithin(top));
        var lastIndex = items.FindLastIndex(e => e.IsWithin(bottom));

        if (firstIndex == -1)
            firstIndex = 0;

        if (lastIndex == -1)
        {
            lastIndex = items.Count;
        }
        else
        {
            lastIndex += 1;
        }

        RenderedDataList = items.GetRange(firstIndex, lastIndex - firstIndex);

        base.OnScroll(_value);
    }

    protected override void CalcListItemPosition(TYPE _data, out Vector2 _anchoredPosition)
    {
        _anchoredPosition = new Vector2(_data.X, -_data.Y);
    }
}
