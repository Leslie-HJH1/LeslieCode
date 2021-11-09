using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Module.Timer
{
    /// <summary>
    /// 计时器接口
    /// </summary>
    public interface ITimer
    {
        /// <summary>
        /// 计时器唯一标识
        /// </summary>
        string ID { get; }
        /// <summary>
        /// 当前的时间
        /// </summary>
        float CurrentTime { get; }
        /// <summary>
        /// 是否正在计时
        /// </summary>
        bool IsTiming { get; }
        /// <summary>
        /// 运行百分比
        /// </summary>
        float Percent { get; }
        /// <summary>
        /// 单词循环持续时间
        /// </summary>
        float Duration { get; }
        /// <summary>
        /// 是否循环
        /// </summary>
        bool IsLoop { get;}
        /// <summary>
        /// 是否完成
        /// </summary>
        bool IsComplete { get; }
        /// <summary>
        /// 继续
        /// </summary>
        void Continue();
        /// <summary>
        /// 暂停
        /// </summary>
        void Pause();
        /// <summary>
        /// 停止
        /// </summary>
        void Stop();
        /// <summary>
        /// 帧函数
        /// </summary>
        void Update();
        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="loop"></param>
        void ResetData(string id,float duration, bool loop);
        
        ITimer AddUpdateListener(Action update);
        ITimer AddCompleteListener(Action complete);
    }

    public interface ITimerManager
    {
        ITimer CreateTimer(string id, float duration, bool loop);
        /// <summary>
        /// 通过标识获取计时器
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ITimer GetTimer(string id);
        void Update();
        void ContinueAll();
        void PauseAll();
        void StopAll();
    }
    
    /// <summary>
    /// 计时器
    /// </summary>
    /// <summary>
    /// 计时器管理类
    /// </summary>
    public class TimerManager:ITimerManager
    {
        private class Timer : ITimer
        {
            // 开始 暂停 结束
            //回调函数 帧函数 计时结束函数
            //初始化 注册
            //获取目前时间 百分比

            /// <summary>
            /// 运行百分比
            /// </summary>
            public float Percent
            {
                get { return _runTimeTotal / _duration; }
            }
            /// <summary>
            /// 单次循环持续时间
            /// </summary>
            public float Duration
            {
                get { return _duration; }
            }
            //是否完成
            public bool IsComplete
            {
                get;
                private set;
            }

            public string ID { get; private set; }

            /// <summary>
            /// 当前的时间
            /// </summary>
            public float CurrentTime
            {
                get { return _runTimeTotal; }
            }
            //是否循环执行 只在初始化的时候赋值一次 后面不改变值
            //只能通过构造函数对readonly型的变量赋值
            public bool IsLoop { get; private set; }
            
            
            //总运行时间
            private float _runTimeTotal;
            //持续时间
            private float _duration;
            //刷新间隔帧数
            private int _offsetFrame = 20;
            private int _frameTimes;
            private Action _onUpdate;
            private Action _onComplete;
            public bool IsTiming { get; private set; }
            private DateTime _startTime;
            
            
            public Timer(string id, float duration, bool loop)
            {
                InitData(id,duration, loop);
            }

            private void InitData(string id, float duration, bool loop)
            {
                ID = id;
                _duration = duration;
                IsLoop = loop;
                
                ResetData();
            }

            public void Continue()
            {
                IsTiming = true;
                _startTime = DateTime.Now;
            }

            public void Pause()
            {
                IsTiming = false;
                _runTimeTotal += GetCurrentTimingTime();
            }

            public void Stop()
            {
                if (IsComplete)
                {
                    _onComplete?.Invoke();
                }
                _onComplete = null;
                _runTimeTotal = 0;
                IsTiming = false;
            }

            public void Update()
            {
                _frameTimes++;
                if (_frameTimes<_offsetFrame)
                    return;

                if (IsComplete || !IsTiming)
                    return;

                IsComplete = JudgeIsComplete();
                
                if (IsLoop)
                {
                    Loop();
                }
                else
                {
                    NotLoop();
                }

                _onUpdate?.Invoke();
            }

            /// <summary>
            /// 重置数据
            /// </summary>
            /// <param name="id"></param>
            /// <param name="duration"></param>
            /// <param name="loop"></param>
            public void ResetData(string id,float duration, bool loop)
            {
                InitData(id, duration, loop);
            }

            private void ResetData()
            {
                IsComplete = false;
                IsTiming = true;
                _startTime = DateTime.Now;
                _runTimeTotal = 0;
            }

            private void Loop()
            {
                if (IsComplete)
                {
                    IsComplete = false;
                    _onComplete?.Invoke();
                    ResetData();
                }
            }

            private void NotLoop()
            {
                if (IsComplete)
                {
                    _onComplete?.Invoke();
                    _onComplete = null;
                }
            }

            public ITimer AddUpdateListener(Action update)
            {
                _onUpdate += update;
                return this;
            }

            public ITimer AddCompleteListener(Action complete)
            {
                _onComplete += complete;
                return this;
            }

            private float GetCurrentTimingTime()
            {
                var time = DateTime.Now - _startTime;
                return (float) time.TotalSeconds;
            }

            /// <summary>
            /// 判断当前是否执行完毕
            /// </summary>
            /// <returns></returns>
            private bool JudgeIsComplete()
            {
                return (_runTimeTotal+GetCurrentTimingTime()) > _duration;
            }
            
            
        }

        private HashSet<ITimer> _activeTimers;
        private HashSet<ITimer> _inactiveTimers;
        private HashSet<ITimer>.Enumerator _activeEnum;
        private Dictionary<string, ITimer> _timersDic;
        
        public TimerManager()
        {
            _activeTimers = new HashSet<ITimer>();
            _inactiveTimers = new HashSet<ITimer>();
            _timersDic = new Dictionary<string, ITimer>();
        }

        /// <summary>
        /// 创建新计时器
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="loop"></param>
        /// <returns></returns>
        public ITimer CreateTimer(string id, float duration, bool loop)
        {
            ITimer timer = null;
            if (_timersDic.ContainsKey(id))
            {
                timer = _timersDic[id];
                if (!timer.IsTiming)
                {
                    _inactiveTimers.Remove(timer);
                    timer.ResetData(id, duration, loop);
                    _activeTimers.Add(timer);
                }
                else
                {
                    return null;
                }
                //Debug.LogError("id:"+id+"已存在");
                //return null;
            }
            else
            {
                if (_inactiveTimers.Count > 0)
                {
                    timer = _inactiveTimers.First();

                    _timersDic.Remove(timer.ID);//移除此 之前存在字典里的清掉
                    
                    _inactiveTimers.Remove(timer);
                    timer.ResetData(id, duration, loop);
                    _activeTimers.Add(timer);
                }
                else
                {
                    timer = new Timer(id, duration, loop);
                    _activeTimers.Add(timer);
                    //如果不想修改AddCompleteListener里面的参数
                    //使用Lambda表达式
                }
                _timersDic[id] = timer;
            }
            
            timer.AddCompleteListener(()=>TimerComplete(timer));
            return timer;
        }

        public ITimer GetTimer(string id)
        {
            if (_timersDic.ContainsKey(id))
            {
                return _timersDic[id];
            }
            else
            {
                return null;
            }
        }
        
        private void TimerComplete(ITimer timer)
        {
            if (!timer.IsLoop)
            {
                _activeTimers.Remove(timer);
                _inactiveTimers.Add(timer);
            }
        }

        public void Update()
        {
            //有些数组用迭代器比用for foreach快
            _activeEnum = _activeTimers.GetEnumerator();
            int count = _activeTimers.Count;

            for (int i = 0; i < count; i++)
            {
                if (!_activeEnum.MoveNext())
                {
                    continue;
                }
                else
                {
                    _activeEnum.Current.Update();
                }
            }

            // if (_activeTimers.Count>0)
            // {
            //     HashSet<ITimer> temp = new HashSet<ITimer>();
            //     foreach (var timer in _activeTimers)
            //     {
            //         timer.Update();
            //         if (!timer.IsLoop && timer.IsComplete)
            //         {
            //             temp.Add(timer);
            //         }
            //         
            //     }
            //
            //     foreach (ITimer timer in temp)
            //     {
            //         _activeTimers.Remove(timer);
            //         _inactiveTimers.Add(timer);
            //     }
            // }
        }

        /// <summary>
        /// 继续执行所有计时器
        /// </summary>
        public void ContinueAll()
        {
            foreach (var timer in _activeTimers)
            {
                timer.Continue();
            }
        }

        /// <summary>
        /// 暂停执行所有计时器
        /// </summary>
        public void PauseAll()
        {
            foreach (var timer in _activeTimers)
            {
                timer.Pause();
            }
        }

        /// <summary>
        /// 关闭执行所有计时器
        /// </summary>
        public void StopAll()
        {
            foreach (var timer in _activeTimers)
            {
                timer.Stop();
            }
        }
        
    }
}