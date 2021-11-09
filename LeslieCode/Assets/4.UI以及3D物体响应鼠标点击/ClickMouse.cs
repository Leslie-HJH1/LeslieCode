using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 解决UI和鼠标点击之间的响应问题
/// 解决不点击UI时 物体响应点击事件
/// 点击了UI时 物体不响应点击事件
/// </summary>
public class ClickMouse : MonoBehaviour
{
    private int _index;
    private GraphicRaycaster _raycaster;//UI专用 所以能够检测到是不是点了UI
    // Start is called before the first frame update
    void Start()
    {
        _raycaster = FindObjectOfType<GraphicRaycaster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsUI())
        {
            ChangeColor();
        }
    }

    void ChangeColor()
    {
        if (_index == 0)
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);
        }
        else
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
        }
        _index = _index == 0 ? 1 : 0;
    }

    private bool IsUI()
    {
        PointerEventData data = new PointerEventData(EventSystem.current);
        data.pressPosition = Input.mousePosition;
        data.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        _raycaster.Raycast(data, results);
        return results.Count > 0;
    }

}
