using UnityEngine;
using Lazymind;

public class ListViewData
{ 
    public int ID { get; private set; }
    //public int Width { get; set; }
    //public int Height { get; set; }

    public int Min { get; set; }
    public int Max { get; set; }

    public int X { get; set; }
    public int Y { get; set; }

    public string PrefabName { get; set; }

    public ListViewData(int _id, string _prefabName = "")
    {
        ID = _id;
        PrefabName = _prefabName;
    }

    public bool IsWithin(float x)
    {
        return MathHelper.IsWithin(x, Min, Max);
    }

    public delegate void NotifyChangeEvent(object _sender);
    public event NotifyChangeEvent NotifyChange;
    public void OnNotifyChange() 
    { 
        if (NotifyChange != null) 
            NotifyChange(this);
    }
}