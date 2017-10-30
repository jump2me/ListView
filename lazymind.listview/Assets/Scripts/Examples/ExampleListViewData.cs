using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleListViewData : ListViewData 
{
    public string StringName { get; set; }
    public ObservableList<ExampleListViewData> DataProvider { get; set; }
}
