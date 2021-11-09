using UnityEngine;
using Util;
using DG.Tweening;
using Const;
using System.Collections.Generic;
using System.Linq;
using System;

namespace UIFrame
{
    public class BtnStateManager : MonoBehaviour
    {
        private Transform _lastBtn;
        private int _parentId;
        private List<BtnParent> _currentParents=new List<BtnParent>();

        public void InitBtnParent(List<Transform> parents)
        {
            if (parents == null)
                return;
            BtnParent temp;
            for (int i = 0; i < parents.Count; i++)
            {
                temp = parents[i].gameObject.AddComponent<BtnParent>();
                temp.Init(i);
            }
        }

        private bool JudgeException(List<Transform> parents)
        {
            return parents == null || parents.Count == 0;
        }

        public void SetDefaultBtn(List<BtnParent> parents)
        {
            foreach (BtnParent parent in parents)
            {
                if (parent.Index==0)
                {
                    parent.SelectedDefault();
                }
            }
        }

        //重置并显示
        public void Show(Transform showUI)
        {
            //Debug.Log(showUI);
            ResetBtnState();//先重置当前界面的所有按钮状态
            ResetData();
            _currentParents = showUI.GetComponentsInChildren<BtnParent>(true).ToList();
            SetDefaultBtn(_currentParents);
        }

        private void ResetData()
        {
            _parentId = 0;
            _currentParents.Clear();//清除一下 防止上一层页面的数据
        }

        private void ResetBtnState()
        {
            foreach (BtnParent btnParent in _currentParents)
            {
                btnParent.ResetChild();
            }
        }

        public void Left()
        {
            MoveIndex(_currentParents[_parentId].Left, -1);
        }

        public void Right()
        {
            MoveIndex(_currentParents[_parentId].Right, 1);
        }

        //目前bool还没什么用 可能只是用于递归
        private bool MoveIndex(Func<bool> moveAction,int symbol)
        {
            if (JudgException(moveAction, symbol))//错误判断
                return false;

            if (_parentId >= 0 && _parentId < _currentParents.Count) //父按钮组Id符合要求
            {
                if (moveAction())//可以移动 当前组激活
                {
                    _currentParents[_parentId].SelectedState = SelectedState.SELECTED;
                    return true;
                }
                else//不可移动 进入下一组
                {
                    _currentParents[_parentId].SelectedState = SelectedState.UNSELECTED;
                    _parentId +=symbol;//下一组按钮
                    return MoveIndex(moveAction, symbol);
                }
            }
            else//父按钮组Id超界限
            {
                ResetParentId();//重置一下
                _currentParents[_parentId].SelectedState = SelectedState.SELECTED;//下一组设为选中状态
                return true;
            }
        }

        public void SelectedButton()
        {
            _currentParents[_parentId].SelectedButton();
        }

        //错误判断
        private bool JudgException(Func<bool> moveAction,int symbol)
        {
            if (moveAction==null)
            {
                Debug.LogError("moveAction is null");
                return true;
            }

            if (symbol!=1 && symbol!=-1)
            {
                Debug.LogError("symbol must be 1 or -1");
                return true;
            }

            return false;
        }

        private void ResetParentId()
        {
            if (_parentId<0)
            {
                _parentId = 0;
                return;
            }
            else if(_parentId>=_currentParents.Count)
            {
                _parentId = _currentParents.Count-1;
            }
        }
    }
}
