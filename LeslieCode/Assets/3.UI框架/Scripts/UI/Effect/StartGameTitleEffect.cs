using Const;
using Util;
using DG.Tweening;
using UnityEngine;

namespace UIFrame
{
    public class StartGameTitleEffect : UIEffectBase
    {
        public override void Enter()
        {
            base.Enter();

            transform.RectTransform().DOAnchorPosX(0, 1);
        }

        public override void Exit()
        {
            transform.RectTransform().DOAnchorPos(_defaultAncherPos, 1);
        }

        public override UiEffect GetEffectLevel()
        {
            return UiEffect.OTHERS_EFFECT;
        }
    }
}
