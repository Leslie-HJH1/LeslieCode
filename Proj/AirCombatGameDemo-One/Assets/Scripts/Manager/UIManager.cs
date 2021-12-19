using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class UIManager : NormalSingleton<UIManager>
{
    private readonly HashSet<string> _skipViews = new HashSet<string>
    {
        Paths.PREFAB_LOADING_VIEW
    };
    private Stack<string> _uiStack = new Stack<string>();
    private Dictionary<string,IView> _views = new Dictionary<string, IView>();
    private Canvas _canvas;
    private IView _dialog;
    
    public Canvas Canvas
    {
        get
        {
            if (_canvas == null)
                _canvas = Object.FindObjectOfType<Canvas>();
            
            if(_canvas == null)
                Debug.LogError("场景中没有Canvas");

            return _canvas;
        }
    }

    public IView Show(string path)
    {
        if (_uiStack.Count > 0)
        {
            //取出来关闭
            string name = _uiStack.Peek();
            //_views[name].Hide();
            if (GetLayer(name) >= GetLayer(path)) HideAll(_views[name]);
        }

        //得到要显示的界面
        IView view = InitView(path);

        //显示出来
        ShowAll(view);

        //加入栈中
        if (!_skipViews.Contains(path))
            _uiStack.Push(path);
        
        _views[path] = view;//存起来

        return view;
    }

    private UILayer GetLayer(string path)
    {
        return UILayerMgr._single.GetLayer(path);
    }

    public void Back()
    {
        if(_uiStack.Count <= 1)
            return;

        if (_dialog == null)//不存在弹出框
        {
            string name = _uiStack.Pop();
            HideAll(_views[name]);

            name = _uiStack.Peek();
            ShowAll(_views[name]);
        }
        else//存在弹出框
        {
            _dialog.Hide();
            _dialog = null;
            _views[_uiStack.Peek()].Show();
        }
        
    }

    public void Hide(string name)
    {
        HideAll(_views[name]);
    }

    private void ShowAll(IView view)
    {
        foreach (IShow show in view.GetTrans().GetComponents<IShow>())
        {
            show.Show();
        }
    }
    
    private void HideAll(IView view)
    {
        foreach (IHide hide in view.GetTrans().GetComponents<IHide>())
        {
            hide.Hide();
        }
    }
    
    public DialogView ShowDialog(string content,Action trueAction = null,Action falseAcion = null)
    {
        var dialogGo = LoadMgr.Single.LoadPrefab(Paths.PREFAB_DIALOG, Canvas.transform);
        AddTypeComponent(dialogGo,Paths.PREFAB_DIALOG);

        DialogView dialog = dialogGo.GetComponent<DialogView>();
        if (dialog != null)
        {
            dialog.InitDialog(content,trueAction,falseAcion);
        }

        _dialog = dialog;
        return dialog;
    }

    private IView InitView(string path)
    {
        if (_views.ContainsKey(path))
        {
            return _views[path];
        }
        else
        {
            var viewGo = LoadMgr.Single.LoadPrefabAndInstantiate(path, Canvas.transform);

            InitLayer(path, viewGo.transform);

            AddTypeComponent(viewGo, path);

            AddUpdateListener(viewGo);

            InitComponent(viewGo);

            IView view = viewGo.GetComponent<IView>();
            
            return view;
        }
    }

    private void InitLayer(string path, Transform view)
    {
        UILayerMgr.Single.SetParent(path, view);
    }

    private void AddTypeComponent(GameObject viewGo,string path)
    {
        foreach (var type in BindUtil.GetType(path))
        {
            viewGo.AddComponent(type);
        }
    }

    private void AddUpdateListener(GameObject viewGo)
    {
        var controller = viewGo.GetComponent<IController>();
        if (controller == null)
        {
            Debug.LogWarning("当前物体没有IController组件，物体名称:"+viewGo.name);
            return;
        }

        foreach (IUpdate update in viewGo.GetComponents<IUpdate>())
        {
            controller.AddUpdateListener(update.UpdateFun);
        }
    }

    private void InitComponent(GameObject viewGo)
    {
        IInit[] inits = viewGo.GetComponents<IInit>();

        foreach (var init in inits)
        {
            init.Init();
        }
    }

    
}
