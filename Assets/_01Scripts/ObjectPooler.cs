using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//using UnityEngine.AddressableAssets;
//using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameLogic
{
    public class ObjectPooler : MonoBehaviour
    {
        public List<PoolObjectHolder> poolObjects = new List<PoolObjectHolder>();
        public static ObjectPooler op { get; private set; }
        private void Awake()
        {
            op = this;
            InitializePool();
        }
        private void InitializePool()
        {
            for (int i = 0; i < poolObjects.Count; i++)
            {
                poolObjects[i].Initialize();
            }
        }

        /// <summary>
        /// pool index 
        /// 0 - Bullet
        /// 1 - BloodHitEffect
        /// 2 - WallHitEffect
        /// </summary>    
        public GameObject GetObjectFromPool(int objectIndex)
        {
            return poolObjects[objectIndex].GetObjectFromPoolSelfExpanding();
        }
        public ParticleSystem TryGetParticle(int objectIndex)
        {
            ParticleSystem ps = poolObjects[objectIndex].GetParticleSystem();
            if (ps != null)
            {
                return ps;
            }
            else
            {
                return null;
            }
        }
        public void ReturnTOPool(int pollIndex, GameObject pllObj)
        {
            //Utility.DebugText($"Returning to pool {pllObj.name}", "red");
            poolObjects[pollIndex].AddTOPool(pllObj);
        }
        [System.Serializable]
        public class PoolObjectHolder
        {
            // public AssetReference myObject;
            public GameObject prefab;
            private List<GameObject> pooledObjects = new List<GameObject>();
            public Stack<GameObject> availableObjectsInPool = new Stack<GameObject>();
            private List<ParticleSystem> specializedObjects = new List<ParticleSystem>();
            public int maxCount;
            public int counter;
        
            bool initialized = false;
        
            public void AddTOPool(GameObject poolPrefab)
            {
                // poolPrefab.SetActive(false);
        
                pooledObjects.Add(poolPrefab);
                availableObjectsInPool.Push(poolPrefab);
                //poolPrefab.GetComponent<IBall>().ResetBall();
                poolPrefab.transform.position = new Vector3(0, 0f, 100f);
                poolPrefab.SetActive(false);
        
            }
            public void Initialize()
            {
                if (!initialized)
                {
                    initialized = true;
                    ExpandPool(maxCount);
                }
            }
            public ParticleSystem GetParticleSystem()
            {
                counter++;
                if (counter > specializedObjects.Count - 1)
                {
                    counter = 0;
                }

                if (specializedObjects != null && specializedObjects.Count > 0)
                {
                    return specializedObjects[counter];
                }
                else
                {
                    ExpandPool(maxCount);
                    counter = 0;
                    return specializedObjects[counter];
                }
            }

            private void ExpandPool(int amount)
            {
                for (int i = 0; i < amount; i++)
                {
                    GameObject tmp = MonoBehaviour.Instantiate(prefab, new Vector3(0, 0, 100f), Quaternion.identity);
                    tmp.SetActive(false);
                    if (tmp.GetComponent<ParticleSystem>() != null)
                    {
                        specializedObjects.Add(tmp.GetComponent<ParticleSystem>());
                    }
                    AddTOPool(tmp);
                }
            }
            public GameObject GetObjectFromPoolSelfExpanding()
            {
        
                GameObject result;
                if (availableObjectsInPool.Count > 0 && availableObjectsInPool.Peek() != null)
                {
                    result = availableObjectsInPool.Pop();
                    //Utility.DebugText($"AVAILABLE RETURNING", "green");
        
                }
                else
                {
                    GameObject tmp = MonoBehaviour.Instantiate(prefab, new Vector3(0, 0f, 100f), Quaternion.identity);
                    tmp.SetActive(false);
                    result = tmp;
                }
                if (availableObjectsInPool.Count <= 50)
                {
                    for (int i = 0; i < 150; i++)
                    {
                        GameObject tmp = MonoBehaviour.Instantiate(prefab, new Vector3(0, 0, 100f), Quaternion.identity);
                        tmp.SetActive(false);
                        AddTOPool(tmp);
                    }
                }
                return result;
            }
        }
    }
}