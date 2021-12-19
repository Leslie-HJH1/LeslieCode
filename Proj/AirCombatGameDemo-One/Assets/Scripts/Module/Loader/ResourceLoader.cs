using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

public class ResourceLoader : ILoader {

    public GameObject LoadPrefab(string path,Transform parent = null)
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        GameObject temp = Object.Instantiate(prefab, parent);
        return temp;
    }

    public T Load<T>(string path) where T : Object
    {
        T sprite = Resources.Load<T>(path);
        if (sprite == null)
        {
            Debug.LogError("未找到对应图片，路径："+path);
            return null;
        }
        else
        {
            return sprite;
        }
    }

    public T[] LoadAll<T>(string path) where T : Object
    {
        T[] sprites = Resources.LoadAll<T>(path);
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogError("当前路径下未找到对应资源，路径："+path);
            return null;
        }
        else
        {
            return sprites;
        }
    }

    public void LoadConfig(string path, Action<object> complete)
    {
        CoroutineMgr.Single.ExecuteOnce(Config(path,complete));
    }

    //WWW使用误区  并不是只能在携程中使用 在普通方法中也能使用
    //2019以前的版本 无法使用线程加载东西 2019之后Unity出了自己的东西 可以用线程 老师提到
    private IEnumerator Config(string path, Action<object> complete)
    {
        //安卓不需要添加前面的路径
        if(Application.platform != RuntimePlatform.Android)
            path = "file://" + path;
       
        WWW www = new WWW(path);//WWW中是保存了路径的一个对象
        yield return www;//这一步在等待WWW把路径中的资源加载好  加载好后才会执行到下一行

        //普通方法中的使用 这里会***阻塞***主线程
        //不推荐 但是存在这种方法 拓宽眼界
        // while (!www.isDone)
        // {
        // }
        
        if (www.error != null)
        {
            Debug.LogError("加载配置错误，路径为："+path);
            yield break;//退出迭代器
        }

        //返回出去
        complete(www.text);
        Debug.Log("文件加载成功，路径为："+path);
    }

    public GameObject LoadPrefabAndInstantiate(string path, Transform parent = null)
    {
        throw new NotImplementedException();
    }
}
