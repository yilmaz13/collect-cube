using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PoolSystem
{
    public class ObjectPooler : Singleton<ObjectPooler>
    {
        #region Variables

        private StringBuilder _stringBuilder = new StringBuilder(20, 20);
        private PoolList _templatePoolList;
        public List<Pool> poolList = new List<Pool>();

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();
            _SetPool();
            _InstantiatePoolObjects(poolList);
        }

        private void _SetPool()
        {
            try
            {
                _templatePoolList = Resources.Load<PoolList>("ScriptableObjects/Pool/TemplatePool");
                List<Pool> poolList = _templatePoolList.pools;
                foreach (Pool pool in poolList)
                {
                    this.poolList.Add(Pool.CopyOf(pool));
                }
            }
            catch (Exception e)
            {
                Debug.Log("Scene doesnt have a objectPool");
                Console.WriteLine(e);
                throw;
            }
        }

        private void _InstantiatePoolObjects(List<Pool> poolList)
        {
            foreach (var pool in poolList)
            {
                var poolParent = new GameObject
                {
                    name = pool.Prefab.name,
                    transform = { parent = transform }
                };
                pool.StartingParent = poolParent.transform;

                for (var i = 0; i < pool.StartingQuantity; i++)
                {
                    GameObject obj = Instantiate(pool.Prefab, poolParent.transform);
                  //  _stringBuilder.Length = 0;
                  //  _stringBuilder.Append(obj.name[0..^7]);
                  //  _stringBuilder.Append(" ");
                  //  _stringBuilder.Append(i);
                  //  obj.name = _stringBuilder.ToString();
                    pool.PooledObjects.Add(obj);
                    obj.SetActive(false);
                    obj.GetComponent<PoolObject>().Parent = poolParent.transform;
                    obj.GetComponent<PoolObject>().PrefabName = name;
                }
            }
        }

        #endregion

        #region Public Methods

        public void AllGotoPool()
        {
            if (poolList != null)
            {
                foreach (var pool in poolList)
                {
                    foreach (var obj in pool.PooledObjects)
                    {
                        if (obj != null)
                        {
                            obj.GetComponent<PoolObject>().GoToPool();
                        }
                    }
                }
            }
        }

        [ContextMenu("CheckPool")]
        public void CheckPool()
        {
            if (poolList != null)
            {

                foreach (var pool in poolList)
                {
                    for (int i = 0; i < pool.PooledObjects.Count; )
                    {
                        GameObject obj = pool.PooledObjects[i];
                        if (obj == null)
                        {
                            pool.PooledObjects.Remove(obj);
                        }
                        else
                        {
                            i++;
                        }
                    }
                }
            }
        }

        public GameObject Spawn(string poolName, Vector3 position, Transform parentTransform = null)
        {
            // Find the pool that matches the pool name:
            for (var i = 0; i < poolList.Count; i++)
            {
                if (poolList[i].Prefab.name == poolName)
                {
                    foreach (var poolObj in poolList[i].PooledObjects)
                    {
                        if (!poolObj.activeSelf && poolList[i].StartingParent == poolObj.transform.parent)
                        {
                            poolObj.SetActive(true);
                            poolObj.transform.localPosition = position;
                            // Set parent:
                            if (parentTransform) poolObj.transform.SetParent(parentTransform, false);
                            poolObj.GetComponent<PoolObject>().PoolSpawn();
                            return poolObj;
                        }
                    }

                    // If there's no game object available then expand the list by creating a new one:
                    var spawnObj = Instantiate(poolList[i].Prefab, poolList[i].StartingParent);
                    var childCount = poolList[i].StartingParent.childCount;
              //      _stringBuilder.Length = 0;
              //      _stringBuilder.Append(spawnObj.name[0..^7]);
              //      _stringBuilder.Append(" ");
              //      _stringBuilder.Append(childCount);
              //      spawnObj.name = _stringBuilder.ToString();
                    spawnObj.transform.localPosition = position;
                    poolList[i].PooledObjects.Add(spawnObj);
                    spawnObj.GetComponent<PoolObject>().PrefabName = poolName;
                    spawnObj.GetComponent<PoolObject>().PoolSpawn();
                    return spawnObj;
                }

                if (i != poolList.Count - 1) continue;
                Debug.LogError("!!!There's no pool named \"" + poolName);
                return null;
            }

            return null;
        }

        #endregion


        [ContextMenu("AllGotoPool")]
        public void AllGotoPoolTest()
        {
            AllGotoPool();
        }
    }
}