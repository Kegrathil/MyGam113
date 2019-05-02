using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    [CreateAssetMenu(menuName = "SmartPool/BurstParticle")]
    public class BurstParticleSmartPoolHandler : SmartPoolHandler
    {
        public override void PostInstantiate(SmartPool prefabSmartPool, SmartPoolObjectInstance obj)
        {
        }

        public override void PreReturnToPool(SmartPool prefabSmartPool, SmartPoolObjectInstance obj)
        {
            var p = obj.GetComponent<ParticleSystem>();
            if (p != null)
            {
                p.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }

        public override void PreReturnToUser(SmartPool prefabSmartPool, SmartPoolObjectInstance obj)
        {
            var p = obj.GetComponent<ParticleSystem>();
            if(p != null)
            {
                p.time = 0;
                if (!p.isPlaying)
                {
                    p.Play();
                }
            }
        }
    }
}