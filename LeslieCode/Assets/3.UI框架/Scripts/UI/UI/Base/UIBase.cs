using System;
using System.Collections.Generic;
using Const;
using UnityEngine;

namespace UIFrame
{
    //ui基础类
    public abstract class UIBase : MonoBehaviour
    {
        private UIState _uiState = UIState.NORMAL;
        public UIState UiState
        {
            get { return _uiState; }
            set
            {
                HandleUiState(value);
                _uiState = value;//保存当前状态
            }
        }

        private void HandleUiState(UIState value)
        {
            switch (value)
            {
                case UIState.INIT://初始化
                    if (_uiState == UIState.NORMAL)
                    {
                        Init();
                    }
                    break;
                case UIState.SHOW://显示
                    if (_uiState == UIState.NORMAL)//准备状态
                    {
                        Init();//先初始化
                        Show();//再显示
                    }
                    else//非准备状态 直接显示
                    {
                        Show();
                    }
                    break;
                case UIState.HIDE:
                    Hide();
                    break;
            }
        }

        protected virtual void Init()
        {
        }

        protected virtual void Show()
        {
            
        }

        protected virtual void Hide()
        {
            
        }

        protected virtual void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public abstract UiId GetUiId();

        public abstract List<Transform> GetBtnParents();//这里保存的是 按钮们的父物体 比如：这个父物体保存了几个按钮 其他父物体也保存了按钮
        
        //public abstract Transform GetDefaultBtn();

        public abstract UILayer GetUiLayer();//直接 用这种方法返回层级 不用放在初始化中
    }
}
