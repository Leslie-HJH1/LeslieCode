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
            foreach (UIEffectBase effectBase in ui.GetComponentsInChildren<UIEffectBase>(true))//传参数true 这样也可以得到所有隐藏的子项
            {
                effectBase.Enter();
            }
        }

        public void Hide(Transform ui)
        {
            if (ui == null)
                return;
            foreach (UIEffectBase effectBase in ui.GetComponentsInChildren<UIEffectBase>(true))//传参数true 这样也可以得到所有隐藏的子项
            {
                effectBase.Exit();
            }
        }

        //关闭其他动销
        //小UI显示的时候用
        public void HideOthersEffect(Transform ui)
        {
            foreach (UIEffectBase effectBase in ui.GetComponentsInChildren<UIEffectBase>(true))//传参数true 这样也可以得到所有隐藏的子项
            {
                if (effectBase.GetEffectLevel() == UiEffect.OTHERS_EFFECT)
                    effectBase.Exit();
            }
        }

        //重新显示其他动销
        //小UI关闭的时候用
        public void ShowOthersEffect(Transform ui)
        {
            foreach (UIEffectBase effectBase in ui.GetComponentsInChildren<UIEffectBase>(true))//传参数true 这样也可以得到所有隐藏的子项
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
