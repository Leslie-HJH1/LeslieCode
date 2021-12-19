﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//全是显示代码
//View
public class PropertyItem : MonoBehaviour,IViewUpdate,IViewShow {

	public enum ItemKey
	{
		name,
		value,
		cost,
		grouth,
		maxVaue,
	}

	private static int _itemId = -1;
	private string _key;
	
	public void Init(string key)
	{
		_key = key;
		_itemId++;
		UpdatePos(_itemId);
	}


	//更新位置
	private void UpdatePos(int itemId)
	{
		RectTransform rect = transform.Rect();
		float offset = rect.rect.height * itemId;
		rect.anchoredPosition -= offset * Vector2.up;
	}

	private void UpdatePlaneId(int planeId)
	{
		UpdateData(planeId);
		UpdateSlider();
	}

	private void UpdateData(int planeId)
	{
		for (ItemKey i = 0; i < ItemKey.grouth; i++)
		{
			Transform trans = transform.Find(ConvertName(i));//直接找 不耗费很多性能
			if (trans != null)
			{
				string key = KeysUtil.GetPropertyKeys(_key + i);
				trans.GetComponent<Text>().text = DataMgr.Single.GetObject(key).ToString();
			}
			else
			{
				Debug.LogError("当前预制名称错误，正确名称："+ConvertName(i));
			}
		}
	}

	private void UpdateSlider()
	{
		Slider slider = transform.Find("Slider").GetComponent<Slider>();
		slider.minValue = 0;
		slider.maxValue = DataMgr.Single.Get<int>(KeysUtil.GetNewKey(ItemKey.maxVaue,_key));
		slider.value = DataMgr.Single.Get<int>(KeysUtil.GetNewKey(ItemKey.value,_key));
	}

	private string ConvertName(ItemKey key)
	{
		string first = key.ToString().Substring(0, 1).ToUpper();
		string others = key.ToString().Substring(1);
		return first + others;
	}

	public void UpdateFun()
	{
		UpdatePlaneId(GameStateModel.Single.SelectedPlaneId);
	}

	public void Show()
	{
		UpdatePlaneId(GameStateModel.Single.SelectedPlaneId);
	}
}
