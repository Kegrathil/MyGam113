using UnityEngine;
using System.Collections;
using System;

namespace AID
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class HexStringAttribute : PropertyAttribute
    {

    }
}