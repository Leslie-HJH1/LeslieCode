using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class LaunchGame : MonoBehaviour {
	// Use this for initialization
	void Start ()
	{
		//DataMgr.Single.ClearAll();//清除之前的数据先  因为Unity PlayerPrefs的原因  

		GameStateModel.Single.CurrentScene = SceneName.Main;
		IInit lifeCycle = LifeCycleMgr.Single;//接口显式实现  调用方法 必须转为对应接口 才能调用方法  防止错误调用
		lifeCycle.Init();
		
		UIManager.Single.Show(Paths.PREFAB_START_VIEW);

		// JsonReader reader = new JsonReader();
		// reader["planes"][0]["life"].Get<float>((value)=>Debug.Log(value));
		// reader.SetData(_json);
		
	}
	
	//private string _json="{'planes':[{'planeId':0,'level':1,'attack':5,'fireRate':0.8,'life':100}]}";
	
}
