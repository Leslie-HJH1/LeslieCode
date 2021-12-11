using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class UGUITools
{

    public static void SetGameObjectActiveEx(this Component comp, bool value)    {        if (comp != null)        {            var go = comp.gameObject;            if (go != null && go.activeSelf != value)            {                go.SetActive(value);            }        }    }

    public static void SetActiveEx(this GameObject go, bool value)    {        if (null != go && go.activeSelf != value)        {            go.SetActive(value);        }    }


    /// <summary>    /// Convenience extension that destroys all children of the transform.    /// </summary>
    static public void DestroyChildren(this Transform t)
    {
        bool isPlaying = Application.isPlaying;

        while (t.childCount != 0)
        {
            Transform child = t.GetChild(0);

            if (isPlaying)
            {
                child.SetParent(null);
                UnityEngine.Object.Destroy(child.gameObject);
            }
            else UnityEngine.Object.DestroyImmediate(child.gameObject);
        }
    }
}
