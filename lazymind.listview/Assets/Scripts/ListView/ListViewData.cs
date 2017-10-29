using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListViewData
{ 
    public int ID { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public int Min { get; set; }
    public int Max { get; set; }

    private GameObject m_prefab;
    public GameObject Prefab 
    { 
        get
        {
            return m_prefab;
        }
        set
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

    public bool IsWithin(float x)
    {
        return MathHelper.IsWithin(x, Min, Max);
    }
    public override string ToString()
    {
        return string.Format("[ListViewData: ID={0}, Width={1}, Height={2}, Min={3}, Max={4}, Prefab={5}]", ID, Width, Height, Min, Max, Prefab);
    }
}