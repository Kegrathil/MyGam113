using UnityEngine;
using System.Collections;

namespace AID
{
    public class PrintResponse : ABResponse
    {

        public string printThis;

        public override void Fire(SensorResponseRouter r) { Debug.Log(printThis); }
    }
}