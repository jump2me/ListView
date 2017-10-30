using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ListView<TYPE> : ListViewBase<TYPE> where TYPE : ListViewData
{
    public ListView(RectTransform _rectTransform, ObservableList<TYPE> _dataProvider, ViewHelper.Origin _origin)
    {
        base.Init(_rectTransform, _dataProvider);

        ViewComponent.SetAnchor(_origin);
    }

    protected override void CalcContentCoordination()
    {
        var sum = 0;
        var origin = ViewComponent.Origin;
        for (int i = 0, max = DataProvider.Count; i < max; i++)
        {
            var data = DataProvider.Items[i];
            data.Min = sum;
            switch (origin)
            {
                case ViewHelper.Origin.Top:
                case ViewHelper.Origin.Bottom:
                    data.Max = sum + data.Height;
                    sum += data.Height;
                    break;
                case ViewHelper.Origin.Left:
                case ViewHelper.Origin.Right:
                    data.Max = sum + data.Width;
                    sum += data.Width;
                    break;
            }
        }

        switch (origin)
        {
            case ViewHelper.Origin.Top:
            case ViewHelper.Origin.Bottom:
                ViewComponent.SetContentRectTransform(ViewComponent.Content.sizeDelta.x, sum);
                break;
            case ViewHelper.Origin.Left:
            case ViewHelper.Origin.Right:
                ViewComponent.SetContentRectTransform(sum, ViewComponent.Content.sizeDelta.y);
                break;
        }

        base.CalcContentCoordination();
    }

    protected override void OnScroll(Vector2 _value)
    {
        var origin = ViewComponent.Origin;

        var top = 0f;
        var bottom = 0f;
        switch (origin)
        {
            case ViewHelper.Origin.Top:
                top = _value.y;
                bottom = (top + ViewComponent.RectTransform.rect.height);
                break;
            case ViewHelper.Origin.Bottom:
                top = _value.y;
                bottom = (top - ViewComponent.RectTransform.rect.height);
                break;
            case ViewHelper.Origin.Left:
                top = _value.x;
                bottom = (top - ViewComponent.RectTransform.rect.width);
                break;
            case ViewHelper.Origin.Right:
                top = _value.x;
                bottom = (top + ViewComponent.RectTransform.rect.width);
                break;
        }

        var items = DataProvider.Items;
        var firstIndex = items.FindIndex(e => e.IsWithin(top));
        var lastIndex = items.FindIndex(e => e.IsWithin(bottom));

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

        switch (ViewComponent.Origin)
        {
            case ViewHelper.Origin.Top:
                _anchoredPosition.y = -_data.Min;
                break;
            case ViewHelper.Origin.Bottom:
                _anchoredPosition.y = _data.Min;
                break;
            case ViewHelper.Origin.Left:
                _anchoredPosition.x = _data.Min;
                break;
            case ViewHelper.Origin.Right:
                _anchoredPosition.x = -_data.Min;
                break;
        }
    }
}
