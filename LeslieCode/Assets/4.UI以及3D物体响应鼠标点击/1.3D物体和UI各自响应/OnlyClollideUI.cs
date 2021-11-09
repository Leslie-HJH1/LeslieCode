using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnlyClollideUI : MonoBehaviour, IPointerClickHandler
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
    }

    
}
