using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    /// <summary>
    /// Interface used by EventSO when it raises a zero param event. 
    /// To be used when creating your own listener types. In general 
    /// EventSOListener may be all that is required.
    /// </summary>
    public interface IEventSOListener
    {
        void OnEventFired(EventSO origin);
    }
}