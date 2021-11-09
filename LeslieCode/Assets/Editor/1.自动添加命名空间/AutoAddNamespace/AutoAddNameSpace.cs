using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace CustomTool
{

    public class AutoAddNameSpace : UnityEditor.AssetModificationProcessor
    {
        private static string _path = "Assets/Editor/AutoAddNamespace/Cache/";
        private static string _dataName = "Data.asset";

        private static void OnWillCreateAsset(string path)
        {
            if (!IsOn())
                return;
            
            path = path.Replace(".meta", "");
            if (path.EndsWith(".cs"))
            {
                string text = "";
                text += File.ReadAllText(path);
                string name = GetClassName(text);
                if (string.IsNullOrEmpty(name))
                {
                    return;
                }
                var newText = GetNewScriptContext(name);
                File.WriteAllText(path, newText);
            }
            AssetDatabase.Refresh();//资源刷新 防止生成的脚本没有编译 不能挂载到物体上
        }

        public static NamespaceData GetData()
        {
            return AssetDatabase.LoadAssetAtPath<NamespaceData>(_path + _dataName);
        }

        private static bool IsOn()
        {
            NamespaceData data = GetData();
            if (data != null)
            {
                return data.IsOn;
            }

            return false;
        }

        //获取新的脚本内容
        private static string GetNewScriptContext(string className)
        {
            var script = new ScriptBuildHelp();
            script.WriteUsing("UnityEngine");
            script.WriteUsing("Util");
            script.WriteUsing("DG.Tweening");
            script.WriteUsing("Const");
            script.WriteEmptyLine();
            var data = AddNamespaceWindow.GetData();
            string name = data == null ? "UIFrame" : data.name;
            script.WriteNamespace(name);

            script.IndentTimes++;//缩进（累加的）
            script.WriteClass(className, "MonoBehaviour");

            script.IndentTimes++;//缩进
            List<string> keys = new List<string>();
            keys.Add("void");
            script.WriteFun(keys, "Start");
            return script.ToString();
        }

        //获取类名
        private static string GetClassName(string text)
        {
            //一般方法
            //string[] data = text.Split(' ');
            //int index = 0;

            //for (int i = 0; i < data.Length; i++)
            //{
            //    if (data[i].Contains("class"))
            //    {
            //        index = i + 1;
            //        break;
            //    }
            //}

            //考虑到类名与：之间可能没有空格
            //if (data[index].Contains(":"))
            //{
            //    //与：连在一起
            //    return data[index].Split(':')[0];
            //}
            //else
            //{
            //    return data[index];
            //}
            
            //正则方法
            //public class NewBehaviourScript : MonoBehaviour
            //A-Za-z0-9_]+ 大写小写字母、数字、下划线 循环（+）
            // \\s* 空格循环
            // :
            // \\s* 空格循环
            string patterm = "public class ([A-Za-z0-9_]+)\\s*:\\s*MonoBehaviour";
            
            var match = Regex.Match(text, patterm);
            if (match.Success)
            {
                //Group有两项 1是字符串整个 2是匹配的那一小段目标对象
                return match.Groups[1].Value;
            }

            return "";
        }
    }
}

