﻿using UnityEngine.UI;

[BindPrefab(Paths.PREFAB_LOADING_VIEW, Const.BIND_PREFAB_PRIORITY_VIEW)]
public class LoadingView : ViewBase
{
    private Slider _slider;

    protected override void InitChild()
    {
        _slider = Util.Get("Slider").Get<Slider>();
    }

    public override void Show()
    {
        base.Show();
        LifeCycleMgr.Single.Add(LifeName.UPDATE, this);
    }

    public override void UpdateFun()
    {
        base.UpdateFun();
        UpdateProgress();
        UpdateSlider();
    }

    public override void Hide()
    {
        base.Hide();
        LifeCycleMgr.Single.Remove(LifeName.UPDATE, this);
    }

    private void UpdateProgress()
    {
        var progress = SceneMgr.Single.Process();
        progress *= 100;
        Util.Get("Progress").SetText(string.Format("{0}%", progress));
    }

    private void UpdateSlider()
    {
        _slider.value = SceneMgr.Single.Process();
    }
}