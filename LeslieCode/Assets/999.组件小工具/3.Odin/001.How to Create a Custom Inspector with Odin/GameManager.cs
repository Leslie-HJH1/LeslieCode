using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using System.Linq;

public enum GameState
{
    start,
    gamnePlay,
    pause,
    end
}

public class GameManager : MonoBehaviour
{

    [BoxGroup("Game State Info")]
    [EnumToggleButtons]
    [OnValueChanged("StateChange")]
    [ShowInInspector]
    public static GameState GameState;

    [BoxGroup("Game State Info")]
    [ShowInInspector]
    public static int TurnsRemaining = 3;
    
    [TabGroup("UI")]
    [SceneObjectsOnly,Required,InlineButton("SelectCanvas","Select")]
    public Canvas StartButtons;
    [TabGroup("UI")]
    [SceneObjectsOnly, Required, InlineButton("SelectCanvas", "Select")]
    public Canvas PauseMenu;
    [TabGroup("UI")]
    [SceneObjectsOnly, Required, InlineButton("SelectCanvas", "Select")]
    public Canvas HUD;


    [TabGroup("Music")]
    public AudioSource MusicSource;

    [Space]
    [ShowInInspector]
    [ValueDropdown("Musics")]
    [InlineButton("PlayMusic", "Play")]
    [TabGroup("Music")]
    private GameObject _CurMusicClip;
    [TabGroup("Music")]
    [InlineEditor(InlineEditorModes.SmallPreview)]
    public List<GameObject> Musics;


    [TabGroup("SFX")]
    public AudioSource sfx;
    [TabGroup("SFX")]
    public AudioClip ui;
    [TabGroup("SFX")]
    public AudioClip weapon;
    [TabGroup("SFX")]
    public AudioClip hit;
    [TabGroup("SFX")]
    public AudioClip spawn;

    [TabGroup("Enemies","Enemy Data")]
    [AssetsOnly]
    public GameObject ene;
    [TabGroup("Enemies", "Enemy Data")]
    [AssetsOnly]
    public List<GameObject> enes;

    [TabGroup("Enemies", "Enemy Points")]
    [InlineEditor(InlineEditorModes.GUIOnly)]
    [SceneObjectsOnly]
    public List<Transform> sps;
    
    [Button(ButtonSizes.Gigantic)]
    [TabGroup("Enemies", "Enemy Data")]
    [GUIColor(0.6f,1f,0.1f)]
    public void SpawnEnemy()
    {

    }

    private void PlaySFX(AudioClip sfxcp)
    {
        if (sfx!=null&&!sfx.isPlaying)
        {
            sfx.PlayOneShot(sfxcp);
        }
    }

    private void PlayMusic(AudioClip music)
    {
        if (MusicSource != null && music!=null)
        {
            MusicSource.clip = music;
            MusicSource.Play();
        }
    }


    public void StateChange()
    {
        switch (GameState)
        {
            case GameState.start:
                break;
            case GameState.gamnePlay:
                break;
            case GameState.pause:
                break;
            case GameState.end:
                break;
            default:
                break;
        }
    }

    private void SelectCanvas(Canvas _object)
    {
        if (_object)
        {
            UnityEditor.Selection.activeObject = _object.gameObject;
        }
    }

    private void PlayMusic(GameObject _object)
    {
        Debug.Log("播放音乐");
    }
}
