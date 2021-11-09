using UnityEngine;
using Util;
using DG.Tweening;
using Const;
using UnityEngine.EventSystems;
using System;

namespace UIFrame
{
    /// <summary>
    /// 挂载在任意Button上
    /// </summary>
    public class SelectedBtn : MonoBehaviour,IPointerEnterHandler     
    {
        public SelectedState SelectedState
        {
            set
            {
                switch (value)
                {
                    case SelectedState.SELECTED:
                        Selected();
                        break;
                    case SelectedState.UNSELECTED:
                        CancelSelected();
                        break;
                }
            }
        }
        
        private Color _defaultColor;

        public int Index { get { return transform.GetSiblingIndex(); } }//GetSiblingIndex当前父物体下所处的位置下标

        private Action<SelectedBtn> _selectAction;//存了点击后 点击的按钮的状态变化 下标等等

        private void Awake()
        {
            SaveDefaultColor(transform);
        }

        public void AddSelectActionListener(Action<SelectedBtn> action)
        {
            _selectAction = action;
        }

        public void Selected()
        {
            if (!JudgeException(transform))//按钮 图片都有 才出现按钮效果
            {
                PlayEffect(transform);
            }
        }

        public void CancelSelected()
        {
            KillEffect(transform);
        }

        public void SelectedButton()
        {
            transform.Button().onClick.Invoke();
        }

        public void KillEffect(Transform btn)
        {
            if (btn == null)
                return;

            btn.Image().DOKill();
            btn.Image().color = _defaultColor;
        }

        private bool JudgeException(Transform btn)
        {
            return btn.Button() == null || btn.Image() == null;
        }

        private void SaveDefaultColor(Transform btn)
        {
            _defaultColor= btn.Image().color;
        }

        private void PlayEffect(Transform btn)
        {
            btn.Image().DOColor(new Color32(154, 170, 255, 255), 1).SetLoops(-1, LoopType.Yoyo);


        }

        public void OnPointerEnter(PointerEventData eventData)
        {

            _selectAction?.Invoke(this);//事件之前添加了 这里只需要通过按钮点击来唤醒

        }
    }
}
