using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace OdinValueDrawersForStructs
{
    public class MyClass : MonoBehaviour
    {
        [ShowInInspector]
        public DateTime myDataTime;
    }
}

