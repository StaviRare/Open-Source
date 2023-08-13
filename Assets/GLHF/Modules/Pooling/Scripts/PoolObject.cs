using System;
using System.Collections.Generic;
using UnityEngine;

namespace GLHF.PoolManager
{
    [Serializable]
    public class PoolObject
    {
        public PoolConfig config;
        public GameObject parentObject;
        public List<GameObject> objectList;
    }
}