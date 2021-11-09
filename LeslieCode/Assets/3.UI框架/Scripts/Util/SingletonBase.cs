using UnityEngine;
using Util;
using DG.Tweening;
using Const;

namespace UIFrame
{
    public class SingletonBase<T> where T:new()
    {
        public static T Single { get; protected set; } = new T();
    }
}
