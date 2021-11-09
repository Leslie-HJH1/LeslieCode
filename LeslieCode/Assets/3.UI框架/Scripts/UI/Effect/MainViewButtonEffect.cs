using Const;
using DG.Tweening;
using UnityEngine;
using Util;

namespace UIFrame
{
    public class MainViewButtonEffect : UIEffectBase
    {
        public override void Enter()
        {
            base.Enter();
            transform.RectTransform().DOKill();//杀掉之前动画 重新执行 防止出现两种动画交叉执行 出现错误预期
            transform.RectTransform().DOAnchorPos(Vector2.down * 324, 1);
        }

        public override void Exit()
        {
            transform.RectTransform().DOKill();
            transform.RectTransform().DOAnchorPos(_defaultAncherPos, 1);
        }

        public override UiEffect GetEffectLevel()
        {
            return UiEffect.VIEW_EFFECT;
        }


    }
}
