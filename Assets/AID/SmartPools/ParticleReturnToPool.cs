using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    public class ParticleReturnToPool : MonoBehaviour
    {
        public SmartPoolObjectInstance poolInst;

        void OnParticleSystemStopped()
        {
            if (poolInst == null)
                poolInst = GetComponent<SmartPoolObjectInstance>();

            poolInst.ReturnToPool();
        }
    }
}