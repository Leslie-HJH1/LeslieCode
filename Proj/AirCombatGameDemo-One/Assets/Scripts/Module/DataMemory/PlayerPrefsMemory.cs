using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerPrefsMemory : IDataMemory
{
    //优化方法
    //由于GetInt GetString GetFloat三种不同方法 即需要返回这三种方法中的一种 供外部调用
    //所以直接创建 Func字典 来存储三种方法
    //这样直接调用时 返回的就是对应的方法 很完美
    //返回值不同 用object
    private Dictionary<Type, Func<string, object>> _dataGetter = new Dictionary<Type, Func<string, object>>()
    {
        {typeof(int), (key) => PlayerPrefs.GetInt(key, (int)_defaultValues[typeof(int)])},
        {typeof(string), (key) => PlayerPrefs.GetString(key, (string)_defaultValues[typeof(string)])},
        {typeof(float), (key) => PlayerPrefs.GetFloat(key, (float)_defaultValues[typeof(float)])},
    };
    
    //设置类型 根据参数填入即可
    //这里的Action就是存了三种方法
    //外界调用字典来调用对应方法
    private Dictionary<Type, Action<string, object>> _dataSetter = new Dictionary<Type, Action<string, object>>()
    {
        {typeof(int), (key,value) => PlayerPrefs.SetInt(key, (int)value)},
        {typeof(string), (key,value) => PlayerPrefs.SetString(key, (string)value)},
        {typeof(float), (key,value) => PlayerPrefs.SetFloat(key, (float)value)},
    };

    //默认值
    private static Dictionary<Type, object> _defaultValues = new Dictionary<Type, object>()
    {
        {typeof(int), default(int)},
        {typeof(string), ""},
        {typeof(float), default(float)},
    };
    
    public T Get<T>(string key)
    {
        //泛型类型转换
        Type type = typeof(T);
        var converter = TypeDescriptor.GetConverter(type);//转换器

        if (_dataGetter.ContainsKey(type))
        {
            return (T) converter.ConvertTo(_dataGetter[type](key), type);
        }
        else
        {
            Debug.LogError("当前数据存储中无此类型数据，类型名："+typeof(T).Name);
            return default(T);//返回当前类型的默认值
        }
    }

    public void Set<T>(string key, T value)
    {
        Type type = typeof(T);

        if (_dataSetter.ContainsKey(type))
        {
            _dataSetter[type](key, value);
        }
        else
        {
            Debug.LogError("当前数据存储中无此类型数据，数据为 key:"+key+" value:"+value);
        }
    }

    public void Clear(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public void ClearAll()
    {
        PlayerPrefs.DeleteAll();
    }

    public bool Contains(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public void SetObject(string key, object value)
    {
        bool success = false;
        foreach (KeyValuePair<Type,Action<string,object>> pair in _dataSetter)
        {
            if (value.GetType() == pair.Key)
            {
                pair.Value(key, value);
                success = true;
            }
        }

        if (!success)
        {
            Debug.LogError("未找到当前值的类型，赋值失败，value："+value);
        }
    }

    public object GetObject(string key)
    {
        if (Contains(key))
        {
            foreach (KeyValuePair<Type,Func<string,object>> pair in _dataGetter)
            {
                object value = pair.Value(key);
                if (!value.Equals(_defaultValues[pair.Key]))
                {
                    return value;
                }
            }
        }
        else
        {
            Debug.LogError("当前数据内不包含对于键值："+key);
        }

        return null;
    }
}
