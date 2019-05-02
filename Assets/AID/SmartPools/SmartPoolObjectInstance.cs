using UnityEngine;
using System.Collections;

namespace AID
{
    /// <summary>
    /// Data required per smart pool instance and link to the configuration data
    /// </summary>
    public class SmartPoolObjectInstance : MonoBehaviour
    {
        public SmartPool Pool { get; set; }
        public bool IsRented { get; set; }
        public SmartPoolHandler poolConfig;

        public void ReturnToPool()
        {
            if(IsRented)
                Pool.ReturnToPool(this);
        }
    }
}

public static partial class GameOjectExtensionMethods
{
    public static void SmartDestroy(this GameObject go)
    {
        if (go == null)
            return;

        var smoi = go.GetComponent<AID.SmartPoolObjectInstance>();

        if (smoi != null)
        {
            smoi.ReturnToPool();
        }
        else
        {
            Object.Destroy(go);
        }
    }

    public static GameObject SmartClone(this GameObject go)
    {
        if (go == null)
            return null;

        var smoi = go.GetComponent<AID.SmartPoolObjectInstance>();

        if (smoi != null)
        {
            return AID.SmartPoolManager.Instance.GetInstance(go).gameObject;
        }
        else
        {
            return Object.Instantiate(go);
        }
    }
}