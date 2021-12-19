using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


/// <summary>
/// Manager是供外面调用的 而Module则是核心内容 不能对外开放的
/// 就是Manager是外部访问Module的中间者
/// </summary>
public class LoadMgr : NormalSingleton<LoadMgr>,ILoader
{
	[SerializeField]
	private ILoader _loader;

	public LoadMgr()
	{
		_loader  = new ResourceLoader();
	}

	public GameObject LoadPrefab(string path, Transform parent = null)
	{
		return _loader.LoadPrefab(path, parent);
	}

    public GameObject LoadPrefabAndInstantiate(string path, Transform parent = null)
    {
        return _loader.LoadPrefabAndInstantiate(path, parent);
    }

    public void LoadConfig(string path, Action<object> complete)
	{
		_loader.LoadConfig(path,complete);
	}

	public T Load<T>(string path) where T : Object
	{
		return _loader.Load<T>(path);
	}

	public T[] LoadAll<T>(string path) where T : Object
	{
		return _loader.LoadAll<T>(path);
	}
}
