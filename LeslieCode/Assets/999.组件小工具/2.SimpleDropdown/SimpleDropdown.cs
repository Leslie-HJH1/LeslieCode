using System;using System.Collections;using System.Collections.Generic;using UnityEngine;using UnityEngine.UI;using UnityEngine.EventSystems;


public class SimpleDropdown : MonoBehaviour
{
    public SimpleDropdownGroup dropdownGroup;
    public GameObject blocker;
    public GameObject dropdownRoot;
    public GameObject optionItem;
    public Transform contentRoot;
    public SimpleDropdownItem shownOptionItem;

    public Action<int> OnValueChanged
    {
        set; get;
    }

    public int Value
    {
        get
        {
            return valueInternal;
        }
        set
        {
            if (value != valueInternal && value >= 0)
            {
                OnValueChanged(value);
                valueInternal = value;
                RefreshShownValue();
            }
        }
    }

    private bool isShowing = false;
    private List<SimpleDropDownItemData> options;
    private int valueInternal = 0;

    public void AddOptions(List<SimpleDropDownItemData> options, bool inverseAdd = false)
    {
        if (options == null)
        {
            Debug.LogError("options = null!");
            return;
        }

        if (this.options == null)
        {
            this.options = new List<SimpleDropDownItemData>(options.Count);
        }

        if (inverseAdd)
        {
            for (int i = options.Count - 1; i >= 0; i--)
            {
                this.options.Add(new SimpleDropDownItemData(options[i]));
            }
        }
        else
        {
            for (int i = 0; i < options.Count; i++)
            {
                this.options.Add(new SimpleDropDownItemData(options[i]));
            }
        }
        RefreshShownValue();
    }

    private void RefreshShownValue()
    {
        if (options == null)
        {
            Debug.LogError("options = null!");
            return;
        }

        if (valueInternal < 0 || valueInternal >= options.Count)
        {
            Debug.LogError("错误的索引值！");
        }

        SimpleDropDownItemData optionData = options[Mathf.Clamp(valueInternal, 0, options.Count - 1)];
        if (shownOptionItem != null)
        {
            shownOptionItem.SetContent(optionData.text, optionData.imageName);
        }
    }

    public void ClearOptions()
    {
        if (options != null)
        {
            options.Clear();
        }
    }


    public void Hide()
    {
        if (!isShowing)
        {
            return;
        }

        contentRoot.DestroyChildren();
        dropdownRoot.SetActive(false);
        blocker.SetActive(false);
        isShowing = false;
    }

    public void Show()
    {
        if (isShowing)
        {
            return;
        }

        if (options == null)
        {
            Debug.LogError("options = null!");
            return;
        }

        dropdownRoot.gameObject.SetActive(true);
        blocker.SetActive(true);

        for (int i = 0; i < options.Count; i++)
        {
            SimpleDropDownItemData data = options[i];
            if (data == null)
            {
                continue;
            }
            GameObject itemGameObj = Instantiate(optionItem, contentRoot);
            itemGameObj.SetActive(true);
            SimpleDropdownItem item = itemGameObj.GetComponent<SimpleDropdownItem>();
            if (item != null)
            {
                item.name = i.ToString();
                item.SetContent(data.text, data.imageName, valueInternal == i);
                item.toggle.onValueChanged.AddListener((isOn) =>
                {
                    OnItemSelected(item.toggle);
                });
            }
        }

        isShowing = true;
        //shownOptionItem.SetContent(options[value]);
    }

    public void OnClickDropdown()
    {
        if (isShowing)
        {
            Hide();
        }
        else
        {
            if (dropdownGroup != null)
            {
                dropdownGroup.CloseOtherDropDown(this);
            }
            Show();
        }
    }


    private void OnItemSelected(Toggle toggle)
    {
        int num = -1;
        Transform transform = toggle.transform;
        Transform parent = transform.parent;
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i) == transform)
            {
                num = i;
                break;
            }
        }
        if (num >= 0)
        {
            Value = num;
            Hide();
        }
    }

    private void Awake()
    {
        if (dropdownGroup != null)
        {
            dropdownGroup.Add(this);
        }
    }
}