using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate void Command();
[DefaultExecutionOrder(-1000)]
public class ServiceTicker : MonoBehaviour
{
    public delegate void OnServiceTickDelegate();

    public UnityAction OnEnabled;
    public UnityAction OnDisabled;
    public event OnServiceTickDelegate OnServiceTick;
    List<ServiceStack> commandStack = new List<ServiceStack> ();
    public List<OnServiceTickDelegate> currentDelegates = new List<OnServiceTickDelegate>();
    public static ServiceTicker st { get; private set; }
    bool commandRunning;
    private void Awake()
    {
        st = this;
    }

    private void OnEnable()
    {
        OnEnabled?.Invoke();
        Utility.DebugText("ServiceTicker enabled");
    }

    private void OnDisable()
    {
        OnDisabled?.Invoke();
        Utility.DebugText("Service ticker disabled");
    }

    private void Update()
    {
        OnServiceTick?.Invoke();
    }
    public void AddSubscriber(OnServiceTickDelegate deletgate)
    {
        if (!currentDelegates.Contains(deletgate))
        {
            OnServiceTick += deletgate;
            currentDelegates.Add(deletgate);
        }

    }
    public void RemoveSubscriber(OnServiceTickDelegate delegateToRemove)
    {
        if (currentDelegates.Contains(delegateToRemove))
        {
            OnServiceTick -= delegateToRemove;
            currentDelegates.Remove(delegateToRemove);
        }
    }
    void CommandWorker()
    {
        if (commandStack.Count > 0)
        {
            for (int i = 0; i < commandStack.Count; i++)
            {
                if (commandStack[i].CheckTimer(Time.time))
                {
                    commandStack[i].callBackOnFinished?.Invoke();
                    Utility.DebugText("OnFinished called");
                    commandStack.RemoveAt(i);
                }
            } 
        }
        else
        {
            OnServiceTick -= CommandWorker;
            commandRunning = false;
        }
    }
    public void CommandExecutionScheduler(float delayTimer, Command cmnd)
    {
        ServiceStack st = new ServiceStack(delayTimer, Time.time, cmnd);
        commandStack.Add(st);
        if (!commandRunning)
        {
            commandRunning = true;
            OnServiceTick += CommandWorker;
        }
    }
    public void CommandExecutionScheduler(float delayTimer, Command cmnd, float duration)
    {
        ServiceStack st = new ServiceStack(delayTimer, Time.time, cmnd, duration);
        commandStack.Add(st);
        if (!commandRunning)
        {
            commandRunning = true;
            OnServiceTick += CommandWorker;
        }
    }
    public void CommandExecutionScheduler(float delayTimer, Command cmnd, float duration, UnityAction callback)
    {
        ServiceStack st = new ServiceStack(delayTimer, Time.time, cmnd, duration, callback);
        commandStack.Add(st);
        if (!commandRunning)
        {
            commandRunning = true;
            OnServiceTick += CommandWorker;
        }
    }
    public ServiceStack CommandExecutionService(float delayTimer, Command cmnd, float duration)
    {
        ServiceStack st = new ServiceStack(delayTimer, Time.time, cmnd, duration);
        commandStack.Add(st);
        if (!commandRunning)
        {
            commandRunning = true;
            OnServiceTick += CommandWorker;
        }
        return st;
    }
}
public class ServiceStack
{
    public float executionTimer;
    public float startTime;
    public float duration;
    public event Command MyCommand;
    public UnityAction callBackOnFinished;
    public ServiceStack(float execTimer, float strtTime, Command cmnd)
    {
        MyCommand += cmnd;
        startTime = strtTime;
        executionTimer = execTimer;
        duration = 0;
    }
    public ServiceStack(float execTimer, float strtTime, Command cmnd, float duration_)
    {
        MyCommand += cmnd;
        startTime = strtTime;
        executionTimer = execTimer;
        duration = duration_;
    }
    public ServiceStack(float execTimer, float strtTime, Command cmnd, float duration_,  UnityAction callBack)
    {
        MyCommand += cmnd;
        startTime = strtTime;
        executionTimer = execTimer;
        duration = duration_;
        callBackOnFinished = callBack;
    }
    public bool CheckTimer(float time)
    {
        if (time - startTime <= executionTimer)
        {
            return false;
        }
        if (duration >= 0)
        {
            duration -= Time.deltaTime;
            MyCommand?.Invoke();
            return false;
        }
        else
        {
            return true;
        }
    }
    public float GetCurrentProgress()
    {
        return Time.time - startTime;
    }
    public void Clear()
    {
        MyCommand = null;
    }
}