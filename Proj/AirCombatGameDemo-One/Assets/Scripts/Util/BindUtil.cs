using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindUtil 
{
	//同一个预制体上 多个脚本
	private static Dictionary<string,List<Type>> _prefabAndScriptMap = new Dictionary<string, List<Type>>();
	private static Dictionary<Type,int> _prioritysMap = new Dictionary<Type, int>();//优先级字典

	public static void Bind(BindPrefab data,Type type)
	{
		string path = data.Path;
		if (!_prefabAndScriptMap.ContainsKey(path))
		{
			_prefabAndScriptMap.Add(path,new List<Type>());
		}

		if (!_prefabAndScriptMap[path].Contains(type))
		{
			_prefabAndScriptMap[path].Add(type);
			_prioritysMap.Add(type,data.Priority);
			_prefabAndScriptMap[path].Sort(new BindPriorityComparer());//排序
		}
	}

	public static List<Type> GetType(string path)
	{
		if (_prefabAndScriptMap.ContainsKey(path))
		{
			return _prefabAndScriptMap[path];
		}
		else
		{
			Debug.LogError("当前数据中未包含路径："+path);
			return null;
		}
	}
	
	public class BindPriorityComparer : IComparer<Type>
	{
		//大于0调换  小于0不动
		public int Compare(Type x, Type y)
		{
			//x空 y要调到前面去 所以返回回1
			if (x == null)
				return 1;
			
			//y空 不动 返回-1
			if (y == null)
				return -1;
			
			//大于0 第一个值 > 第二个值
			//y会与x替换
			//从小到大排列
			return _prioritysMap[x] - _prioritysMap[y];
		}
	}
}


