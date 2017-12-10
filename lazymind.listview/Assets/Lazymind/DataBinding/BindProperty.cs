using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindProperty<TYPE>
{
    public TYPE m_value;
    public TYPE Value
    {
        get
        {
            return m_value;
        }
        set
        {
            m_value = value;

            OnValueChanged();
        }
    }

    public delegate void ValueChangeEvent();
    private event ValueChangeEvent ValueChanged;
    private void OnValueChanged()
    {
        if (ValueChanged != null)
        {
            ValueChanged();
        }
    }

    public BindProperty(ValueChangeEvent _callback)
    {
        ValueChanged += _callback;
    }
}