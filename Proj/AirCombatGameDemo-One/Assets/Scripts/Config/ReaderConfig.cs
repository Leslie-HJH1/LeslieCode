using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaderConfig
{
	//为什么用Func  因为需要获取一个新的Reader对象 每次都需要一个新的对象 用Func可以得到一个对象 
	private static readonly Dictionary<string, Func<IReader>> readersDic = new Dictionary<string, Func<IReader>>()
	{
		{".json", () => new JsonReader()}
	};

	public static IReader GetReader(string path)
	{
		foreach (KeyValuePair<string,Func<IReader>> pair in readersDic)
		{
			if (path.Contains(pair.Key))
			{
				return pair.Value();//返回的就是Func创建的对象
			}
		}
		
		Debug.LogError("未找到对应文件的读取器，文件路径："+path);
		return null;
	}
}
