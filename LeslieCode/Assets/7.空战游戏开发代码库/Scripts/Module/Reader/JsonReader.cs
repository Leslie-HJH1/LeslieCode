using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using LitJson;
using UnityEngine;
using UnityEngine.Events;

//jsonReader["planes"][0]["planeId"].Get<int>(()=>)
//jsonReader["planes"][0]["planeId"]
public class JsonReader : IReader
{
    private JsonData _data;//当前解析的数据
    private JsonData _tempData;//缓存当前的通过key获得的JsonData
    
    private KeyQueue _keys;//当前正在添加的缓存 相当于这只是单个json文件的keys
    private Queue<KeyQueue> _keyQueues = new Queue<KeyQueue>();//不一定只有一组数据 比如同时加载好几个数据表 多套命令队列
    
    public IReader this[string key]
    {
        get
        {
            if (!SetKey(key))//没设置成功 说明数据存在 直接往下层找数据即可
            {
                try
                {
                    _tempData = _tempData[key];//存储当前获取的值
                }
                catch (Exception e)
                {
                    Debug.LogError("在数据中无法找到对应键值，数据："+_tempData.ToJson()+"  键值："+key);
                }
                
            }
            return this;
        }
    }

    public IReader this[int key]
    {
        get
        {
            if (!SetKey(key))
            {
                try
                {
                    _tempData = _tempData[key];//存储当前获取的值
                }
                catch (Exception e)
                {
                    Debug.LogError("在数据中无法找到对应键值，数据："+_tempData.ToJson()+"  键值："+key);
                }
            }
            return this;
        }
    }

    //由于传入的参数有 int string 所以用泛型解决
    //每次调用 this[] 方法的时候 调用这个方法，就把key值缓存下来 这样就可以在
    //当出现 当下时刻数据没有加载完 的情况  就可以等加载完之后 再次从缓存中记载数据
    //从缓存中读取 是否是调用了命令了 直接从缓存中 读取之前没有读到的数据
    //其实缓存中存的就是 当数据加载时（还没有加载完） 调用Reader读取json数据的key队列
    //这样在数据加载完成后 就可以读取队列把当时没有加载的数据重新加载
    //这我就理解了老师之前讲到的 王者荣耀 断线重连的缓存机制 把断线后的命令缓存下来
    //等重连后 把数据重新发送给客户端
    private bool SetKey<T>(T key)//设置是否成功
    {
        //判断当前数据是否为空
        //_keys != null 表示当前正在索引该列数据 等获取到数据后 会手动将_keys置空
        //用是否等于空的情况来表示当前这组数据是否加载完毕
        if (_data == null || _keys != null)
        {
            //表示来了一组新的队列
            //因为如果一组结束了 到底了 那么会手动置空
            if(_keys == null)
                _keys = new KeyQueue();
            
            //Key值的缓存
            IKey keyData = new Key();
            keyData.Set(key);
            _keys.Enqueue(keyData);//当前数据没有 把key加入队列

            return true;//加入缓存成功
        }

        return false;
    }

    public void Count(Action<int> callBack)
    {
        bool success = SetKey<Action>(() =>
        {
            if (callBack != null)
                callBack(GetCount());
        });

        if (!success)
        {
            callBack(GetCount());
        }
        else
        {
            _keyQueues.Enqueue(_keys);
            _keys = null;
        }
    }

    private int GetCount()
    {
        return _tempData.IsArray ? _tempData.Count : 0;
    }

    public void Get<T>(Action<T> callBack)
    {
        //调用Get表示已经到最后进行回调出数据的过程
        //所以如果_keys还是不为空，表示缓存中还有key值
        //而且已经到最后了 加上回调 这样加载完就可以回传数据
        //那么就表示把回调写进去，用来
        if (_keys != null)
        {
            //加载好后 把加载好的jsondata传入进去
            //回调函数的缓存
            //我们只要只要知道 这个一加完 说明该条队列添加完毕
            //等待数据加载完成 就可以
            _keys.OnComplete((dataTemp) =>
            {
                //执行同样的Get代码
                T value = GetValue<T>(dataTemp);
                ResetData();//记得重置数据 执行到这里 也要把当前的JsonData赋值给_tempData
                callBack(value);
            }); 
            
            //这一组就搞好了 加入队列
            _keyQueues.Enqueue(_keys);
            _keys = null;//这样下次判断时 就能以空的为依据
            
            ExecuteKeysQueue();//放这里是因为 这个时候 当前缓存队列_keys已经入_keyQueues里了
                               //就可以直接执行队列了
                               //每次Get完都会调用一次 到最后加载完 总可以把队列清空完毕
            return;
        }
        
        if (callBack == null)//回调为空时
        {
            Debug.LogWarning("当前回调方法为空，不返回数据");
            ResetData();
            return;
        }

        T data = GetValue<T>(_tempData);//解析数据
        ResetData();//让tempData缓存当前通过下标获取的值
        callBack(data);//数据传出去
    }

    /// <summary>
    /// 执行缓存队列
    /// 命令存好以后 就可以调用一次
    /// </summary>
    private void ExecuteKeysQueue()
    {
        //检测_data是否为空 如果为空表示还在执行SetData的_data = JsonMapper.ToObject(data as string);的方法
        //不为空 那么就可以执行缓存队列
        if(_data == null)
            return;

        IReader reader = null;
        foreach (KeyQueue keyQueue in _keyQueues)
        {
            //对所有Value执行完操作
            foreach (object value in keyQueue)
            {
                if (value is string)//这里就是IKey里Type存在的意义
                {
                    reader = this[(string) value];//reader的作用就是让this[(string) value]执行
                }
                else if(value is int)
                {
                    reader = this[(int) value];
                }
                else if (value is Action)
                {
                    ((Action) value)();//执行操作
                }
                else
                {
                    Debug.LogError("当前键值类型错误");
                }
            }
            
            //等数据执行完毕后
            //最后执行回调 把数据传出去
            keyQueue.Complete(_tempData);
        }
    }

    private T GetValue<T>(JsonData data)
    {
        //泛型的类型转换
        var converter = TypeDescriptor.GetConverter(typeof(T));

        try//安全校验
        {
            if (converter.CanConvertTo(typeof(T)))//能转换
            {
                return (T) converter.ConvertTo(data.ToString(), typeof(T));
            }
            else//无法转换
            {
                return (T) (object) data;//先转换成object 再转T
            }
        }
        catch (Exception e)
        {
           Debug.LogError("当前类型转换出现问题，目标类型为："+typeof(T).Name+"  data:"+data);
           return default(T);
        }
        
    }

    //数据重置
    //_data存的是最初赋值的
    private void ResetData()
    {
        _tempData = _data;
    }

    /// <summary>
    /// 加载配置需要时间
    /// 加载完成后 通过回调 调用SetData
    /// </summary>
    /// <param name="data"></param>
    public void SetData(object data)
    {
        if (data is string)
        {
            _data = JsonMapper.ToObject(data as string);
            ResetData();//让tempData缓存当前通过下标获取的值
            
            //存在缓存的意义 就是 SetData 的时候_data是空的
            //所以设置好_data之后 把所有缓存全部执行一遍  设置=》_data = JsonMapper.ToObject(data as string);
            //用于 先data[0][1] 后SetData(data) 这样的操作导致的没有加载就进行索引
            //这样就需要再SetData的时候重新调用一下队列   因为一开始的索引操作 用于设置缓存队列和设置回调
            ExecuteKeysQueue();
        }
        else
        {
            Debug.LogError("当前传入数据类型错误，当前类只能解析json");
        }
    }

    public ICollection<string> Keys()
    {
        if (_tempData == null)
            return new string[0];//细节 不return null
        
        return _tempData.Keys;
    }
}

public class KeyQueue : IEnumerable
{
    private Queue<IKey> _keys = new Queue<IKey>();
    private Action<JsonData> _complete;

    public void Enqueue(IKey key)
    {
        _keys.Enqueue(key);
    }
    
    public void Dequeue()
    {
        _keys.Dequeue();
    }

    public void Clear()
    {
        _keys.Clear();
    }
    
    public void Complete(JsonData data)
    {
        if (_complete != null)
            _complete(data);
    }
    
    //为了获取最后的值Get() 需要添加回调
    public void OnComplete(Action<JsonData> complete)
    {
        _complete = complete;
    }
    
    //foreach方法其实 调用的 就是这个方法
    public IEnumerator GetEnumerator()
    {
        foreach (IKey key in _keys)
        {
            yield return key.Get();//返回object  即key
        }
    }
}

public interface IKey
{
    void Set<T>(T key);//方法
    object Get();//方法
    Type KeyType { get; }//属性 不写private set;默认表示set私有
}

public class Key : IKey
{
    private object _key;
    public Type KeyType { get; private set; }

    public void Set<T1>(T1 key)
    {
        _key = key;
    }

    public object Get()
    {
        return _key;
    }
}