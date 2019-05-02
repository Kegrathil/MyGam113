using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace AID
{
    /// <summary>
    /// Singleton that is auto created when needed, entry point into all smart pools
    /// </summary>
    [AddComponentMenu("")]
    public class SmartPoolManager : MonoBehaviour
    {
        #region singleton stuff
        static private SmartPoolManager inst;

        public void Awake()
        {
            if (inst != null)
                DestroyImmediate(this);
            else
                inst = this;
        }
        
        //this is how you get access to this classes functions
        static public SmartPoolManager Instance
        {
            get
            {
                if (inst != null)
                    return inst;

                return inst = AID.UTIL.GetAutoGameRules().GetOrAddComponent<SmartPoolManager>();
            }
        }

        

        #endregion

        private Dictionary<int, SmartPool> pools = new Dictionary<int, SmartPool>();

        public SmartPoolObjectInstance GetInstance(GameObject prefab)
        {
            var res = GetPool(prefab);

            if (res != null)
            {
                return res.GetInstance();
            }

            return null;
        }

        public void DestroyAll()
        {
            foreach (var pool in pools.Values)
            {
                pool.DestroyAll();
            }
        }

        public void RecallAllRented()
        {
            foreach (var pool in pools.Values)
            {
                pool.RecallAllRented();
            }
        }

        public SmartPool GetOrCreatePool(GameObject prefab)
        {
            SmartPool pool = GetPool(prefab);
            if (pool != null)
                return pool;

            if (pool == null)
            {
                pool = new SmartPool(prefab);
                pools[prefab.GetInstanceID()] = pool;
            }

            return pool;
        }

        public void PreparePool(GameObject prefab, int amt)
        {
            if (prefab == null)
                return;

            var p = GetOrCreatePool(prefab);
            p.GrowPoolTo(amt);
        }

        public SmartPool GetPool(GameObject prefab)
        {
            SmartPool pool = null;
            pools.TryGetValue(prefab.GetInstanceID(), out pool);

            if(pool == null)
            {
                pool = new SmartPool(prefab);
                pools.Add(prefab.GetInstanceID(), pool);
            }
            return pool;
        }

        public void MarkAllAsUnwanted()
        {
            foreach (var pool in pools.Values)
            {
                pool.IsWanted = false;
            }
        }

        public void MarkWantedByPrefab(GameObject prefab)
        {
            var res = GetPool(prefab);

            if(res != null) 
                res.IsWanted = true;
        }

        public void EmptyAllUnwatedPools()
        {
            foreach (var pool in pools.Values)
            {
                pool.EmptyIfUnwanted();
            }
        }

        void Start()
        {
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg1 == LoadSceneMode.Single)
                RecallAllRented();
        }
    }
}