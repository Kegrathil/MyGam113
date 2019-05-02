using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    /// <summary>
    /// Specialised collection for housing gameobject references. 
    /// </summary>
    [CreateAssetMenu(menuName = "Collection/GameObject")]
    public class GameObjectCollectionSO : CollectionSO<GameObject>
    {
    }
}