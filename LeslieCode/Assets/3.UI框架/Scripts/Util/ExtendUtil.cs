using Const;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Util
{
    /// <summary>
    /// ��չ����
    /// </summary>
    public static class ExtendUtil     
    {
        
        public static void AddBtnListener(this RectTransform rect,Action action)
        {
            var button = rect.GetComponent<Button>() ?? rect.gameObject.AddComponent<Button>();

            button.onClick.AddListener(()=>action());//AddListener������UnityAction ����ֱ��ת��Action ��Lambda���ʽ����
        }

        public static void AddBtnListener(this Transform rect, Action action)
        {
            var button = rect.GetComponent<Button>() ?? rect.gameObject.AddComponent<Button>();

            button.onClick.AddListener(() => action());
        }

        public static RectTransform RectTransform(this Transform transform)
        {
            var rect = transform.GetComponent<RectTransform>();

            if (rect!=null)
            {
                return rect;
            }
            else
            {
                Debug.LogError("can not find RectTransform");
                return null;
            }
        }

        public static Image Image(this Transform transform)
        {
            var img = transform.GetComponent<Image>();

            if (img != null)
            {
                return img;
            }
            else
            {
                Debug.LogError("can not find Image");
                return null;
            }
        }

        public static Button Button(this Transform transform)
        {
            var btn = transform.GetComponent<Button>();

            if (btn != null)
            {
                return btn;
            }
            else
            {
                Debug.LogError("can not find Button");
                return null;
            }
        }

        public static Transform GetBtnParent(this Transform transform)
        {
            var parent = transform.Find(ConstValue.BUTTON_PARENT_NAME);
            if (parent == null)
            {
                Debug.LogError("can not find ButtonParent name:" + ConstValue.BUTTON_PARENT_NAME);
                return null;
            }
            else
            {
                return parent;
            }
        }

        public static void AddBtnListener(this Transform transform, string btnName, Action callBack)
        {
            var buttonTrans = transform.Find(ConstValue.BUTTON_PARENT_NAME + "/" + btnName);
            if (buttonTrans == null)
            {
                Debug.LogError("can not find button, name:" + btnName);
            }
            else
            {
                buttonTrans.AddBtnListener(callBack);
            }
        }

        public static T GetOrAddComponent<T>(this Transform transform) where T :Component
        {
            var component = transform.GetComponent<T>();
            if (component==null)
            {
                return transform.gameObject.AddComponent<T>();
            }
            else
            {
                return component;
            }
        }

        public static Transform GetByName(this Transform transform, string name)
        {
            var temp = transform.Find(name);
            if (temp == null)
            {
                Debug.LogError("can not find " + name + " under the parent:" + transform.name);
                return null;
            }
            else
            {
                return temp;
            }
        }
    }
}
