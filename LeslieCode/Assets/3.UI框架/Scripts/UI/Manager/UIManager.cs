using UnityEngine;
using System.Collections.Generic;
using Const;
using System;
using System.Threading.Tasks;
using Util;

namespace UIFrame
{
    public class UIManager : MonoBehaviour     
    {
        private Dictionary<UiId, GameObject> _prefabDictionary = new Dictionary<UiId, GameObject>();
        private Stack<UIBase> _uiStack = new Stack<UIBase>();
        private Func<UILayer,Transform> GetLayerObject;//这里存了_layerManager.GetLayerObject 的方法
        private Action<Transform> InitCallBack;//初始化的回调 每次产生新界面 或者 切换UI界面 都会重置InitBtnParent

        //Tuple<Transform, Transform> 1.要显示的  2.要隐藏的
        public Tuple<Transform, Transform> Show(UiId id)
        {
            GameObject ui = GetPrefabObject(id);//创建对象 或者 获取属性等操作 都要进行一次非空判断 这样有报错 检查起来方便 节省时间
            if (ui==null)
            {
                Debug.LogError("can not find prefab " + id);
                return null;
            }

            UIBase uiScript = GetUiScript(ui, id);
            if (uiScript == null)
                return null;

            InitUi(uiScript);

            Transform hideUI = null;
            if (uiScript.GetUiLayer()==UILayer.BASIC_UI)//大UI 覆盖当前层
            {
                uiScript.UiState = UIState.SHOW;
                hideUI = Hide();
            }
            else
            {
                uiScript.UiState = UIState.SHOW;//同样 如果不是BAsicUI 那么就表示显示的是小UI传出去为空
            }
            
            _uiStack.Push(uiScript);

            return new Tuple<Transform,Transform>(ui.transform,hideUI) ;//元组应用
        }

        //出现新的界面
        //隐藏当前层
        private Transform Hide()
        {
            if (_uiStack.Count!=0)
            {
                _uiStack.Peek().UiState = UIState.HIDE;
                return _uiStack.Peek().transform;
                //_effectManager.Hide(_uiStack.Peek().transform);
            }
            return null;
        }

        public Tuple<Transform, Transform> Back()
        {
            if (_uiStack.Count>1)
            {
                UIBase hideUI = _uiStack.Pop();//取出操作目标
                Transform showUI = null;
                if (hideUI.GetUiLayer()==UILayer.BASIC_UI)//是基础 则传出去 表示达到上个界面 不是基础 不用传出去 关掉的只是小UI
                {
                    hideUI.UiState = UIState.HIDE;
                    _uiStack.Peek().UiState = UIState.SHOW;
                    showUI = _uiStack.Peek().transform;
                    //_effectManager.Show(_uiStack.Peek().transform);//用元组传出去
                }
                else
                {
                    hideUI.UiState = UIState.HIDE;//如果不是BASIC_UI 那么showUI返回空
                }
                //_effectManager.Hide(hideUI.transform);

                return new Tuple<Transform, Transform>(showUI, hideUI.transform);
            }
            else
            {
                Debug.LogError("uistack has one or no element");
                return null;
            }            
        }

        public void AddGetLayerObjectListener(Func<UILayer, Transform> fun)
        {
            if (fun == null)//提前判断
            {
                Debug.LogError("GetLayerObject function can not be null");
                return;
            }
            GetLayerObject = fun;
        }

        public void AddInitCallBackListener(Action<Transform> callBack)
        {
            if (callBack == null)//提前判断
            {
                Debug.LogError("InitCallBack function can not be null");
                return;
            }
            InitCallBack = callBack;
        }

        private void InitUi(UIBase uiScript)
        {
            if (uiScript.UiState==UIState.NORMAL)//默认即Normal 只要刚创建的时候才初始化 其他都已经初始化好了
            {
                Transform ui = uiScript.transform;
                //根据层级信息，添加到对应的父物体
                ui.SetParent(GetLayerObject?.Invoke(uiScript.GetUiLayer()));//从layermanager里面获取对应层级的位置
                ui.localPosition = Vector3.zero;
                ui.localScale = Vector3.one;
                ui.RectTransform().offsetMax = Vector2.zero;//left right
                ui.RectTransform().offsetMin = Vector2.zero;//top bottom

                //生成单个UI界面后 添加
                //就调用 重置存到每组Ui都有BtnParent 存进去
                InitCallBack?.Invoke(ui);
            }
        }

        private GameObject GetPrefabObject(UiId id)
        {
            if (!_prefabDictionary.ContainsKey(id)|| _prefabDictionary[id]==null)
            {
                GameObject prefab= LoadManager.Single.Load<GameObject>(Path.UI_PATH, id.ToString());
                if (prefab!=null)
                {
                    _prefabDictionary[id] = Instantiate(prefab);
                }
                else
                {
                    Debug.LogError("can not find prefab:" + id);
                }
                
            }

            return _prefabDictionary[id];
        }

        private UIBase GetUiScript(GameObject prefab, UiId id)
        {
            UIBase ui = prefab.GetComponent<UIBase>();//未添加脚本

            if (ui==null)
            {
                return AddUiScript(prefab, id);
            }
            else
            {
                return ui;
            }
        }

        private UIBase AddUiScript(GameObject prefab,UiId id)
        {
            string scriptName = ConstValue.UI_NAMESPACE_NAME+"."+ id +ConstValue.UI_SCRIPT_POSTFIX;
            Type ui = Type.GetType(scriptName);
            if (ui==null)
            {
                Debug.LogError("can not find script:" + scriptName);
            }
            return prefab.AddComponent(ui) as UIBase;
        }

        public List<Transform> GetBtnParents(Transform showUI)
        {
            if (showUI!=null)
            {
                return showUI.GetComponent<UIBase>().GetBtnParents();
            }
            else
            {
                return null;
            }
            
        }

        //当前界面
        public Transform GetCurrentUITrans()
        {
            return _uiStack.Peek().transform;
        }

        //最近的BasicUI
        public Transform GetBasicUITrans()
        {
            var array = _uiStack.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].GetUiLayer()==UILayer.BASIC_UI)
                {
                    return array[i].transform;
                }
            }
            return null;//这里一般 是运行不到 最差 也会到MainMenuView
        }


    }
}
