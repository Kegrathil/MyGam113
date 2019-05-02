using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    /// <summary>
    /// MonoBehaviour that handles adding and removing to a CollectionSO based on enable and disable.
    /// 
    /// See GameObjectCollectionBeh as an example
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U">Concrete class that is CollectionSO<T>, required so we can show it in the inspector</typeparam>
    public abstract class CollectionSOBeh<T, U> : MonoBehaviour where U : CollectionSO<T>
    {
        [Tooltip("Collection to place in. An object can be in many.")]
        public U collection;

        /// <summary>
        /// Required to know how to get that object into the collection. 
        /// 
        /// For GameObject, this would simply be return gameObject;
        /// </summary>
        /// <returns></returns>
        public abstract T GetObjectForCollection();

        private void OnEnable()
        {
            collection.Add(GetObjectForCollection());
        }

        private void OnDisable()
        {
            collection.Remove(GetObjectForCollection());
        }
    }
}