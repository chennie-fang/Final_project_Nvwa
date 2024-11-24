using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                
                if (FindObjectsOfType<T>().Length > 1)
                {
                    Debug.LogError("More than 1!");
                    return instance;
                }

                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    string instanceName = typeof(T).Name;
                    Debug.Log("Instance Name: " + instanceName);
                    GameObject instanceGO = GameObject.Find(instanceName);

                    if (instanceGO == null)
                        instanceGO = new GameObject(instanceName);

                    instance = instanceGO.AddComponent<T>();
                }
                else
                {
                    Debug.Log("instance: " + instance.name);
                }
            }

            return instance;
        }
    }

    protected virtual void OnDestroy()
    {
        instance = null;
    }
}
