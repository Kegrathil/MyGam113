using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    /// <summary>
    /// Common base allows for a simple editor for CollectionSO<T>
    /// </summary>
    public abstract class CollectionBaseSO : BaseSO
    {
        public abstract int Count { get; }
    }

    /// <summary>
    /// A ScriptableObject that holds a collection of a type of object. 
    /// 
    /// Objects themselves use a CollectionSOBeh to interact with the collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectionSO<T> : CollectionBaseSO
    {
        [Tooltip("Optional EventSO to that can be fired when an object is added or removed from the collection.")]
        public EventSO OnCollectionChanged;

        /// <summary>
        /// Presence in the list is managed elsewhere this accessor is to allow things like 
        /// find a random spawn point or finding the closest target to attack.
        /// </summary>
        public System.Collections.ObjectModel.ReadOnlyCollection<T> Collection
        {
            get
            {
                return col.AsReadOnly();
            } 
        }

        public override int Count
        {
            get
            {
                return col.Count;
            }
        }

        public T this[int index]
        {
            get
            {
                return col[index];
            }
        }

        public T First
        {
            get
            {
                return col.Front();
            }
        }

        public T Last
        {
            get
            {
                return col.Last();
            }
        }

        public T Random
        {
            get
            {
                return col[UnityEngine.Random.Range(0, col.Count)];
            }
        }

        private List<T> col = new List<T>();

        /// <summary>
        /// As with Add(T obj, bool notifyOnChange) will always notify.
        /// </summary>
        public void Add(T obj)
        {
            Add(obj, true);
        }

        /// <summary>
        /// Add an object uniquely to the collection
        /// </summary>
        /// <param name="notifyOnChange"> If true and an object is added, the OnCollectionChanged EventSO will fire if one has been set.</param>
        public void Add(T obj, bool notifyOnChange)
        {
            if (!col.Contains(obj))
            {
                col.Add(obj);

                if (notifyOnChange)
                    FireOnCollectionChanged();
            }
        }

        /// <summary>
        /// As with Remove(T obj, bool notifyOnChange) but will always notify
        /// </summary>
        public void Remove(T obj)
        {
            Remove(obj, true);
        }

        /// <summary>
        /// Remove object from collection
        /// </summary>
        /// <param name="notifyOnChange"> If true and an object is removed, the OnCollectionChanged EventSO will fire if one has been set.</param>
        public void Remove(T obj, bool notifyOnChange)
        {
            if(col.Remove(obj))
            {
                if (notifyOnChange)
                    FireOnCollectionChanged();
            }
        }

        public bool Contains(T t)
        {
            return col.Contains(t);
        }

        private void FireOnCollectionChanged()
        {
            if(OnCollectionChanged != null)
                OnCollectionChanged.Fire();
        }
    }
}