using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class ControllerBase : MonoBehaviour,IController
{
    private List<IControllerUpdate> _updates;
    private List<IControllerInit> _inits;
    private List<IControllerShow> _shows;
    private List<IControllerHide> _hides;
    private Action _onUpdate;
    
    public virtual void Init()
    {
        //先初始化好各个部分 比如按钮的监听事件 即Controller
        InitChild();
        InitAllComponents();
        InitComponents();
        //再添加各个部分的更新函数 即View
        AddUpdateAction();
    }

    protected abstract void InitChild();
    
    private void AddUpdateAction()
    {
        foreach (Button button in GetComponentsInChildren<Button>())
        {
            button.onClick.AddListener(()=>
            {
                if(_onUpdate != null)
                    _onUpdate();

                UpdateFun();
            });
        }
    }
    
    public void Reacquire()
    {
        InitInterface();
        InitComponents();
    }

    private void InitInterface()
    {
        InitComponent(_inits, this);
        InitComponent(_shows, this);
        InitComponent(_hides, this);
        InitComponent(_updates, this);
    }

    private void InitAllComponents()
    {
        _inits = new List<IControllerInit>();
        _shows = new List<IControllerShow>();
        _hides = new List<IControllerHide>();
        _updates = new List<IControllerUpdate>();

        InitInterface();
    }

    //参数components 与 外面传进来的地址为同一个地址
    //所以可以在方法内 可以改变引用类型的内容
    //注意：对于引用类型 你可以对其操作 但是不能重新初始化
    private void InitComponent<T>(List<T> components,T removeObject)
    {
        var temp = transform.GetComponentsInChildren<T>();
        
        components.AddRange(temp);
        
        components.Remove(removeObject);
    }

    private void InitComponents()
    {
        foreach (var component in _inits)
        {
            component.Init();
        }
    }

    public virtual void Show()
    {
        foreach (var component in _shows)
        {
            component.Show();
        }
    }

    public virtual void Hide()
    {
        foreach (var component in _hides)
        {
            component.Hide();
        }
    }

    public virtual void UpdateFun()
    {
        foreach (var component in _updates)
        {
            component.UpdateFun();
        }
    }

    public void AddUpdateListener(Action update)
    {
        _onUpdate += update;
    }
}
