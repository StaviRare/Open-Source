using UnityEngine;
using System;

namespace GLHF.PoolManager
{
    [Serializable]
    public class PoolConfig
    {
        public int poolSize;
        public string poolName;
        public bool poolCanExpand;
        public GameObject poolPrefab;
    }
}