using UnityEngine;

namespace Const
{
    /// <summary>
    /// UI�㼶
    /// </summary>
    public enum UILayer
    {
        BASIC_UI,
        OVERLAY_UI,
        TOP_UI
    }
    /// <summary>
    /// UI����״̬
    /// </summary>
    public enum UIState
    {
        NORMAL,//��Init֮ǰ��׼��״̬ Ĭ��ֵ
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
        VIEW_EFFECT,//���涯Ч
        OTHERS_EFFECT//������Ч ������Ч��������СUI����ʱ����������
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
    /// ��Ϸ��UI����
    /// </summary>
    public enum GameUIName
    {
        HumanSkill
    }

    
}
