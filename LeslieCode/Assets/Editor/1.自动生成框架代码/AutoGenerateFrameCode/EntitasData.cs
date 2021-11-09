using UnityEngine;

namespace CustomTool
{
    [System.Serializable]
    public class EntitasData : ScriptableObject     
    {
        /// <summary>
        /// View��·��
        /// </summary>
        public string ViewPath;
        /// <summary>
        /// Service��·��
        /// </summary>
        public string ServicePath;
        /// <summary>
        /// System��·��
        /// </summary>
        public string SystemPath;
        /// <summary>
        /// ServiceManager·��
        /// </summary>
        public string ServiceManagerPath;
        /// <summary>
        /// ViewFeature�ű�·��
        /// </summary>
        public string ViewFeaturePath;
        /// <summary>
        /// InputFeature�ű�·��
        /// </summary>
        public string InputFeaturePath;
        /// <summary>
        /// GameFeature�ű�·��
        /// </summary>
        public string GameFeaturePath;
    }
}
