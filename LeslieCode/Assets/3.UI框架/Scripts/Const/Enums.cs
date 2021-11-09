using UnityEngine;

namespace Const
{
    /// <summary>
    /// UI层级
    /// </summary>
    public enum UILayer
    {
        BASIC_UI,
        OVERLAY_UI,
        TOP_UI
    }
    /// <summary>
    /// UI运行状态
    /// </summary>
    public enum UIState
    {
        NORMAL,//在Init之前的准备状态 默认值
        INIT,
        SHOW,
        HIDE,
    }

    public enum UiId
    {
        MainMenu,
        StartGame,
        NewGameWarning,
        Loading
    }

    public enum UiEffect
    {
        VIEW_EFFECT,//界面动效
        OTHERS_EFFECT//其他动效 其他动效是拿来与小UI进出时发生交互的
    }

    public enum SelectedState
    {
        SELECTED,
        UNSELECTED
    }

    public enum UIAudioName
    {
        UI_bg,
        UI_click,
        UI_in,
        UI_logo_in,
        UI_logo_out,
        UI_out
    }

    public enum BgAudioName
    {
        Level_Bg
    }

    public enum DifficultLevel
    {
        NONE,
        EASY,
        NORMAL,
        HARD
    }

    public enum ComicsParentId
    {
        LeftComics,
        CurrentComics,
        RightComics
    }

    /// <summary>
    /// 游戏内UI名称
    /// </summary>
    public enum GameUIName
    {
        HumanSkill
    }

    
}
