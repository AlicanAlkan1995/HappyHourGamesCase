using System;
using UnityEngine;
using System.Collections.Generic;

namespace HappyHourGames.Object_Pooling
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private List<PoolInfo> poolList = new List<PoolInfo>();
        
        public void FillPool()
        {
            foreach (var poolInfo in poolList)
            {
                FillPool(poolInfo);
            }
        }

        /// <summary>
        /// Fills the pool according to the given pool info
        /// </summary>
        /// <param name="info"></param>
        private void FillPool(PoolInfo info)
        {
            for (var i = 0; i < info.amount; i++)
            {
                var poolObj = Instantiate(info.prefab, transform);
                poolObj.SetActive(false);
                info.pooledObjects.Enqueue(poolObj);
            }
        }

        /// <summary>
        /// Gets pool object.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public GameObject GetPoolObject(PoolObjectType type)
        {
            var selectedPool = GetPoolByType(type);

            GameObject poolObj;
            if (selectedPool.pooledObjects.Count > 0)
            {
                poolObj = selectedPool.pooledObjects.Dequeue();
            }
            else
            {
                poolObj = Instantiate(selectedPool.prefab, transform);
            }

            poolObj.SetActive(true);

            return poolObj;
        }

        /// <summary>
        /// Retrieves the object back in its pool by type.
        /// </summary>
        /// <param name="poolObj"></param>
        /// <param name="type"></param>
        public void DestroyObject(GameObject poolObj, PoolObjectType type)
        {
            poolObj.SetActive(false);

            if (poolObj.transform.parent != transform)
            {
                poolObj.transform.SetParent(transform);
            }
            
            var selectedPool = GetPoolByType(type);
            
            if(!selectedPool.pooledObjects.Contains(poolObj))
                selectedPool.pooledObjects.Enqueue(poolObj);
        }

        /// <summary>
        /// Gets the pool based on the type of object.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private PoolInfo GetPoolByType(PoolObjectType type)
        {
            foreach (var poolInfo in poolList)
            {
                if (type == poolInfo.type)
                    return poolInfo;
            }

            return null;
        }
    }
    
}