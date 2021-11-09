using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineController
{
	private static int _id;
	public int ID { get; private set; }//唯一ID 对应于每一个Ctrl 每个Ctrl负责每个Item的迭代器对象
	private CoroutineItem _item;
	private MonoBehaviour _mono;//Mono
	private IEnumerator _routine;//具体需要执行的迭代器对象
	private Coroutine _coroutine;//用于执行 Item 中的迭代器

	public CoroutineController(MonoBehaviour mono,IEnumerator routine)
	{
		_item = new CoroutineItem();
		_mono = mono;
		_routine = routine;
		ResetData();//编号赋值
	}

	public void Start()
	{
		_item.State = CoroutineItem.CoroutineState.RUNNING;
		_coroutine = _mono.StartCoroutine(_item.Body(_routine));
	}

	public void Pause()
	{
		_item.State = CoroutineItem.CoroutineState.PASUED;
	}

	public void Stop()
	{
		_item.State = CoroutineItem.CoroutineState.STOP;
	}

	public void Continue()
	{
		_item.State = CoroutineItem.CoroutineState.RUNNING;
	}

	public void ReStart()
	{
		if(_coroutine != null)
			_mono.StopCoroutine(_coroutine);

		Start();
	}

	private void ResetData()
	{
		ID = _id++;//自加 就可以把每个Ctrl分开
	}
}
