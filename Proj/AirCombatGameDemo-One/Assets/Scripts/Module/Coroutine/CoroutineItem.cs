using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineItem {

	//创建在类中 这样不通过类名是调用不到枚举的 增加密封性
	public enum CoroutineState
	{
		WAITTING,
		RUNNING,
		PASUED,
		STOP
	}

	public CoroutineState State { get; set; }

	//控制迭代器流程
	public IEnumerator Body(IEnumerator routine)
	{
		while (State == CoroutineState.WAITTING)//写成while 卡住逻辑 不然只停一帧
		{
			yield return null;
		}

		while (State == CoroutineState.RUNNING)
		{
			if (State == CoroutineState.PASUED)
			{
				yield return null;//等待一帧
			}
			else
			{
				//不为空 且 还有没有下一个值
				if (routine != null && routine.MoveNext())
				{
					yield return routine.Current;//下一个
				}
				else//没有了
				{
					State = CoroutineState.STOP;
				}
			}
		}
	}
}
