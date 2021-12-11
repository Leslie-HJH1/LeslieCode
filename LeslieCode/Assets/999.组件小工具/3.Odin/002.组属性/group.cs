using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class group : MonoBehaviour
{
    [FoldoutGroup("Buttons in Boxes")]//定义一个折叠组
    [HorizontalGroup("Buttons in Boxes/Horizontal", Width = 60)]//定义一个横向组

    [Button(ButtonSizes.Large)] 
    [BoxGroup("Buttons in Boxes/Horizontal/One")]
    public void Button1() { }

    [Button(ButtonSizes.Large)]
    [BoxGroup("Buttons in Boxes/Horizontal/Two")]
    public void Button2() { }

    [Button]
    [BoxGroup("Buttons in Boxes/Horizontal/Double")]
    public void Accept() { }

    [Button]
    [BoxGroup("Buttons in Boxes/Horizontal/Double")]
    public void Cancel() { }
    
    
}
