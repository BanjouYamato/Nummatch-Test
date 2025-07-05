using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public static Observer Instance { get; private set; }
    Dictionary<string, List<Delegate>> actionListt = new();

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void AddToObser(string name, Action action)
    {
        if (!actionListt.ContainsKey(name))
        {
            actionListt.Add(name, new List<Delegate>());
        }
        actionListt[name].Add(action);
    }
    public void AddToObser<T>(string name, Action<T> action)
    {
        if (!actionListt.ContainsKey(name))
        {
            actionListt.Add(name, new List<Delegate>());
        }
        actionListt[name].Add(action);
    }

    public void RemoveObser(string name, Action action)
    {
        if (!actionListt.ContainsKey(name))
        {
            Debug.LogError("lam gi co thang lon");
            return;
        }
        actionListt[name].Remove(action);
    }
    public void RemoveObser<T>(string name, Action<T> action)
    {
        if (!actionListt.ContainsKey(name))
        {
            Debug.LogError("lam gi co thang lon");
            return;
        }
        actionListt[name].Remove(action);
    }
    public void TriggerAction(string name)
    {
        if (!actionListt.ContainsKey(name))
        {
            Debug.LogError("lam gi co thang lon");
            return;
        }
        foreach (Action action in actionListt[name])
        {
            action?.Invoke();
        }
    }
    public void TriggerAction<T>(string name, T value)
    {
        if (!actionListt.ContainsKey(name))
        {
            Debug.LogError("lam gi co thang lon");
            return;
        }
        foreach (var del in actionListt[name])
        {
            if (del is Action<T> action)
                action?.Invoke(value);
        }
    }
}
