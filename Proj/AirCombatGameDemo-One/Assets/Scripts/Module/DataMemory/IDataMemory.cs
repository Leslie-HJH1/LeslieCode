using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataMemory
{
    //获取数据
    T Get<T>(string key);
    //设置数据
    void Set<T>(string key, T value);
    //清楚某个数据
    void Clear(string key);
    //清楚全部数据
    void ClearAll();
    
    bool Contains(string key);

    void SetObject(string key, object value);
    
    object GetObject(string key);
}
