using System.Collections.Generic;
using UnityEngine;
using Lazymind;
using System.Linq;

public class ListViewBase<TYPE> where TYPE : ListViewData
{
    public struct ListItemInfo
    {
        public RectTransform RectTransform { get; set; }
        public ListItem<TYPE> ListItem { get; set; }
    }

    public ViewComponent ViewComponent { get; set; }
    public ObservableList<TYPE> DataProvider { get; set; }
    public Dictionary<string, GameObject> PrefabByName { get; private set; }
    public Dictionary<string, Vector2> PrefabSizeByName { get; private set; }

    public Dictionary<string, Queue<ListItemInfo>> ListItemInfoQueueByPrefabName { get; set; }

    public List<TYPE> RenderedDataList { get; set; }

    SimpleGameObjectPool ListItemRendererPool { get; set; }

    protected void Init(RectTransform _rectTransform, ObservableList<TYPE> _dataProvider, params GameObject[] _prefabs)
    {
        ViewComponent    = _rectTransform.gameObject.AddComponent<ViewComponent>();
        DataProvider     = _dataProvider;

        RenderedDataList = new List<TYPE>();

        ListItemInfoQueueByPrefabName = new Dictionary<string, Queue<ListItemInfo>>();

        ViewComponent.ScrollEvent += OnScroll;

        RegisterEvents();

        ListItemRendererPool = new SimpleGameObjectPool();

        PrefabByName = new Dictionary<string, GameObject>();
        PrefabSizeByName = new Dictionary<string, Vector2>();
        foreach(GameObject prefab in _prefabs)
        {
            if(PrefabByName.ContainsKey(prefab.name) == false)
            {
                PrefabByName.Add(prefab.name, prefab);

                var rectTransform = prefab.GetComponent<RectTransform>();
                PrefabSizeByName.Add(prefab.name, rectTransform.rect.size);
            }
        }
    }

    public void RegisterEvents()
    {
        DataProvider.ItemAdded      += OnItemAdded;
        DataProvider.ItemRemoved    += OnItemRemoved;
        DataProvider.Cleared        += OnCleared;
    }

    public void UnregisterEvents()
    {
        DataProvider.ItemAdded      -= OnItemAdded;
        DataProvider.ItemRemoved    -= OnItemRemoved;
        DataProvider.Cleared        -= OnCleared;
    }

    public void CleanUp()
    {
        UnregisterEvents();

        ClearQueue();

        UnityEngine.Object.Destroy(ViewComponent.gameObject);
        UnityEngine.Object.Destroy(ViewComponent);

        ViewComponent = null;
        DataProvider = null;
    }

    void OnItemAdded(ObservableList<TYPE> _collection)
    {
        CalcContentCoordination();
    }

    void OnItemRemoved(ObservableList<TYPE> _collection, bool _result)
    {
        // need better solution...
        ClearQueue();

        CalcContentCoordination();
    }

    void OnCleared(ObservableList<TYPE> _collection)
    {
        ClearQueue();

        CalcContentCoordination();
    }

    void ClearQueue()
    {
        foreach (var kv in ListItemInfoQueueByPrefabName)
        {
            foreach (ListItemInfo info in kv.Value)
            {
                info.ListItem.Clear();
                ListItemRendererPool.Release(info.ListItem.gameObject);
            }
        }

        ListItemInfoQueueByPrefabName.Clear();
    }

    protected void InvalidateListItem(TYPE _data)
    {
        RectTransform listItemRectTransform = null;
        ListItem<TYPE> listItem = null;

        GameObject prefab = null;
        string prefabName = _data.PrefabName;
        prefab = GetPrefab(prefabName);

        if (ListItemInfoQueueByPrefabName.ContainsKey(prefab.name) == false)
            ListItemInfoQueueByPrefabName.Add(prefab.name, new Queue<ListItemInfo>());

        var queue = ListItemInfoQueueByPrefabName[prefab.name];
        if (queue.Count < RenderedDataList.Count)
        {
            GameObject go = ListItemRendererPool.Get(prefab);

            listItemRectTransform = go.GetComponent<RectTransform>();
            listItemRectTransform.SetAnchor(ViewComponent.Origin);
            listItemRectTransform.SetParent(ViewComponent.Content, false);

            listItem = go.GetComponent<ListItem<TYPE>>();

            queue.Enqueue(new ListItemInfo() { RectTransform = listItemRectTransform, ListItem = listItem });
        }
        else
        {
            var info = queue.Dequeue();
            listItem = info.ListItem;
            listItemRectTransform = info.RectTransform;

            queue.Enqueue(info);
        }

        var anchoredPosition = GetListItemAnchoredPosition(_data);

        listItemRectTransform.anchoredPosition = anchoredPosition;
        listItemRectTransform.sizeDelta = GetListItemSizeDelta(prefab.name);

        listItem.SetData(_data);
    }

    protected virtual Vector2 GetListItemSizeDelta(string _prefabName)
    {
        return PrefabSizeByName[_prefabName];
    }

    protected GameObject GetPrefab(string prefabName)
    {
        GameObject prefab;
        if (string.IsNullOrEmpty(prefabName) || PrefabByName.TryGetValue(prefabName, out prefab) == false)
            prefab = PrefabByName.FirstOrDefault().Value;
        
        return prefab;
    }

    protected virtual void OnScroll(Vector2 _value)
    {
        for (int i = 0, max = RenderedDataList.Count; i < max; i++)
        {
            var data = RenderedDataList[i];
            InvalidateListItem(data);
        }
    }

    protected virtual void CalcContentCoordination()
    {
        OnScroll(ViewComponent.Content.anchoredPosition);
    }

    protected virtual Vector2 GetListItemAnchoredPosition(TYPE _data)
    {
        return Vector2.zero;
    }
}