﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DataMgr的方法其实与其Module中的方法相同 所以直接继承接口即可
/// </summary>
public class DataMgr : NormalSingleton<DataMgr>,IDataMemory
{
    private IDataMemory _dataMemory;

    public DataMgr()
    {
        _dataMemory = new PlayerPrefsMemory();
    }
    
    public T Get<T>(string key)
    {
        return _dataMemory.Get<T>(key);
    }

    public void Set<T>(string key, T value)
    {
        _dataMemory.Set(key,value);
    }

    public void Clear(string key)
    {
        _dataMemory.Clear(key);
    }

    public void ClearAll()
    {
        _dataMemory.ClearAll();
    }

    public bool Contains(string key)
    {
       return _dataMemory.Contains(key);
    }

    public void SetObject(string key, object value)
    {
        _dataMemory.SetObject(key,value);
    }

    public object GetObject(string key)
    {
        return _dataMemory.GetObject(key);
    }
}
