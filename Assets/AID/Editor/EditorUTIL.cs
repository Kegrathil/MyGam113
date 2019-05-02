using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AID
{
    public partial class EditorUTIL : MonoBehaviour
    {
        public static void PingElements<T>(List<T> list)
        {
            PingElements<T>(list.AsReadOnly());
        }

        public static void PingElements<T>(System.Collections.ObjectModel.ReadOnlyCollection<T> list)
        {
            List<GameObject> gos = new List<GameObject>();

            foreach (var item in list)
            {
                var mItem = item as MonoBehaviour;
                if (mItem != null)
                {
                    gos.Add(mItem.gameObject);
                }
            }

            if (gos.Count > 0)
                Selection.objects = gos.ToArray();
        }
    }
}