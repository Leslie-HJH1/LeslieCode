using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReader
{
    IReader this[string key] { get; }
    IReader this[int key] { get; }
    void Count(Action<int> callBack);
    
    //考虑到延时 用回调
    void Get<T>(Action<T> callBack);
    
    //设置当前数据
    void SetData(object data);
    ICollection<string> Keys();
}
