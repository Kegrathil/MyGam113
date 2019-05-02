using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AID
{
    public class SmartPool
    {
        public GameObject Prefab { get; private set; }
        public GameObject Container { get; private set; }
        private SmartPoolHandler handler;

        public bool IsWanted { get; set;}
        public int TotalPooledObjects { get { return availableInstances.Count + unavailableInstances.Count; } }
        
        //objects that are in stock
        public Stack<SmartPoolObjectInstance> availableInstances = new Stack<SmartPoolObjectInstance>();

        //objects that are out
        public LinkedList<SmartPoolObjectInstance> unavailableInstances = new LinkedList<SmartPoolObjectInstance>();



        public SmartPool(GameObject thePrefab)
        {
            Prefab = thePrefab;
            handler = Prefab.GetComponent<SmartPoolObjectInstance>().poolConfig;
            if(handler == null)
            {
                Debug.LogError("Cannot find SmartPoolConfig on " + thePrefab.name);
            }

            Container = new GameObject("[SmartPool]" + Prefab.name);
            Container.transform.parent = AID.SmartPoolManager.Instance.transform;
            handler.Init(this);
        }

        //Get
        public SmartPoolObjectInstance GetInstance()
        {
            SmartPoolObjectInstance retval = null;
            if(availableInstances.Count == 0)
            {
                //notify of request when none avail
                handler.RequestWhenNoneAvail(this);
            }

            if (availableInstances.Count > 0)
            {
                retval = availableInstances.Pop();

                unavailableInstances.AddLast(retval);
            }

            return PrepareForReturnToUser(retval);
        }
        

        //Return
        public void ReturnToPool(SmartPoolObjectInstance obj)
        {
            var res = unavailableInstances.Remove(obj);

            if (res)
            {
                PrepareBeforeInsertToAvailable(obj);
                availableInstances.Push(obj);
            }
            //TODO these elses don't need to be in builds
            else
            {
                if (availableInstances.Contains(obj))
                {
                    Debug.LogWarning("Attempted to return GO that is already marked as available");
                }
                else
                {
                    Debug.LogError("Attempted to return GO that doesn't belong to this pool");
                }
            }
        }

        public void RecallAllRented()
        {
            //TODO this method is slow but probably not a problem
            while (unavailableInstances.Count > 0)
            {
                var obj = unavailableInstances.First.Value;
                unavailableInstances.RemoveFirst();

                //do the return
                PrepareBeforeInsertToAvailable(obj);
                availableInstances.Push(obj);
            }
        }

        public void GrowPoolTo(int totalObjects)
        {
            GrowPoolBy(TotalPooledObjects - totalObjects);
        }

        public void GrowPoolBy(int additionalObjects)
        {
            for (int i = 0; i < additionalObjects; i++)
            {
                CreateNewInstance();
            }
        }

        public SmartPoolObjectInstance CreateNewInstance()
        {
            var newGO = Object.Instantiate(Prefab).GetComponent<SmartPoolObjectInstance>();
            PrepareAfterInstantiation(newGO);
            PrepareBeforeInsertToAvailable(newGO);
            availableInstances.Push(newGO);
            return newGO;
        }

        //Shrink pool to
        public void ShrinkPoolTo(int newTotalInstances)
        {
            ShrinkPoolBy(TotalPooledObjects - newTotalInstances);
        }

        public void ShrinkPoolBy(int numToRemove)
        {
            for (int i = 0; i < numToRemove && availableInstances.Count > 0; i++)
            {
                var go = availableInstances.Pop();
                Object.Destroy(go);
            }
        }

        public void DestroyAll()
        {
            while(availableInstances.Count > 0)
            {
                var obj = availableInstances.Pop();
                handler.DestroyGO(obj);
            }
            availableInstances.Clear();


            while (unavailableInstances.Count > 0)
            {
                var g = unavailableInstances.Last.Value;
                handler.DestroyGO(g);
                unavailableInstances.RemoveLast();
            }
            unavailableInstances.Clear();
        }

        //prep object for return
        private SmartPoolObjectInstance PrepareForReturnToUser(SmartPoolObjectInstance obj)
        {
            if (obj != null)
            {
                obj.IsRented = true;
#if UNITY_EDITOR
                obj.gameObject.transform.parent = null;
#endif
                handler.PreReturnToUser(this,obj);
            }

            return obj;
        }

        private void PrepareAfterInstantiation(SmartPoolObjectInstance obj)
        {
            obj.Pool = this;
            handler.PostInstantiate(this, obj);
        }

        private void PrepareBeforeInsertToAvailable(SmartPoolObjectInstance obj)
        {
            handler.PreReturnToPool(this, obj);
            obj.IsRented = false;
#if UNITY_EDITOR
            obj.transform.parent = Container.transform;
#endif

        }

        public void EmptyIfUnwanted()
        {
            if(!IsWanted)
                DestroyAll();
        }
    }
}