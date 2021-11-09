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

        //���ò���ʾ
        public void Show(Transform showUI)
        {
            //Debug.Log(showUI);
            ResetBtnState();//�����õ�ǰ��������а�ť״̬
            ResetData();
            _currentParents = showUI.GetComponentsInChildren<BtnParent>(true).ToList();
            SetDefaultBtn(_currentParents);
        }

        private void ResetData()
        {
            _parentId = 0;
            _currentParents.Clear();//���һ�� ��ֹ��һ��ҳ�������
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

        //Ŀǰbool��ûʲô�� ����ֻ�����ڵݹ�
        private bool MoveIndex(Func<bool> moveAction,int symbol)
        {
            if (JudgException(moveAction, symbol))//�����ж�
                return false;

            if (_parentId >= 0 && _parentId < _currentParents.Count) //����ť��Id����Ҫ��
            {
                if (moveAction())//�����ƶ� ��ǰ�鼤��
                {
                    _currentParents[_parentId].SelectedState = SelectedState.SELECTED;
                    return true;
                }
                else//�����ƶ� ������һ��
                {
                    _currentParents[_parentId].SelectedState = SelectedState.UNSELECTED;
                    _parentId +=symbol;//��һ�鰴ť
                    return MoveIndex(moveAction, symbol);
                }
            }
            else//����ť��Id������
            {
                ResetParentId();//����һ��
                _currentParents[_parentId].SelectedState = SelectedState.SELECTED;//��һ����Ϊѡ��״̬
                return true;
            }
        }

        public void SelectedButton()
        {
            _currentParents[_parentId].SelectedButton();
        }

        //�����ж�
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
