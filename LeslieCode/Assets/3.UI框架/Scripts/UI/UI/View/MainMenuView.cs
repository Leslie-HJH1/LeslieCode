using System.Collections.Generic;
using Const;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UIFrame
{
    public class MainMenuView : BasicUI
    {
        public override UiId GetUiId()
        {
            return UiId.MainMenu;
        }

        private void Start()
        {
            //transform.Find("Buttons/StartGame").RectTransform().AddBtnListener(() => { RootManager.Instance.Show(UiId.StartGame); });
            //transform.Find("Buttons/DOJO").RectTransform().AddBtnListener(() => {  });
            //transform.Find("Buttons/Help").RectTransform().AddBtnListener(() => {  });
            //transform.Find("Buttons/ExitGame").RectTransform().AddBtnListener(() => { Application.Quit(); });
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
            transform.AddBtnListener("StartGame", () => { RootManager.Instance.Show(UiId.StartGame); });
            transform.AddBtnListener("DOJO", () => { });
            transform.AddBtnListener("Help", () => { });
            transform.AddBtnListener("ExitGame", () => { Application.Quit(); });
        }

        //public override Transform GetDefaultBtn()
        //{
        //    return transform.Find("Buttons/StartGame");
        //}
    }
}
