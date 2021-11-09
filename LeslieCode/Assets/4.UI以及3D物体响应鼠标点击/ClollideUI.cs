using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClollideUI : MonoBehaviour,IPointerClickHandler
{
    private int _index;

    public void ChangeColor()
    {
        if (_index == 0)
        {
            GetComponent<Image>().color = Color.blue;
        }
        else
        {
            GetComponent<Image>().color = Color.white;
        }
        _index = _index == 0 ? 1 : 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ChangeColor();
        ExecuteAll(eventData);
    }

    /// <summary>
    /// 解决UI和3D物体同时响应问题
    /// </summary>
    /// <param name="eventData"></param>
    public void ExecuteAll(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();

        //获取所有射线碰到的对象 包括自己
        EventSystem.current.RaycastAll(eventData, results);
        foreach (RaycastResult result in results)
        {
            //剔除自己
            if (result.gameObject != gameObject)
            {
                //ExecuteEvents是EventSystems定义的委托 调用想要的对应方法即可
                ExecuteEvents.Execute(result.gameObject, eventData, ExecuteEvents.pointerClickHandler);
            }
        }
    }
}
