using UnityEngine;

public class ListView<TYPE> : ListViewBase<TYPE> where TYPE : ListViewData
{
    public ListView(RectTransform _rectTransform, ObservableList<TYPE> _dataProvider, ViewHelper.Origin _origin, params GameObject[] _prefabs)
    {
        base.Init(_rectTransform, _dataProvider, _prefabs);

        ViewComponent.SetAnchor(_origin);
    }

    protected override void CalcContentCoordination()
    {
        var sum = 0;
        var origin = ViewComponent.Origin;
        for (int index = 0, max = DataProvider.Count; index < max; index++)
        {
            var data = DataProvider[index];
            var prefab = GetPrefab(data.PrefabName);

            var size = PrefabSizeByName[prefab.name];
            var width = (int)size.x;
            var height = (int)size.y;

            data.Min = sum;
            switch (origin)
            {
                case ViewHelper.Origin.Top:
                case ViewHelper.Origin.Bottom:
                    data.Max = sum + height;
                    sum += height;
                    break;
                case ViewHelper.Origin.Left:
                case ViewHelper.Origin.Right:
                    data.Max = sum + width;
                    sum += width;
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

        var firstIndex = DataProvider.FindIndex(e => e.IsWithin(top));
        var lastIndex = DataProvider.FindIndex(e => e.IsWithin(bottom));

        if (firstIndex == -1)
            firstIndex = 0;

        if (lastIndex == -1)
        {
            lastIndex = DataProvider.Count;
        }
        else
        {
            lastIndex += 1;
        }

        RenderedDataList = DataProvider.GetRange(firstIndex, lastIndex - firstIndex);

        base.OnScroll(_value);
    }

    protected override Vector2 GetListItemAnchoredPosition(TYPE _data)
    {
        var result = new Vector2(_data.X, -_data.Y);

        switch (ViewComponent.Origin)
        {
            case ViewHelper.Origin.Top:
                result.y = -_data.Min;
                break;
            case ViewHelper.Origin.Bottom:
                result.y = _data.Min;
                break;
            case ViewHelper.Origin.Left:
                result.x = _data.Min;
                break;
            case ViewHelper.Origin.Right:
                result.x = -_data.Min;
                break;
        }

        return result;
    }

    protected override Vector2 GetListItemSizeDelta(string _prefabName)
    {
        var size = PrefabSizeByName[_prefabName];

        var width = size.x;
        var height = size.y;

        var rect = ViewComponent.RectTransform.rect;
        switch(ViewComponent.Origin)
        {
            case ViewHelper.Origin.Top:
            case ViewHelper.Origin.Bottom:
                return new Vector2(rect.width, height);
            case ViewHelper.Origin.Left:
            case ViewHelper.Origin.Right:
                return new Vector2(width, rect.height);
        }

        return new Vector2(width, height);
    }
}
