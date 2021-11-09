using System;
using Const;
using UnityEngine;

namespace UIFrame
{
    public abstract class BasicUI : UIBase
    {
        //之前用的Init 在UIManager里面的Show调用有点晚 InitUi里需要先获得UI层级 所以为了防止先后顺序的问题
        //直接放到GetUiLayer里 不用初始化了
        public override UILayer GetUiLayer()
        {
            return UILayer.BASIC_UI;
        }
    }
}
