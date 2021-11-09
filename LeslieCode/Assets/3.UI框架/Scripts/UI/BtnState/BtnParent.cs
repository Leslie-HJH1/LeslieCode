using UnityEngine;
using Util;
using DG.Tweening;
using Const;
using System.Collections.Generic;

namespace UIFrame
{
    public class BtnParent : MonoBehaviour     
    {
        public SelectedState SelectedState
        {
            set
            {
                ResetChildState();//先重置再选择
                if (value==SelectedState.SELECTED)
                {
                    childs[_childId].SelectedState = SelectedState.SELECTED;
                }
            }
        }

        public int Index { get; private set; }
        private List<SelectedBtn> childs;
        private int _childId;
        public int ChildCount
        {
            get { return transform.childCount; }
        }

        public void Init(int index)
        {
            Index = index;
            _childId = 0;
            childs = new List<SelectedBtn>();//初始化 没设置数量
            int childIndex = 0;
            SelectedBtn temp;
            foreach (Transform trans in transform)//挨个儿赋值事件 并添加到列表里
            {
                temp = trans.gameObject.AddComponent<SelectedBtn>();
                childs.Add(temp);//往里加即可
                temp.AddSelectActionListener(SelectButtonMouse);
                childIndex++;
            }
        }

        //监听用
        private void SelectButtonMouse(SelectedBtn btn)
        {
            _childId = btn.Index;//从儿子那里传出来的
            ResetChildState();
            btn.SelectedState = SelectedState.SELECTED;
        }

        public void SelectedDefault()
        {
            Selected(childs[0].transform);
        }

        private void Selected(Transform selected)
        {
            var btn = selected.GetComponentInChildren<SelectedBtn>();
            if (btn != null) 
            {
                btn.Selected();
            }
        }

        public void CancelSelected()
        {

        }

        public bool Left()
        {
            _childId--;
            if (_childId>=0)
            {
                Selected(childs[_childId].transform);
                return true;
            }
            else
            {
                _childId = 0;
                return false;
            }
        }

        public bool Right()
        {
            _childId++;
            if (_childId < ChildCount)
            {
                Selected(childs[_childId].transform);
                return true;
            }
            else
            {
                _childId = ChildCount-1;
                return false;
            }
        }

        public void ResetChild()
        {
            ResetChildState();
        }

        private void ResetChildState()
        {
            foreach (SelectedBtn child in childs)
            {
                child.SelectedState = SelectedState.UNSELECTED;
            }
        }

        public void SelectedButton()
        {
            childs[_childId].SelectedButton();
        }
    }
}
