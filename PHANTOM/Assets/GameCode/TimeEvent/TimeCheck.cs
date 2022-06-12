using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class TimeEventCheck
{
    public enum TimeScale
    {
        ScaleTime,
        UnScaleTime,
    }
    static List<TimeEvent> allEvents = new List<TimeEvent>();

    public static void AddTimeEvent(Action _action,float dt,TimeScale timeScale, bool Pause=true)
    {
        allEvents.Add(CreateTimeEvent(_action, dt, timeScale, Pause));
    }


    public static void TimeUpdate()
    {
        bool dirty = false;
        List<TimeEvent> Copy = (from te in allEvents select te).ToList();
        foreach (var te in Copy)
        {
            if (_pause)
            {
                if (te.Pause) continue;
            }
            TimeEvent.State state=te.Update();
            if (!dirty)
            {
                if (state == TimeEvent.State.Delete)
                {
                    dirty = true;
                }
            }
            
            
        }

        if (dirty)
        {
            allEvents = (from te in allEvents where te.drop == false select te).ToList();
        }
    }

    static TimeEvent CreateTimeEvent(Action _action, float dt, TimeScale timeScale,bool IsPause)
    {
        if (timeScale == TimeScale.UnScaleTime)
        {
            return new UnScaleTimeEvent { action = _action, dt = dt, Pause = IsPause };
        }
        else if (timeScale == TimeScale.ScaleTime)
        {
            return new ScaleTimeEvent { action = _action, dt = dt, Pause = IsPause };
        }
        else { Debug.LogError("there is no that kind of TimeScale."); return null; }
    }
    static bool _pause;
    public static void Pause(bool pause)
    {
        _pause = pause;
    }
}
public static class TimeEventUpdate
{

    static List<TimeUpdate> allEvents = new List<TimeUpdate>();
    /// <summary>
    /// 要重複運行的action跟運行的時間
    /// </summary>
    /// <param name="_action"></param>
    /// <param name="dt"></param>
    public static void AddTimeUpdate(Action _action, float dt)
    {
        TimeUpdate te = new TimeUpdate { action = _action,triggerTime = dt };
        allEvents.Add(te);
    }


    public static void Update()
    {
        bool dirty = false;
        List<TimeUpdate> Copy = (from te in allEvents select te).ToList();
        foreach (var te in Copy)
        {
            TimeUpdate.State state = te.Update();
            if (!dirty)
            {
                if (state == TimeUpdate.State.Delete)
                {
                    dirty = true;
                }
            }


        }

        if (dirty)
        {
            allEvents = (from te in allEvents where te.drop == false select te).ToList();
        }


    }


}




abstract class TimeEvent
{

    public enum State
    {
        Keep,
        Delete
    }
    /// <summary>
    /// 想要在幾秒後觸發Event
    /// </summary>
    public float dt;
    /// <summary>
    /// 觸發的事件
    /// </summary>
    public Action action;
    /// <summary>
    /// 觸發時間
    /// </summary>
    public float triggerTime=0f;
    /// <summary>
    /// 是否要重複觸發
    /// </summary>
    public bool repeat = false;
    /// <summary>
    /// 是否要刪除此event
    /// </summary>
    public bool drop = false;
    public bool Pause { get; set; }
    protected abstract float UsingTime { get; }
    public State Update()
    {
        triggerTime += UsingTime;
        if (triggerTime>dt)
        {
            action();
            if (repeat)
            {
                triggerTime =0;
                return State.Keep;
            }
            drop = true;
            return State.Delete;
        }
        return State.Keep;
    }
}
class UnScaleTimeEvent:TimeEvent
{
    protected override float UsingTime => Time.unscaledDeltaTime;
}
class ScaleTimeEvent:TimeEvent
{
    protected override float UsingTime => Time.deltaTime;
}
class TimeUpdate
{
    public enum State
    {
        Keep,
        Delete
    }
    public Action action;
    /// <summary>
    /// 想要update多久
    /// </summary>
    public float triggerTime;
    float clock;
    /// <summary>
    /// 是否要刪除此event
    /// </summary>
    public bool drop = false;
    public State Update()
    {
        clock +=Time.unscaledDeltaTime;
        action();
        if (clock >= triggerTime)
        {
            drop = true;
            return State.Delete;
        }
        else return State.Keep;
    }
}
