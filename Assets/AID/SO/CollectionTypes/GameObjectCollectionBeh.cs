using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    /// <summary>
    /// Monobehaviour that will add the parent gameobject to a given GameObjectCollectionSO
    /// </summary>
    public class GameObjectCollectionBeh : CollectionSOBeh<GameObject, GameObjectCollectionSO>
    {
        public override GameObject GetObjectForCollection()
        {
            return gameObject;
        }
    }
}