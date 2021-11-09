using System;
using UnityEngine;

public interface ILoader
{
    GameObject LoadPrefab(string path, Transform parent = null);
    void LoadConfig(string path, Action<object> complete); //一般用WWW 用IO流会出问题
    T Load<T>(string path) where T : UnityEngine.Object;
    T[] LoadAll<T>(string path) where T : UnityEngine.Object;
}
