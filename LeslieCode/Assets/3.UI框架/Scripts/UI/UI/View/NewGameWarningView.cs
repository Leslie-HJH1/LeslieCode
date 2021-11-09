using System.Collections.Generic;
using Const;
using Manager;
//using Manager;
using UnityEngine;
using Util;

namespace UIFrame
{
    public class NewGameWarningView : OverlayUI
    {
        public override UiId GetUiId()
        {
            return UiId.NewGameWarning;
        }

        public override List<Transform> GetBtnParents()
        {
            List<Transform> list = new List<Transform>();
            list.Add(transform.GetBtnParent());
            //list.Add(transform.Find("Buttons"));
            return list;
        }

        protected override void Init()
        {
            base.Init();
            transform.AddBtnListener("Yes", () =>
            {
                //DataManager.Single.ResetData();
                RootManager.Instance.Show(UiId.Loading);
                //Debug.Log("yes");
            });
            transform.AddBtnListener("No", () => { RootManager.Instance.Back(); });
        }

        //public override Transform GetDefaultBtn()
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
