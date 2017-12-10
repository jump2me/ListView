using UnityEngine;

public class TileView<TYPE> : ListViewBase<TYPE> where TYPE : ListViewData
{
    public int Column { get; set; }

    public TileView(RectTransform _rectTransform, ObservableList<TYPE> _dataProvider, int _column, params GameObject[] _prefabs)
    {
        base.Init(_rectTransform, _dataProvider, _prefabs);

        ViewComponent.SetAnchor(ViewHelper.Origin.Top);
        Column = _column;
    }

    protected override void CalcContentCoordination()
    {
        var maxY = 0;
        for (int index = 0, max = DataProvider.Count; index < max; index++)
        {
            var data = DataProvider[index];
            var prefab = GetPrefab(data.PrefabName);

            var size = PrefabSizeByName[prefab.name];
            var width = (int)size.x;
            var height = (int)size.y;

            data.X = (index % Column) * width;
            data.Y = (index / Column) * height;

            data.Min = data.Y;
            data.Max = data.Y + height;

            maxY = data.Max;
        }

        ViewComponent.SetContentRectTransform(ViewComponent.Content.sizeDelta.x, maxY);

        base.CalcContentCoordination();
    }

    protected override void OnScroll(Vector2 _value)
    {
        var top = _value.y;
        var bottom = (top + ViewComponent.RectTransform.rect.height);

        var firstIndex = DataProvider.FindIndex(e => e.IsWithin(top));
        var lastIndex = DataProvider.FindLastIndex(e => e.IsWithin(bottom));

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
        return new Vector2(_data.X, -_data.Y);
    }
}
