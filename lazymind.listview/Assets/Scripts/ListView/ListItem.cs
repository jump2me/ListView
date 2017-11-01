using UnityEngine;

public class ListItem<TYPE> : MonoBehaviour where TYPE : ListViewData
{
    public RectTransform RectTransform { get; private set; }
    protected TYPE Data { get; set; }

    protected virtual void Awake()
    {
        RectTransform = gameObject.GetComponent<RectTransform>();
    }

    public virtual void Clear()
    {
        Data = null;
    }

    public void SetData(TYPE _data)
    {
        if (Data != null && Data.ID == _data.ID)
            return;

        if(Data != null)
            Data.NotifyChange -= OnNotifyChange;

        Data = _data;
        Data.NotifyChange += OnNotifyChange;

        RectTransform.sizeDelta = new Vector2(Data.Width, Data.Height);

        Invalidate();
    }

    protected virtual void Invalidate()
    {
        
    }

    void OnNotifyChange(object _sender)
    {
        Invalidate();
    }
}
