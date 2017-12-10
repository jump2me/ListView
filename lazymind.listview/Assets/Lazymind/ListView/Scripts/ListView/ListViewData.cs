using UnityEngine;
using Lazymind;

public class ListViewData
{ 
    public int ID { get; private set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public int Min { get; set; }
    public int Max { get; set; }

    public int X { get; set; }
    public int Y { get; set; }

    private GameObject m_prefab;
    public GameObject Prefab 
    { 
        get
        {
            return m_prefab;
        }
        private set
        {
            m_prefab = value;

            if (Width < 1)
            {
                Width = (int)m_prefab.GetComponent<RectTransform>().rect.width;
            }

            if(Height < 1)
            {
                Height = (int)m_prefab.GetComponent<RectTransform>().rect.height;
            }
        }
    }

    public ListViewData(int _id, GameObject _prefab)
    {
        ID = _id;
        Prefab = _prefab;
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

    public override string ToString()
    {
        return string.Format("[ListViewData: ID={0}, Width={1}, Height={2}, Min={3}, Max={4}, X={5}, Y={6}, Prefab={7}]", ID, Width, Height, Min, Max, X, Y, Prefab);
    }
}