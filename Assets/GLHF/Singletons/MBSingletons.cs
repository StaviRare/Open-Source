using System.Collections.Generic;
using System;
using UnityEngine;

namespace GLHF.Singletons
{
    // MonoBehaviour based instance (singleton) manager
    public static class MBSingletons
    {
        private const string logHeader = "[Singleton Manager] ";
        private static Dictionary<Type, MonoBehaviour> sharedInstances = new Dictionary<Type, MonoBehaviour>();

        public static T Get<T>() where T : MonoBehaviour
        {
            var instanceType = typeof(T);
            var instanceDoesExist = sharedInstances[instanceType] != null;

            if (instanceDoesExist)
            {
                return (T)sharedInstances[instanceType];
            }
            else
            {
                Debug.LogWarning(logHeader + "Could not find \"" + instanceType + "\" instance");
                return null;
            }
        }

        public static void Register<T>(T instance) where T : MonoBehaviour
        {
            if (instance == null)
            {
                Debug.LogWarning(logHeader + "Can not register null instance!");
                return;
            }

            var instanceType = instance.GetType();
            var instanceDoesNotExist = !sharedInstances.ContainsKey(instanceType);

            if (instanceDoesNotExist)
            {
                sharedInstances.Add(instanceType, instance);
            }
            else
            {
                if (sharedInstances[instanceType] == null)
                {
                    sharedInstances[instanceType] = instance;
                    Debug.LogWarning(logHeader + "A replacement for instance \"" + instanceType + "\" has been filled by gameObject \"" + instance.name + "\"");
                }
                else
                {
                    // Since we are not interested in another instance of the same type, we will delete it
                    UnityEngine.Object.Destroy(instance.gameObject);
                    Debug.LogWarning(logHeader + "Instance \"" + instanceType + "\" already exists! Destroying gameObject \"" + instance.name + "\"");
                }
            }
        }
    }
}