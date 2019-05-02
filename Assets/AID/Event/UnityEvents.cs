using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    [System.Serializable]
    public class UnityEventInt : UnityEngine.Events.UnityEvent<int> { }

    [System.Serializable]
    public class UnityEventFloat : UnityEngine.Events.UnityEvent<float> { }

    [System.Serializable]
    public class UnityEventString : UnityEngine.Events.UnityEvent<string> { }

    [System.Serializable]
    public class UnityEventVector2 : UnityEngine.Events.UnityEvent<Vector2> { }

    [System.Serializable]
    public class UnityEventVector3 : UnityEngine.Events.UnityEvent<Vector3> { }

    [System.Serializable]
    public class UnityEventVector4 : UnityEngine.Events.UnityEvent<Vector4> { }

    [System.Serializable]
    public class UnityEventQuaternion : UnityEngine.Events.UnityEvent<Quaternion> { }

    [System.Serializable]
    public class UnityEventCollider : UnityEngine.Events.UnityEvent<Collider> { }

    [System.Serializable]
    public class UnityEventCollider2D : UnityEngine.Events.UnityEvent<Collider2D> { }

    [System.Serializable]
    public class UnityEventCollision : UnityEngine.Events.UnityEvent<Collision> { }

    [System.Serializable]
    public class UnityEventCollision2D : UnityEngine.Events.UnityEvent<Collision2D> { }

    [System.Serializable]
    public class UnityEventGameObject : UnityEngine.Events.UnityEvent<GameObject> { }

    [System.Serializable]
    public class UnityEventTransform : UnityEngine.Events.UnityEvent<Transform> { }
}