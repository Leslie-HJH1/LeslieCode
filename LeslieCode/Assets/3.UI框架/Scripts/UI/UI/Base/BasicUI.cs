using System;
using Const;
using UnityEngine;

namespace UIFrame
{
    public abstract class BasicUI : UIBase
    {
        //֮ǰ�õ�Init ��UIManager�����Show�����е��� InitUi����Ҫ�Ȼ��UI�㼶 ����Ϊ�˷�ֹ�Ⱥ�˳�������
        //ֱ�ӷŵ�GetUiLayer�� ���ó�ʼ����
        public override UILayer GetUiLayer()
        {
            return UILayer.BASIC_UI;
        }
    }
}
