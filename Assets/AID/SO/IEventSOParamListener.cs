using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    /// <summary>
    /// Interface used by EventSOParam when it raises param event. 
    /// To be used when creating your own listener types. In general a specialised
    /// EventSOParamListener may be all that is required.
    /// </summary>
    public interface IEventSOParamListener<T>
    {
        void OnEventFired(EventSOParam<T> origin,T param);
    }
}