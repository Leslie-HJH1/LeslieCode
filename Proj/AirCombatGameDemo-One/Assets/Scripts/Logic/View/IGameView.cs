using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameView : IInit
{
    GameLayer Layer { get; }
    Transform Self { get; }
}

public interface IGameRoot
{
    Transform Self { get; }
}

public abstract class GameViewBase : MonoBehaviour, IGameView
{
    private SubMsgMgr _msgMgr;

    protected SubMsgMgr MsgMgr
    {
        get
        {
            if (_msgMgr == null)
                _msgMgr = GameUtil.GetSubMsgMgr(transform);

            return _msgMgr;
        }
    }

    public virtual void Init()
    {
        InitComponent();
    }

    public abstract GameLayer Layer { get; }

    public Transform Self
    {
        get { return transform; }
    }

    protected virtual void InitComponent()
    {
    }

    protected virtual void OnEnable()
    {
        if (GameLayerMgr.Single != null)//自动设置层级
            GameLayerMgr.Single.SetLayerParent(this);

        Init();
    }

    public void SetPos(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }
}

public class BGView : GameViewBase, IGameRoot
{
    public override GameLayer Layer => GameLayer.BACKGROUND;
}

public class PlaneView : GameViewBase, IGameRoot
{
    public override GameLayer Layer => GameLayer.PLANE;
}

public class EffectView : GameViewBase, IGameRoot
{
    public override GameLayer Layer => GameLayer.EFFECT;
}



