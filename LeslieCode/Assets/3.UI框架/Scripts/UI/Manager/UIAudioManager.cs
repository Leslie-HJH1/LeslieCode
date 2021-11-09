using UnityEngine;
using Util;
using DG.Tweening;
using Const;
using System;
using System.Collections.Generic;

namespace UIFrame
{
    public class UIAudioManager : MonoBehaviour     
    {
        private AudioSource _audioSource;
        private Func<string, AudioClip[]> _loadAudioFunc;
        private readonly Dictionary<string, AudioClip> _audioClipDic = new Dictionary<string, AudioClip>();

        public void Init(string audioPath, Func<string, AudioClip[]> loadFunc)
        {
            _audioSource = transform.GetOrAddComponent<AudioSource>();
            AddLoadListener(loadFunc);
            LoadAllAudioClip(audioPath);

        }

        private void AddLoadListener(Func<string, AudioClip[]> loadFunc)
        {
            if (loadFunc==null)
            {
                Debug.LogError("loadFunc can not be null");
                return;
            }
            _loadAudioFunc = loadFunc;
        }

        private void LoadAllAudioClip(string audioPath)
        {
            var audios = _loadAudioFunc(audioPath);
            foreach (AudioClip clip in audios)
            {
                if (!_audioClipDic.ContainsKey(clip.name))
                {
                    _audioClipDic[clip.name] = clip;
                }
            }
        }

        private AudioClip GetClip(string name)
        {
            if (_audioClipDic.ContainsKey(name))
            {
                return _audioClipDic[name];
            }
            else
            {
                Debug.LogError("audioClipDic dont contains key name:" + name);
                return null;
            }
        }

        public void Play(string name)
        {
            var temp= GetClip(name);
            if (temp!=null)
            {
                //_audioSource.clip = temp;
                _audioSource.PlayOneShot(temp,0.5f);
            }
        }

        public void PlayBg(string name)
        {
            var temp = GetClip(name);

            if (temp!=null)
            {
                _audioSource.clip = temp;
                _audioSource.loop = true;
                _audioSource.volume = 0.6f;
                _audioSource.Play();
            }
        }

    }
}
