using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OdinValueDrawersForClasses
{
    public class MyClass : MonoBehaviour
    {
        public MySubClass mySubClass;
    }

    [System.Serializable]
    public class MySubClass
    {
        public string text;
        public int number;
        public Vector3 location;
    }
}

