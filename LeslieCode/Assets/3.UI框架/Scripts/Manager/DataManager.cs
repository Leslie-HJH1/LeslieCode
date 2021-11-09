using UnityEngine;
using Util;
using DG.Tweening;
using Const;
using System;
using UIFrame;
using UnityEngine.SceneManagement;
//using Game;

namespace Manager
{
    public class DataManager : SingletonBase<DataManager>
    {
        public DifficultLevel DifficultLevel
        {
            set { PlayerPrefs.SetString(ConstValue.DIFFICULT_LEVEL, value.ToString()); }
            get
            {
                string value = PlayerPrefs.GetString(ConstValue.DIFFICULT_LEVEL, DifficultLevel.NONE.ToString());
                DifficultLevel level;
                if (!Enum.TryParse(value, out level))
                {
                    Debug.LogError("parse Diffcultlevel type faile");
                    return DifficultLevel.NONE;
                }
                else
                {
                    return level;
                }
            }
        }

        /// <summary>
        /// 关卡数据标记 默认是1
        /// </summary>
        // public LevelID LevelIndex
        // {
        //     set { PlayerPrefs.SetInt(ConstValue.LEVEL_INDEX, (int) value); }
        //     get { return (LevelID) PlayerPrefs.GetInt(ConstValue.LEVEL_INDEX, 1); }
        // }

        /// <summary>
        /// 关卡的第几部分的标记 默认是1
        /// </summary>
        // public LevelPartID LevelPartIndex
        // {
        //     set { PlayerPrefs.SetInt(ConstValue.LEVEL_PART_INDEX, (int)value); }
        //     get { return (LevelPartID)PlayerPrefs.GetInt(ConstValue.LEVEL_PART_INDEX, 1); }
        // }

        public bool JudgeExistData()//判断是否已经存档过
        {
            return DifficultLevel != DifficultLevel.NONE;
        }

        // public string GetSceneName()
        // {
        //     if (JudgeCurrentScene(ConstValue.MAIN_SCENE))
        //     {
        //         return ConstValue.COMICS_SCENE;
        //     }
        //     else if(JudgeCurrentScene(ConstValue.COMICS_SCENE))
        //     {
        //         return ConstValue.LEVEL_SCENE+"_"+LevelIndex.ToString("00");
        //     }
        //     else
        //     {
        //         return ConstValue.MAIN_SCENE;
        //     }
        // }

        private bool JudgeCurrentScene(string name)
        {
            return SceneManager.GetActiveScene().name == name;
        }

        // public void ResetData()
        // {
        //     LevelIndex = LevelID.ONE;
        //     //LevelGamePartIndex = LevelGamePartID.ONE;
        //     LevelPartIndex = LevelPartID.ONE;
        // }
    }
}
