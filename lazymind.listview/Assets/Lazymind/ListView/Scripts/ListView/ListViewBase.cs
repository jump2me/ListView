using System.Collections.Generic;
using UnityEngine;

public class SimpleGameObjectPool
{
    Dictionary<string, Queue<GameObject>> m_queueByPrefabName = new Dictionary<string, Queue<GameObject>>();

    public GameObject Get(GameObject _prefab)
    {
        var prefabName = _prefab.name;
        if (m_queueByPrefabName.ContainsKey(prefabName) == false)
            m_queueByPrefabName.Add(prefabName, new Queue<GameObject>());

        if (m_queueByPrefabName[prefabName].Count == 0)
        {
            var item = UnityEngine.Object.Instantiate(_prefab);
            item.name = prefabName;
            m_queueByPrefabName[prefabName].Enqueue(item);
        }

        var go = m_queueByPrefabName[prefabName].Dequeue();
        if (go.activeInHierarchy == false)
            go.SetActive(true);

        return go;
    }

    public void Release(GameObject _go)
    {
        _go.SetActive(false);

        var prefabName = _go.name;
        if (m_queueByPrefabName.ContainsKey(prefabName) == false)
        {
            m_queueByPrefabName.Add(prefabName, new Queue<GameObject>());
        }
        m_queueByPrefabName[prefabName].Enqueue(_go);
    }
}

public class ListViewBase<TYPE> where TYPE : ListViewData
{
    public struct ListItemInfo
    {
        public RectTransform RectTransform { get; set; }
        public ListItem<TYPE> ListItem { get; set; }
    }

    public ViewComponent ViewComponent { get; set; }
    public ObservableList<TYPE> DataProvider { get; set; }
    public Dictionary<GameObject, Queue<ListItemInfo>> ListItemInfoQueueByPrefab { get; set; }

    public List<TYPE> RenderedDataList { get; set; }

    SimpleGameObjectPool ListItemRendererPool { get; set; }

    protected void Init(RectTransform _rectTransform, ObservableList<TYPE> _dataProvider)
    {
        ViewComponent    = _rectTransform.gameObject.AddComponent<ViewComponent>();
        DataProvider     = _dataProvider;
        ListItemInfoQueueByPrefab    = new Dictionary<GameObject, Queue<ListItemInfo>>();
        RenderedDataList = new List<TYPE>();

        ViewComponent.ScrollEvent += OnScroll;

        RegisterEvents();

        ListItemRendererPool = new SimpleGameObjectPool();
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
        foreach (var kv in ListItemInfoQueueByPrefab)
        {
            foreach (ListItemInfo info in kv.Value)
            {
                info.ListItem.Clear();
                ListItemRendererPool.Release(info.ListItem.gameObject);
            }
        }

        ListItemInfoQueueByPrefab.Clear();
    }

    protected void InvalidateListItem(TYPE _data)
    {
        var anchoredPosition = GetListItemAnchoredPosition(_data);

        RectTransform rectTransform = null;
        ListItem<TYPE> listItem = null;

        if (ListItemInfoQueueByPrefab.ContainsKey(_data.Prefab) == false)
            ListItemInfoQueueByPrefab.Add(_data.Prefab, new Queue<ListItemInfo>());

        var queue = ListItemInfoQueueByPrefab[_data.Prefab];
        if (queue.Count < RenderedDataList.Count)
        {
            GameObject go = ListItemRendererPool.Get(_data.Prefab);

            rectTransform = go.GetComponent<RectTransform>();
            rectTransform.SetAnchor(ViewComponent.Origin);
            rectTransform.SetParent(ViewComponent.Content, false);

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