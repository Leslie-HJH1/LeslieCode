using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Menu
{

    [MenuItem("GameObject/UI/YouYou",false,2500)]
    private static void MakeYouYouText(MenuCommand menuCommand)
    {
        if (menuCommand.context==null)
        {
            AttachToCanvas(MakeYouYouPrefab("TestObj", menuCommand));
        }
        else
        {
            MakeYouYouPrefab("TestObj", menuCommand);
        }
    }

    /// <summary>
    /// 创建预设
    /// </summary>
    /// <param name="prefabName"></param>
    /// <param name="menuCommand"></param>
    /// <returns></returns>
    private static GameObject MakeYouYouPrefab(string prefabName,MenuCommand menuCommand)
    {
        GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Editor/GameObject/"+ prefabName+".prefab");
        GameObject newObj = UnityEngine.Object.Instantiate(gameObject);

        newObj.name = gameObject.name;
        GameObjectUtility.SetParentAndAlign(newObj,menuCommand.context as GameObject);
        Selection.activeObject = newObj;

        return newObj;
    }

    /// <summary>
    /// 附加画布
    /// </summary>
    /// <param name="gameObject"></param>
    private static void AttachToCanvas(GameObject gameObject)
    {
        Canvas canvas = UnityEngine.Object.FindObjectOfType<Canvas>();
        if (canvas==null)
        {
            GameObject obj = new GameObject();
            canvas = obj.AddComponent<Canvas>();
            canvas.name = "Canvas";
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            canvas.gameObject.AddComponent<CanvasScaler>();
            canvas.gameObject.AddComponent<GraphicRaycaster>();
        }
        gameObject.transform.SetParent(canvas.transform);
    }

    /// <summary>
    /// 操作组件
    /// CONTEXT要大写
    /// </summary>
    [MenuItem("CONTEXT/Text/ChangeToYouYou")]
    private static void ChangeToYouYou(MenuCommand menuCommand)
    {
        Text currText=menuCommand.context as Text;
        GameObject obj = currText.gameObject;

        //存好Text中的属性值
        string text = currText.text;
        //...
        
        UnityEngine.Object.DestroyImmediate(currText);

        Money money = obj.AddComponent<Money>();
        
        //重新赋值新的Text中的属性值
    }
}