using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace AID
{
    //currently using reflection as enable doesn't stem from a single common base class within unity
    public class ComponentEnableResponse : ABResponse
    {
        [SerializeField]
        public List<UnityEngine.Object> enableThese = new List<UnityEngine.Object>();
        [SerializeField]
        public List<UnityEngine.Object> disableThese = new List<UnityEngine.Object>();
        [SerializeField]
        public List<UnityEngine.Object> toggleThese = new List<UnityEngine.Object>();

        public override void Fire(SensorResponseRouter r)
        {
            SetAll(enableThese, true);
            SetAll(disableThese, false);

            foreach (UnityEngine.Object o in toggleThese)
            {
                System.Type t = o.GetType();

                FieldInfo f = t.GetField("enable");

                if (f != null)
                {
                    f.SetValue(o, !(bool)f.GetValue(o));
                    break;
                }

                PropertyInfo p = t.GetProperty("enable");

                if (p != null)
                {
                    p.SetValue(o, !(bool)p.GetValue(o, null), null);
                }
            }
        }

        private void SetAll(List<UnityEngine.Object> list, bool b)
        {
            foreach (UnityEngine.Object o in list)
            {
                System.Type t = o.GetType();

                FieldInfo f = t.GetField("enable");

                if (f != null)
                {
                    f.SetValue(o, b);
                    break;
                }

                PropertyInfo p = t.GetProperty("enable");

                if (p != null)
                    p.SetValue(o, b, null);
            }
        }
    }
}