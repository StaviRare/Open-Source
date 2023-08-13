using System.Collections.Generic;
using UnityEngine;

namespace GLHF.PoolManager
{
    public static class PoolManager
    {
        private static Transform _poolContainer;
        private static List<PoolObject> _poolList = new List<PoolObject>();


        public static void CreatePool(PoolConfig poolConfig)
        {
            if (_poolContainer == null)
            {
                _poolContainer = new GameObject("Pool Container").transform;
            }

            var poolDoesNotExists = GetPoolObjectByName(poolConfig.poolName) == null;

            if (poolDoesNotExists)
            {
                var newPool = new PoolObject()
                {
                    config = poolConfig,
                    parentObject = new GameObject(poolConfig.poolName),
                    objectList = new List<GameObject>()
                };

                newPool.parentObject.transform.parent = _poolContainer;

                for (int i = 0; i < poolConfig.poolSize; i++)
                {
                    InstantiateObjectInPool(newPool);
                }

                _poolList.Add(newPool);
            }
            else
            {
                Debug.Log("Pool Name: \"" + poolConfig.poolName + "\" already exists!");
            }
        }

        public static void DestroyPool(PoolConfig poolConfig)
        {
            var specifiedPool = _poolList.Find(x => x.config == poolConfig);

            if (specifiedPool != null)
            {
                _poolList.Remove(specifiedPool);
                Object.Destroy(specifiedPool.parentObject);
            }
        }

        public static PoolConfig GetPoolConfigByName(string poolName)
        {
            var specifiedPool = _poolList.Find(x => x.config.poolName == poolName);
            return specifiedPool.config;
        }

        public static GameObject GetPoolObjectByName(string poolName)
        {
            var specifiedPool = _poolList.Find(x => x.config.poolName == poolName);
            var returnValue = specifiedPool == null ? null : GetAvailablePoolObject(specifiedPool);

            return returnValue;
        }

        public static GameObject GetPoolObjectByConfig(PoolConfig poolConfig)
        {
            var specifiedPool = _poolList.Find(x => x.config == poolConfig);
            var returnValue = specifiedPool == null ? null : GetAvailablePoolObject(specifiedPool);

            return returnValue;
        }


        private static GameObject InstantiateObjectInPool(PoolObject poolItemList)
        {
            var poolPrefab = poolItemList.config.poolPrefab;
            var poolParent = poolItemList.parentObject.transform;
            var instantiatedPoolObject = Object.Instantiate(poolPrefab, poolParent);

            instantiatedPoolObject.SetActive(false);
            poolItemList.objectList.Add(instantiatedPoolObject);

            return instantiatedPoolObject;
        }

        private static GameObject GetAvailablePoolObject(PoolObject poolObject)
        {
            for (int i = 0; i < poolObject.objectList.Count; i++)
            {
                var objectIsAvailable = 
                    poolObject.objectList[i] != null 
                    && poolObject.objectList[i].activeInHierarchy == false;

                if (objectIsAvailable)
                {
                    return poolObject.objectList[i];
                }
            }

            if (poolObject.config.poolCanExpand)
            {
                return InstantiateObjectInPool(poolObject);
            }
            else
            {
                return null;
            }
        }
    }
}