using UnityEngine;

public class ListItem<TYPE> : MonoBehaviour where TYPE : ListViewData
{
    public RectTransform RectTransform { get; private set; }
    public TYPE Data { get; private set; }

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
        Data = _data;

        RectTransform.sizeDelta = new Vector2(Data.Width, Data.Height);

        Invalidate();
    }

    public TYPE GetData()
    {
        return Data;
    }

    protected virtual void Invalidate()
    {
        
    }
}
