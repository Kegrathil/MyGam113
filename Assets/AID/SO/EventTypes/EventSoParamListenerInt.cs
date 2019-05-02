using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AID
{
    /// <summary>
    /// Simple example of how to feed the EventSOParamListener the required concrete classes. 
    /// Pairs with EventSOParamInt
    /// </summary>
    public class EventSoParamListenerInt : EventSOParamListener<EventSOParamInt, UnityEventInt, int>
    {
    }
}