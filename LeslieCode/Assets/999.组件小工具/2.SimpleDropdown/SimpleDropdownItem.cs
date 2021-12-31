using UnityEngine;using UnityEngine.UI;


public class SimpleDropDownItemData
{
    public string text;
    public string imageName;

    public SimpleDropDownItemData(SimpleDropDownItemData data)
    {
        text = data.text;
        imageName = data.imageName;
    }

    public SimpleDropDownItemData(string text, string imageName)
    {
        this.text = text;
        this.imageName = imageName;
    }

    public SimpleDropDownItemData(string text)
    {
        this.text = text;
        this.imageName = "";
    }
}

public class SimpleDropdownItem : MonoBehaviour
{
    public Text text;
    public Image image;
    public Toggle toggle;
    public GameObject highLight;

    public void SetContent(string text, string imageName, bool needHighLight = false)
    {
        if (text != null)
        {
            this.text.text = text;
        }
        if (image != null)
        {
            if (string.IsNullOrEmpty(imageName))
            {
                image.enabled = false;
            }
            else
            {
                image.name = imageName;
            }
        }
        if (needHighLight && highLight != null)
        {
            highLight.SetActive(true);
        }
    }
}