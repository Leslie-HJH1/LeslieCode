using Const;
using System;
using UnityEngine;
using Util;

namespace UIFrame
{
    public abstract class UIEffectBase : MonoBehaviour     
    {
        protected Vector2 _defaultAncherPos = Vector2.zero;

        protected Action _onEnterComplete;//Enter()执行完后通知外面 用Action把外面的方法传进来，这样就可以在Enter()执行完的时候调用外面的方法
        protected Action _onExitComplete;
        public virtual void Enter()
        {
            if (_defaultAncherPos == Vector2.zero)
            {
                _defaultAncherPos = transform.RectTransform().anchoredPosition;//初始位置
            }
        }
        public abstract void Exit();
        
        public virtual void OnEnterComplete(Action enterAction)
        {
            //小知识：
            //1._onEnterComplete = enterAction; _onEnterComplete只代表enterAction方法
            //2._onEnterComplete += enterAction; _onEnterComplete代表所有+进来的方法 _onEnterComplete一执行 所有+的方法都会执行
            _onEnterComplete = enterAction;
        }

        public virtual void OnExitComplete(Action exitAction)
        {
            _onExitComplete = exitAction;
        }

        public abstract UiEffect GetEffectLevel();
    }
}
