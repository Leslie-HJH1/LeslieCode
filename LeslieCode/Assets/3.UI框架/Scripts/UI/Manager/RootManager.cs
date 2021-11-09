using Const;
using System;
using UnityEngine;

namespace UIFrame
{
    public class RootManager : MonoBehaviour
    {
        public static RootManager Instance { get; private set; }

        private UIManager _uiManager;
        private UIEffectManager _effectManager;
        private UILayerManager _layerManager;
        private InputManager _inputManager;
        private BtnStateManager _btnStateManager;
        private UIAudioManager _audioManager;

        private void Awake()
        {
            Instance = this;
            _uiManager = gameObject.AddComponent<UIManager>();
            _effectManager = gameObject.AddComponent<UIEffectManager>();
            _layerManager = gameObject.AddComponent<UILayerManager>();
            _inputManager = gameObject.AddComponent<InputManager>();
            _btnStateManager= gameObject.AddComponent<BtnStateManager>();
            _audioManager = gameObject.AddComponent<UIAudioManager>();

            _audioManager.Init(Path.UI_AUDIO_PATH, LoadManager.Single.LoadAll<AudioClip>);
            _uiManager.AddGetLayerObjectListener(_layerManager.GetLayerObject);//通过委托传进去
            _uiManager.AddInitCallBackListener((uiTrans) =>//每次切换新界面 都会调用
            {
                var list = _uiManager.GetBtnParents(uiTrans);//获取每组按钮的父物体
                _btnStateManager.InitBtnParent(list);//对父物体添加BtnParent
            });

            _audioManager.PlayBg(UIAudioName.UI_bg.ToString());


        }

        private void Start()
        {
            Show(UiId.MainMenu);
        }

        public void Show(UiId id)
        {
            var uiPara = _uiManager.Show(id);
            ExcuteEffect(uiPara);
            ShowBtnState(uiPara.Item1);
        }

        public void Back()
        {
            var uiPara = _uiManager.Back();
            ExcuteEffect(uiPara);
            ShowBtnState(_uiManager.GetCurrentUITrans());//当前界面
        }

        public void ButtonLeft()
        {
            _btnStateManager.Left();
        }

        public void ButtonRight()
        {
            _btnStateManager.Right();
        }

        //1.要显示的  2.要隐藏的
        //处理效果
        //show 和 back 同理 对于小UI 
        //小UI显示 要把当前层的Other动效关闭
        //小UI关闭 要把当前层的Other动效开启
        private void ExcuteEffect(Tuple<Transform, Transform> uiPara)
        {
            ShowUI(uiPara.Item1);
            HideUI(uiPara.Item2); 
        }

        private void ShowUI(Transform showUI)
        {
            ShowEffect(showUI);
            ShowUIAudio();
        }

        private void HideUI(Transform hideUI)
        {
            HideEffect(hideUI);
            HideUIAudio();
        }

        private void ShowUIAudio()
        {
            _audioManager.Play(UIAudioName.UI_in.ToString());
        }

        private void HideUIAudio()
        {
            _audioManager.Play(UIAudioName.UI_out.ToString());
        }

        private void ShowEffect(Transform showUI)
        {
            if (showUI == null)
            {
                _effectManager.ShowOthersEffect(_uiManager.GetCurrentUITrans());//当前的UI 因为小UI关闭了 当前的基础UI的其他动效就是要展示出来
            }
            else
            {
                _effectManager.Show(showUI);
            }
        }

        private void HideEffect(Transform hideUI)
        {
            //如果当前是小UI 那么第二项为空
            //为空 表示要找到最近的basicUI 把它的other动效关闭
            if (hideUI == null)
            {
                _effectManager.HideOthersEffect(_uiManager.GetBasicUITrans());//最近的基础UI 因为显示的是小UI
            }
            else
            {
                _effectManager.Hide(hideUI);
            }
        }

        public void PlayAudio(UIAudioName name)
        {
            _audioManager.Play(name.ToString());
        }

        public void SelectedButton()
        {
            _btnStateManager.SelectedButton();
        }

        private void ShowBtnState(Transform ui)
        {
            _btnStateManager.Show(ui);
        }


    }
}
