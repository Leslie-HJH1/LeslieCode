using UnityEngine;

//数据持久化
//ScriptableObject Unity 持久化类
namespace CustomTool
{
    [System.Serializable]//序列化
    public class NamespaceData : ScriptableObject
    {
        [SerializeField]
        public new string name;
        [SerializeField]
        public bool IsOn;
    }
}
