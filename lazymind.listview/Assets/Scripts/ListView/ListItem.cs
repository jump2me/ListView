using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListItem<TYPE> : MonoBehaviour where TYPE : ListViewData
{
    public RectTransform RectTransform { get; private set; }
    public TYPE Data { get; private set; }

    private void Awake()
    {
        RectTransform = gameObject.GetComponent<RectTransform>();
    }

    public void SetData(TYPE _data)
    {
        Data = _data;

        RectTransform.sizeDelta = new Vector2(Data.Width, Data.Height);

        Invalidate();
    }

    public virtual void Invalidate()
    {
        
    }
}
