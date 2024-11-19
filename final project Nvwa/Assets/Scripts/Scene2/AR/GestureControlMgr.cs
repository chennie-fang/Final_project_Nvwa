using System;
using System.Collections;
using System.Collections.Generic;
using Rokid.UXR.Interaction;
using UnityEngine;

public class GestureControlMgr : MonoSingleton<GestureControlMgr>
{
    public List<GestureBase> managers = new List<GestureBase>();
    private void Awake()
    {
        GesEventInput.OnTrackedSuccess += OnTrackedSuccess;
        GesEventInput.OnTrackedFailed += OnTrackedFailed;
        GesEventInput.OnRenderHand += OnRenderHand;
    }

    private void OnDestroy()
    {
        GesEventInput.OnTrackedSuccess -= OnTrackedSuccess;
        GesEventInput.OnTrackedFailed -= OnTrackedFailed;
        GesEventInput.OnRenderHand -= OnRenderHand;
    }

    private void OnTrackedSuccess(HandType handType)
    {
       if(managers.Count < 1) return;
       foreach (var item in managers)
       {
           item.OnTrackedSuccess(handType);
       }
    }

    private void OnTrackedFailed(HandType handType)
    {
        if(managers.Count < 1) return;
        foreach (var item in managers)
        {
            item.OnTrackedFailed(handType);
        }
    }

    private void OnRenderHand(HandType handType, GestureBean gestureBean)
    {
        if(managers.Count < 1 || gestureBean == null) return;

        foreach (var item in managers)
        {
            item.OnRenderHand(handType,gestureBean);
        }
    }

    /// <summary>
    /// 通过名字获取对应的手势脚本
    /// </summary>
    /// <param name="className">手势脚本名</param>
    /// <returns></returns>
    // public GestureBase FindGestureType<T>(string className) where T : Component
    // {
    //     GestureBase foundClass = managers.Find(x => x.GetType().Name == className);  
    //     if (foundClass != null)
    //     {
    //         return foundClass;
    //     }
    //     else
    //     {
    //         GameObject newGesture = new GameObject("className");
    //         newGesture.transform.SetParent(this.transform);
    //         T newComponent = newGesture.AddComponent<T>();
    //         return newComponent as GestureBase;
    //     }
    // }
    
    /// <summary>
    /// 通过泛型获取对应的手势脚本
    /// </summary>
    /// <returns></returns>
    
    public T FindGestureType<T>() where T : GestureBase
    {
        T component = managers.Find(x => x.GetType() == typeof(T)) as T;
        if (component != null)
        {
            return component;
        }
        else
        {
            GameObject newGesture = new GameObject(typeof(T).Name);
            newGesture.transform.SetParent(this.transform);
            T newComponent = newGesture.AddComponent<T>();
            return newComponent ;
        }
    }
}
