using System.Collections;using System.Collections.Generic;using UnityEngine;


public class SimpleDropdownGroup : MonoBehaviour
{
    private List<SimpleDropdown> dropdowns;

    public void CloseOtherDropDown(SimpleDropdown clicked)
    {
        foreach (var item in dropdowns)
        {
            if (item != null && item != clicked)
            {
                item.Hide();
            }
        }
    }

    public void Add(SimpleDropdown dropdown)
    {
        if (dropdowns == null)
        {
            dropdowns = new List<SimpleDropdown>();
        }
        dropdowns.Add(dropdown);
    }
}