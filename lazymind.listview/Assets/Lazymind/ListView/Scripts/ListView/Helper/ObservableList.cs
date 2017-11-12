using System.Collections.Generic;
using System.Linq;
using System;

public class ObservableList<TYPE> where TYPE : ListViewData
{
    public delegate void ItemAddEvent(ObservableList<TYPE> _sender);
    public event ItemAddEvent ItemAdded;
    public void OnItemAdded() { if (ItemAdded != null) ItemAdded(this); }

    public delegate void ItemRemoveEvent(ObservableList<TYPE> _sender, bool _result);
    public event ItemRemoveEvent ItemRemoved;
    public void OnItemRemoved(bool _result = true) { if (ItemRemoved != null) ItemRemoved(this, _result); }

    public delegate void ClearEvent(ObservableList<TYPE> _sender);
    public event ClearEvent Cleared;
    public void OnClear() { if (Cleared != null) Cleared(this); }

    readonly List<TYPE> m_list = new List<TYPE>();

    public void Insert(int _index, TYPE _data, bool _notify = true)
    {
        m_list.Insert(_index, _data);

        if (_notify)
            OnItemAdded();
    }

    public void InsertRange(int _index, IEnumerable<TYPE> _collection, bool _notify = true)
    {
        m_list.InsertRange(_index, _collection);

        if (_notify)
            OnItemAdded();
    }

    public void Add(TYPE _data, bool _notify = true)
    {
        m_list.Add(_data);

        if(_notify)
            OnItemAdded();
    }

    public void AddRange(IEnumerable<TYPE> _collection, bool _notify = true)
    {
        m_list.AddRange(_collection);

        if (_notify)
            OnItemAdded();
    }

    public bool Remove(TYPE _data, bool _notify = true)
    {
        var result = m_list.Remove(_data);

        if(_notify)
            OnItemRemoved(result);

        return result;
    }

    public void RemoveAt(int _index, bool _notify = true)
    {
        m_list.RemoveAt(_index);

        if(_notify)
            OnItemRemoved();
    }

    public int RemoveAll(System.Predicate<TYPE> _match, bool _notify = true)
    {
        var result = m_list.RemoveAll(_match);

        if(_notify)
            OnItemRemoved();

        return result;
    }

    public void Clear(bool _notify = true)
    {
        m_list.Clear();

        if(_notify)
            OnClear();
    }

    public TYPE LastOrDefault()
    {
        return m_list.LastOrDefault();
    }

    public int Count
    {
        get
        {
            return m_list.Count;
        }
    }

    public TYPE ElementAtOrDefault(int _index)
    {
        return m_list.ElementAtOrDefault(_index);
    }

    public int FindIndex(Predicate<TYPE> _match)
    {
        return m_list.FindIndex(_match);
    }

    public int FindLastIndex(Predicate<TYPE> _match)
    {
        return m_list.FindLastIndex(_match);
    }

    public List<TYPE> GetRange(int index, int count)
    {
        return m_list.GetRange(index, count);
    }

}
