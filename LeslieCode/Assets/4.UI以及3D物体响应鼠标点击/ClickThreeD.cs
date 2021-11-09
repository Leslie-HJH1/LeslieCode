using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 相机上挂一个 Physics Raycaster
/// </summary>
public class ClickThreeD : MonoBehaviour, IPointerClickHandler
{
    private int _index;

    public void OnPointerClick(PointerEventData eventData)
    {
        ChangeColor();
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

    
}
