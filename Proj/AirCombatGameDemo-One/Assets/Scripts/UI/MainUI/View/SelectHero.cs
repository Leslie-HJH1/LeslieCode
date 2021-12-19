using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectHero : ViewBase
{
	protected override void InitChild()
	{
		//英雄选择 3个子物体有序排列再父物体下面 并依次添加脚本上去
		foreach (Transform trans in transform)
		{
			trans.gameObject.AddComponent<HeroItem>();
		}
	}
}
