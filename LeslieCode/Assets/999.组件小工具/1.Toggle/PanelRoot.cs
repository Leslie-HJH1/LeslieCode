using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelRoot : MonoBehaviour
{
    public item curLevelItem;

    public TabSwitcher Tab;

    public item StrengthenPrefab;
    public item SpringPrefab;

    public Transform StrengthenContent;
    public Transform SpringContent;

    public GameObject StrengthenPanel;
    public GameObject SpringPanel;

    public List<item> _StrengthenItems = new List<item>();
    public List<item> _SpringItems = new List<item>();

    private void Start()
    {
        Tab.OnSwitchTabCallback = OnSwitch;
        Tab.SwitchTab(0);
    }

    private void OnSwitch(int index)
    {
        if (index == 0)
            SetStrengthenItem();
        if (index == 1)
            SetSpringItem();

        StrengthenPanel.SetActive(index == 0);
        SpringPanel.SetActive(index == 1);
    }

    private void SetStrengthenItem()
    {
        //StrengthenContent.transform.DestroyChildren();

        //初始化
        for (int i = 0; i < 10; i++)
        {
            //数量不够才实例化 否则 直接用原来的更改数据即可
            if (_StrengthenItems.Count<=i)
            {
                var item = Instantiate(StrengthenPrefab, StrengthenContent.transform);
                item.name = "Strengthen" + i;
                _StrengthenItems.Add(item);

                //里面只考虑点击后的逻辑变化
                item.transform.GetComponent<Toggle>().onValueChanged.AddListener(delegate (bool isOpen)//回调
                {
                    curLevelItem = item.GetComponent<item>();
                    //curLevelItem.frame.SetActiveEx(isOpen);
                });
                
                item.SetGameObjectActiveEx(true);
            }

            //初始化 把每个都先关闭（由于 只单独打开某一个 toggle组件不能自动把其他的关闭 所以要先全关闭）
            //ison 参数 如果全部都为false 那么toggle会主动自动打开一个（第一次创建的时候 遍历从第一个开始 所以打开第一个）
            //之后跳页面时 首先所有的ison都设为false 否则toggle组件会先跳到代码指定的那个 然后又跳回之前选择得那个 出现图像跳变
            //所以 每次进这个页面都先全部关闭 然后 代码指定要打开的那个
            _StrengthenItems[i].transform.GetComponent<Toggle>().isOn = false;

            //刷新数据
            //_StrengthenItems[i].setdata(data)
        }

        //切页面专用 打开第一个
        _StrengthenItems[0].transform.GetComponent<Toggle>().isOn = true;

    }

    private void SetSpringItem()
    {
        //SpringContent.transform.DestroyChildren();

        for (int i = 0; i < 10; i++)
        {
            if (_SpringItems.Count <= i)
            {
                var item = Instantiate(SpringPrefab, SpringContent.transform);
                item.name = "Spring" + i;
                _SpringItems.Add(item);

                item.transform.GetComponent<Toggle>().onValueChanged.AddListener(delegate (bool isOpen)//回调
                {
                    curLevelItem = item.GetComponent<item>();
                });
                
                item.SetGameObjectActiveEx(true);
            }

            _SpringItems[i].transform.GetComponent<Toggle>().isOn = false;
            
        }

        //切页面专用 打开第一个
        _SpringItems[0].transform.GetComponent<Toggle>().isOn = true;

    }
}
