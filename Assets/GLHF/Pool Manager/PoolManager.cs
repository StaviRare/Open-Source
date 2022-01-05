using System.Collections.Generic;
using UnityEngine;

namespace GLHF.PoolManager
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance;

        private List<PoolObject> poolList;

        #region Public 

        public void CreatePool(PoolConfig poolConfig)
        {
            var poolExists
                = GetPoolObjectByName(poolConfig.poolName) != null;

            if (poolExists)
            {
                Debug.Log("Pool Name: \"" + poolConfig.poolName + "\" already exists!");
                return;
            }

            var newPool = new PoolObject()
            {
                config = poolConfig,
                parentObject = new GameObject(poolConfig.poolName),
                objectList = new List<GameObject>()
            };

            for (int i = 0; i < poolConfig.poolSize; i++)
            {
                InstantiateObjectInPool(newPool);
            }

            poolList.Add(newPool);
            newPool.parentObject.transform.parent = transform;
        }

        public void DestroyPool(PoolConfig poolConfig)
        {
            var specifiedPool = poolList.Find(x => x.config == poolConfig);

            if (specifiedPool == null)
                return;

            poolList.Remove(specifiedPool);
            Destroy(specifiedPool.parentObject);
        }

        public PoolConfig GetPoolConfigByName(string poolName)
        {
            var specifiedPool = poolList.Find(x => x.config.poolName == poolName);
            return specifiedPool.config;
        }

        public GameObject GetPoolObjectByName(string poolName)
        {
            var specifiedPool = poolList.Find(x => x.config.poolName == poolName);

            if (specifiedPool == null)
                return null;

            return GetAvailablePoolObject(specifiedPool);
        }

        public GameObject GetPoolObjectByConfig(PoolConfig poolConfig)
        {
            var specifiedPool = poolList.Find(x => x.config == poolConfig);

            if (specifiedPool == null)
                return null;

            return GetAvailablePoolObject(specifiedPool);
        }

        #endregion


        #region Private

        private void Awake()
        {
            SingletonCheck();

            poolList = new List<PoolObject>();
        }

        private void SingletonCheck()
        {
            var instanceAlreadyExists
                = Instance != null && Instance != this;

            if (instanceAlreadyExists)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
        }

        private GameObject InstantiateObjectInPool(PoolObject poolItemList)
        {
            var poolPrefab = poolItemList.config.poolPrefab;
            var poolParent = poolItemList.parentObject.transform;
            var instantiatedPoolObject = Instantiate(poolPrefab, poolParent);

            instantiatedPoolObject.SetActive(false);
            poolItemList.objectList.Add(instantiatedPoolObject);

            return instantiatedPoolObject;
        }

        private GameObject GetAvailablePoolObject(PoolObject poolObject)
        {
            for (int i = 0; i < poolObject.objectList.Count; i++)
            {
                var availableObjectFromPool
                    = !poolObject.objectList[i].activeInHierarchy;

                if (availableObjectFromPool)
                {
                    return poolObject.objectList[i];
                }
            }

            if (poolObject.config.poolCanExpand)
            {
                var obj = InstantiateObjectInPool(poolObject);
                return obj;
            }

            return null;
        }

        #endregion
    }
}