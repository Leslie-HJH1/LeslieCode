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
                    continue;//虽然ifelse后面没有逻辑了 但是还是写上去 这样防止以后会写东西
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
