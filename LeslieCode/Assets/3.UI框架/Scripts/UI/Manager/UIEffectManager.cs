using Const;
using System;
using UnityEngine;

namespace UIFrame
{
    public class UIEffectManager : MonoBehaviour     
    {
        public void Show(Transform ui)
        {
            if (ui == null)
                return;
            foreach (UIEffectBase effectBase in ui.GetComponentsInChildren<UIEffectBase>(true))//������true ����Ҳ���Եõ��������ص�����
            {
                effectBase.Enter();
            }
        }

        public void Hide(Transform ui)
        {
            if (ui == null)
                return;
            foreach (UIEffectBase effectBase in ui.GetComponentsInChildren<UIEffectBase>(true))//������true ����Ҳ���Եõ��������ص�����
            {
                effectBase.Exit();
            }
        }

        //�ر���������
        //СUI��ʾ��ʱ����
        public void HideOthersEffect(Transform ui)
        {
            foreach (UIEffectBase effectBase in ui.GetComponentsInChildren<UIEffectBase>(true))//������true ����Ҳ���Եõ��������ص�����
            {
                if (effectBase.GetEffectLevel() == UiEffect.OTHERS_EFFECT)
                    effectBase.Exit();
            }
        }

        //������ʾ��������
        //СUI�رյ�ʱ����
        public void ShowOthersEffect(Transform ui)
        {
            foreach (UIEffectBase effectBase in ui.GetComponentsInChildren<UIEffectBase>(true))//������true ����Ҳ���Եõ��������ص�����
            {
                if (effectBase.GetEffectLevel() == UiEffect.OTHERS_EFFECT)
                    effectBase.Enter();
            }
        }

        public void AddViewEffectEnterListener(Transform ui,Action enterComplete)
        {
            foreach (UIEffectBase effectBase in ui.GetComponentsInChildren<UIEffectBase>(true))
            {
                if (effectBase.GetEffectLevel() == UiEffect.VIEW_EFFECT)
                {
                    effectBase.OnEnterComplete(enterComplete);
                }
            }
        }

        public void AddViewEffectExitListener(Transform ui, Action exitComplete)
        {
            foreach (UIEffectBase effectBase in ui.GetComponentsInChildren<UIEffectBase>(true))
            {
                if (effectBase.GetEffectLevel() == UiEffect.VIEW_EFFECT)
                {
                    effectBase.OnExitComplete(exitComplete);
                }
            }
        }
    }
}
