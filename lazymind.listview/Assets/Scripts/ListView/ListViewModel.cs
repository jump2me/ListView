using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ListViewModel<TYPE> where TYPE : ListViewData
{
    public struct ListItemInfo
    {
        public RectTransform RectTransform { get; set; }
        public ListItem<TYPE> ListItem { get; set; }
    }

    public ListView ListView { get; set; }
    public ObservableList<TYPE> DataProvider { get; set; }
    public Dictionary<GameObject, Queue<ListItemInfo>> QueueByPrefab { get; set; }

    public List<TYPE> RenderedDataList { get; set; }

    public ListViewModel(RectTransform _rectTransform, ObservableList<TYPE> _dataProvider, ListViewHelper.Origin _origin)
    {
        ListView = _rectTransform.gameObject.AddComponent<ListView>();
        ListView.SetAnchor(_origin);

        DataProvider = _dataProvider;
        QueueByPrefab = new Dictionary<GameObject, Queue<ListItemInfo>>();
    
        RenderedDataList = new List<TYPE>();

        ListView.ScrollEvent += OnScroll;

        Register();
    }

    public void Register()
    {
        DataProvider.ItemAdded += OnItemAdded;
        DataProvider.ItemRemoved += OnItemRemoved;
        DataProvider.Cleared += OnCleared;
    }

    public void Unregister()
    {
        DataProvider.ItemAdded -= OnItemAdded;
        DataProvider.ItemRemoved -= OnItemRemoved;
        DataProvider.Cleared -= OnCleared;
    }

    public void CleanUp()
    {
        Unregister();

        UnityEngine.Object.Destroy(ListView.gameObject);
        UnityEngine.Object.Destroy(ListView);

        ListView = null;
        DataProvider = null;
    }

    void OnItemAdded(ObservableList<TYPE> _collection)
    {
        InvalidateView();
    }

    void OnItemRemoved(ObservableList<TYPE> _collection, bool _result)
    {
        ClearQueue();

        InvalidateView();
    }

    void ClearQueue()
    {
        foreach (var kv in QueueByPrefab)
        {
            foreach (ListItemInfo info in kv.Value)
            {
                UnityEngine.Object.Destroy(info.ListItem.gameObject);
                UnityEngine.Object.Destroy(info.ListItem);
            }
        }

        QueueByPrefab.Clear();
    }

    void OnCleared(ObservableList<TYPE> _collection)
    {
        ClearQueue();

        InvalidateView();
    }

    void OnScroll(Vector2 _value)
    {
        var origin = ListView.Origin;

        var top = 0f;
        var bottom = 0f;
        switch(origin)
        {
            case ListViewHelper.Origin.Top:
                top = _value.y;
                bottom = (top + ListView.RectTransform.rect.height);
                break;
            case ListViewHelper.Origin.Bottom:
                top = _value.y;
                bottom = (top - ListView.RectTransform.rect.height);
                break;
            case ListViewHelper.Origin.Left:
                top = _value.x;
                bottom = (top - ListView.RectTransform.rect.width);
                break;
            case ListViewHelper.Origin.Right:
                top = _value.x;
                bottom = (top + ListView.RectTransform.rect.width);
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

        for (int i = 0, max = RenderedDataList.Count; i < max; i++)
        {
            var data = RenderedDataList[i];
            var listItem = GetListItem(data);
        }
    }

    void InvalidateView()
    {
        var sum = 0;
        var origin = ListView.Origin;
        for (int i = 0, max = DataProvider.Count; i < max; i++)
        {
            var data = DataProvider.Items[i];
            data.Min = sum;

            var value = 0;
            switch(origin)
            {
                case ListViewHelper.Origin.Top:
                case ListViewHelper.Origin.Bottom:
                    value = data.Height;
                    break;
                case ListViewHelper.Origin.Left:
                case ListViewHelper.Origin.Right:
                    value = data.Width;
                    break;
            }
            data.Max = sum + value;

            sum += value;
        }

        switch(origin)
        {
            case ListViewHelper.Origin.Top:
            case ListViewHelper.Origin.Bottom:
                ListView.SetContentRectTransform(ListView.Content.sizeDelta.x, sum);
                break;
            case ListViewHelper.Origin.Left:
            case ListViewHelper.Origin.Right:
                ListView.SetContentRectTransform(sum, ListView.Content.sizeDelta.y);
                break;
        }

        OnScroll(Vector2.zero);
    }

    public ListItem<TYPE> GetListItem(TYPE _data)
    {
        var anchoredPosition = Vector2.zero;

        switch(ListView.Origin)
        {
            case ListViewHelper.Origin.Top:
                anchoredPosition.y = -_data.Min;
                break;
            case ListViewHelper.Origin.Bottom:
                anchoredPosition.y = _data.Min;
                break;
            case ListViewHelper.Origin.Left:
                anchoredPosition.x = _data.Min;
                break;
            case ListViewHelper.Origin.Right:
                anchoredPosition.x = -_data.Min;
                break;
        }

        RectTransform rectTransform = null;
        ListItem<TYPE> listItem = null;

        if (QueueByPrefab.ContainsKey(_data.Prefab) == false)
            QueueByPrefab.Add(_data.Prefab, new Queue<ListItemInfo>());

        var queue = QueueByPrefab[_data.Prefab];
        if (queue.Count < RenderedDataList.Count)
        {
            var go = UnityEngine.Object.Instantiate(_data.Prefab);
            rectTransform = go.GetComponent<RectTransform>();
            rectTransform.SetAnchor(ListView.Origin);
            rectTransform.SetParent(ListView.Content, false);

            listItem = go.GetComponent<ListItem<TYPE>>();

            queue.Enqueue(new ListItemInfo() { RectTransform = rectTransform, ListItem = listItem });
        }
        else
        {
            var info = queue.Dequeue();
            listItem = info.ListItem;
            rectTransform = info.RectTransform;

            queue.Enqueue(info);
        }

        rectTransform.anchoredPosition = anchoredPosition;

        listItem.SetData(_data);

        return listItem;
    }
}
