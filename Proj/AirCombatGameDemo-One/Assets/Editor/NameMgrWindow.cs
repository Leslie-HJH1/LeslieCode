using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class NameMgrWindow : EditorWindow {

    //原本的名称值 要修改的名称值
    private Dictionary<string,string> _namesDic = new Dictionary<string, string>();
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<NameMgrWindow>();
    }

    private void OnGUI()
    {
        GUILayout.Label("资源名称管理器");
        
        NameMgrWindowData.UpdateData();//数据更新：把已经不存在的项去掉
        
        foreach (KeyValuePair<FolderData,List<string>> pair in NameMgrWindowData.SpriteDic)
        {
            //*******************************1 第一行
            GUILayout.BeginHorizontal();
            
            GUILayout.Label("路径：",GUILayout.MaxWidth(50));
            GUILayout.Label(pair.Key.FolderPath,GUILayout.MaxWidth(150));
            GUILayout.Label("范例：",GUILayout.MaxWidth(50));
            GUILayout.Label(pair.Key.NameTip,GUILayout.MaxWidth(150));
            
            GUILayout.EndHorizontal();
            //*******************************1
            
            //*******************************2 第二行
            GUILayout.BeginHorizontal();
            foreach (string path in pair.Value)
            {
                //*******************************3 第三行
                GUILayout.BeginVertical();
                
                Texture2D texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                GUILayout.Box(texture2D,GUILayout.Height(80),GUILayout.Width(80));//现实图片
                string name = Path.GetFileNameWithoutExtension(path);
                if (!_namesDic.ContainsKey(name))//不包含当前名称 那么就保存下
                {
                    _namesDic[name] = name;
                }
                
                //*******************************4
                //修改值的操作
                GUILayout.BeginHorizontal();
                //输入和输出要相同
                _namesDic[name] = GUILayout.TextArea(_namesDic[name], GUILayout.Width(40));//文本输入框
                if (GUILayout.Button("确认", GUILayout.Width(30)))
                {
                    if (name != _namesDic[name])//判断当前值是否改变了
                    {
                        ChangeFileName(name, _namesDic[name], path);
                        _namesDic.Remove(name);
                    }
                    
                    AssetDatabase.Refresh();//刷新界面
                }
                GUILayout.EndHorizontal();
                //*******************************4
                
                GUILayout.EndVertical();
                //*******************************3
            }
            GUILayout.EndHorizontal();
            //*******************************2
        }
    }

    /// <summary>
    /// 重命名文件名称
    /// </summary>
    /// <param name="sourceName">原本名称</param>
    /// <param name="destName">目标名称</param>
    /// <param name="path">路径</param>
    private void ChangeFileName(string sourceName,string destName,string path)
    {
        string destPath = path.Replace(sourceName, destName);
        if (File.Exists(destPath))
        {
            Debug.LogError("当前文件名称已存在");
        }
        else
        {
            File.Move(path,destPath);//源路径 -> 目标路径
        }
    }
}

public class NameMgrWindowData
{
    //存 资源名称
    public static Dictionary<FolderData, List<string>> SpriteDic = new Dictionary<FolderData, List<string>>();
    
    public static void Add(FolderData key,string value)
    {
        if (!SpriteDic.ContainsKey(key))
        {
            SpriteDic[key] = new List<string>();
            
        }
        
        SpriteDic[key].Add(value);
    }

    public static void UpdateData()
    {
        foreach (KeyValuePair<FolderData,List<string>> pair in SpriteDic)
        {
            int count = pair.Value.Count;
            
            //不能用foreach 因为遍历的时候不能动子项
            for (int i = 0; i < count; i++)
            {
                if (!File.Exists(pair.Value[i]))
                {
                    pair.Value.Remove(pair.Value[i]);//移除后 此下标项去除 后面的前移 当前项得到补充 需要让下标不动 i++与i--抵消
                    i--;
                }
            }
        }
    }
}

public class FolderData
{
    //存 资源路径
    public string FolderPath;
    public string NameTip;
}
