﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCycleMgr : MonoSingleton<LifeCycleMgr>, IInit
{
    //接口显式实现
    void IInit.Init()
    {
        LifeCycleAddConfig config = new LifeCycleAddConfig();
        config.Init();
        InitObject(config);

        LifeCycleConfig.LifeCycleFuns[LifeName.INIT]();
    }

    private void InitObject(LifeCycleAddConfig config)
    {
        foreach (object o in config.Objects)
        {
            foreach (var cycle in LifeCycleConfig.LifeCycles)
            {
                if (cycle.Value.Add(o))
                {
                    break;//进入下一个循环
                }
            }
        }
    }

    public void Add(LifeName name,object o)
    {
        LifeCycleConfig.LifeCycles[name].Add(o);
    }

    public void Remove(LifeName name,object o)
    {
        LifeCycleConfig.LifeCycles[name].Remove(o);
    }

    public void RemoveAll(object o)
    {
        foreach (var cycle in LifeCycleConfig.LifeCycles)
        {
            cycle.Value.Remove(o);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameStateModel.Single.Pause)
            return;
        LifeCycleConfig.LifeCycleFuns[LifeName.UPDATE]();
    }
}

public interface ILifeCycle
{
    bool Add(object o);//直接返回是否添加成功
    void Remove(object o);
    void Execute<T>(Action<T> execute);
}

public class LifeCycle<T> : ILifeCycle
{
    private HashSet<object> _objects = new HashSet<object>();
    private HashSet<object> _removeObjects = new HashSet<object>();

    public bool Add(object o)
    {
        if (o is T)
        {
            _objects.Add(o);
            return true;
        }

        return false;
    }

    public void Remove(object o)
    {
        _removeObjects.Add(o);
    }

    public void Execute<T1>(Action<T1> execute)
    {
        foreach (var o in _objects)
        {
            if (o == null)
                _removeObjects.Add(o);
            execute((T1)o);
        }

        foreach (object o in _removeObjects)
        {
            _objects.Remove(o);
        }
        
        _removeObjects.Clear();
    }
}