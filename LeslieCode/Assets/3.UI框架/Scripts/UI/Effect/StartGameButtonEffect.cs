using Const;
using UnityEngine;
using Util;
using DG.Tweening;

namespace UIFrame
{
    public class StartGameButtonEffect : UIEffectBase     
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
            return UiEffect.VIEW_EFFECT;
        }

        
    }
}
