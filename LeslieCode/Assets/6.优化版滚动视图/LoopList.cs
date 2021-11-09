using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopList : MonoBehaviour
{
    public float OffsetY;
    private float _itemHeight;
    private RectTransform _content;
    private List<LoopListItem> _items;
    private List<LoopListItemModel> _models;

    // Start is called before the first frame update
    void Start()
    {
        _items = new List<LoopListItem>();
        _models = new List<LoopListItemModel>();
        //模拟数据获取
        GetModel();

        _content = transform.Find("Viewport/Content").GetComponent<RectTransform>();
        GameObject itemPrefab = Resources.Load<GameObject>("LoopListItem");
        _itemHeight = itemPrefab.GetComponent<RectTransform>().rect.height;
        int num = GetShowItemNum(_itemHeight, OffsetY);
        SpwanItem(num, itemPrefab);
        SetContentSize();

        transform.GetComponent<ScrollRect>().onValueChanged.AddListener(ValueChange);//监听scrollrect滑动时的变化
    }

    private void ValueChange(Vector2 data)
    {
        foreach (LoopListItem item in _items)//生成后 每个单位都要监听
        {
            item.OnValueChange();
        }
    }

    private int GetShowItemNum(float itemHeight, float offset)
    {
        float height = GetComponent<RectTransform>().rect.height;

        return Mathf.CeilToInt(height / (itemHeight + offset)) + 1;
    }

    private void SpwanItem(int num, GameObject itemPrefab)
    {
        GameObject temp = null;
        LoopListItem itemTemp = null;
        for (int i = 0; i < num; i++)
        {
            temp = Instantiate(itemPrefab, _content);
            itemTemp = temp.AddComponent<LoopListItem>();
            itemTemp.AddGetDataListener(GetData);
            itemTemp.Init(i,OffsetY,num);//初始化
            //itemTemp.Init(i, OffsetY, num);
            _items.Add(itemTemp);
        }
    }

    private LoopListItemModel GetData(int index)
    {
        if (index < 0 || index >= _models.Count)
            return new LoopListItemModel();

        return _models[index];
    }

    //产生数据
    private void GetModel()
    {
        foreach (var sprite in Resources.LoadAll<Sprite>("Icon"))
        {
            _models.Add(new LoopListItemModel(sprite, sprite.name));
        }
    }

    /// <summary>
    /// 区域总高度
    /// </summary>
    private void SetContentSize()
    {
        float y = _models.Count * _itemHeight + (_models.Count - 1) * OffsetY;
        _content.sizeDelta = new Vector2(_content.sizeDelta.x, y);
    }
}
