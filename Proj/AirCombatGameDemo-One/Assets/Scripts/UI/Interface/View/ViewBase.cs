﻿using System.Collections.Generic;
using UnityEngine;

public abstract class ViewBase : MonoBehaviour,IView
{
    private UiUtil _util;
    private List<IViewUpdate> _viewUpdates;
    private List<IViewInit> _viewInits;
    private List<IViewShow> _viewShows;
    private List<IViewHide> _viewHides;


    protected UiUtil Util
    {
        get
        {
            if (_util == null)
            {
                _util = gameObject.AddComponent<UiUtil>();
                _util.Init();
            }

            return _util;
        }
    }
    
    public virtual void Init()
    {
        InitChild();//先初始化子物体
        InitSubView();//再把子物体加入列表
        
        //子项的Init
        InitAllSubView();
    }

    //初始化子物体
    protected abstract void InitChild();

    private void InitSubView()
    {
        _viewInits = new List<IViewInit>();
        _viewShows = new List<IViewShow>();
        _viewHides = new List<IViewHide>();
        _viewUpdates = new List<IViewUpdate>();

        InitInterface();
    }

    //获取所有接口
    //调用此方法 不用写<T> 因为传进去的List<T>已经有T了 所以自动识别出来
    private void InitViewInterface<T>(List<T> list)
    {
        T view;
        foreach (Transform trans in transform)
        {
            view = trans.GetComponent<T>();
            if (view != null)
                list.Add(view);
        }
    }

    private void InitAllSubView()
    {
        foreach (var view in _viewInits)
        {
            view.Init();
        }
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        foreach (var view in _viewShows)
        {
            view.Show();
        }

        UpdateFun();
    }

    public virtual void Hide()
    {
        //先执行逻辑 再隐藏
        foreach (var view in _viewHides)
        {
            view.Hide();
        }
        gameObject.SetActive(false);
    }

    public virtual void UpdateFun()
    {
        foreach (IViewUpdate update in _viewUpdates)
        {
            update.UpdateFun();
        }
    }

    public Transform GetTrans()
    {
        return transform;
    }

    private void InitInterface()
    {
        InitViewInterface(_viewInits);
        InitViewInterface(_viewShows);
        InitViewInterface(_viewHides);
        InitViewInterface(_viewUpdates);
    }

    public void Reacquire()
    {
        InitInterface();
        InitAllSubView();
    }
}
