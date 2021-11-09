using Const;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace UIFrame
{
    public class UILayerManager : MonoBehaviour     
    {
        private readonly Dictionary<UILayer, Transform> _layerDictionary = new Dictionary<UILayer, Transform>();

        private void Awake()         
        {
            Transform temp = null;
            foreach (UILayer item in Enum.GetValues(typeof(UILayer)))
            {
                temp = transform.Find(item.ToString());
                if (temp==null)
                {
                    Debug.LogError("can not find Layer:" + item + " GameObject");
                    continue;//��Ȼifelse����û���߼��� ���ǻ���д��ȥ ������ֹ�Ժ��д����
                }
                else
                {
                    _layerDictionary[item] = temp;
                }
            }
        }

        public Transform GetLayerObject(UILayer layer)
        {
            if (_layerDictionary.ContainsKey(layer))
            {
                return _layerDictionary[layer];
            }
            else
            {
                Debug.LogError("_layerDictionary did not contains layer:" + layer);
                return null;
            }
        }

        //private void 
    }
}
