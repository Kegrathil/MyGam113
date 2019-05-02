using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    /// <summary>
    /// EventSO that sends a single int through with the event, making its delegate
    ///   void OnEventFired(EventSOParamInt origin, int param);
    ///   
    /// See EventSoParamListenerInt for an example of a how to set up a listener
    /// </summary>
    [CreateAssetMenu(menuName = "Event/Int Param")]
    public class EventSOParamInt : EventSOParam<int>
    {
    }
}