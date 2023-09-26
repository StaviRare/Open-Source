using System;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviourRegistry
{
    private static Dictionary<Type, MonoBehaviour> _registeredInstances = new Dictionary<Type, MonoBehaviour>();

    public static T Get<T>() where T : MonoBehaviour
    {
        T returnValue = null;
        var instanceType = typeof(T);

        if (_registeredInstances.TryGetValue(instanceType, out var instance))
        {
            if (instance != null)
            {
                returnValue = (T)instance;
            }
            else
            {
                Debug.LogWarning("Instance \"" + instanceType + "\" is null. Returning null.");
            }
        }
        else
        {
            Debug.LogWarning("Could not find \"" + instanceType + "\" instance");
        }

        return returnValue;
    }

    public static void Register(MonoBehaviour instance)
    {
        if (instance == null)
        {
            Debug.LogWarning("Cannot register null instance!");
        }
        else
        {
            var instanceType = instance.GetType();

            if (_registeredInstances.ContainsKey(instanceType))
            {
                Debug.LogWarning("Instance \"" + instanceType + "\" already exists! Destroying gameObject \"" + instance.name + "\"");
                UnityEngine.Object.Destroy(instance.gameObject);
            }
            else
            {
                _registeredInstances.Add(instanceType, instance);
                Debug.Log("\"" + instanceType + "\" instance has been successfully registered!");
            }
        }
    }

    public static void Unregister(MonoBehaviour instance)
    {
        if (instance == null)
        {
            Debug.LogWarning("Cannot unregister null instance!");
        }
        else
        {
            var instanceType = instance.GetType();

            if (_registeredInstances.ContainsKey(instanceType) && _registeredInstances[instanceType] == instance)
            {
                _registeredInstances.Remove(instanceType);
                Debug.Log("\"" + instanceType + "\" instance has been successfully unregistered!");
            }
            else
            {
                Debug.LogWarning("Instance \"" + instanceType + "\" is not registered or does not match the provided instance.");
            }
        }
    }
}
