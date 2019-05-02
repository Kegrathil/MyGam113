using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AID
{
    /// <summary>
    /// Adds a listing of the current number of elements in a collection base
    /// </summary>
    [CustomEditor(typeof(CollectionBaseSO), true)]
    public class CollectionSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            CollectionBaseSO colBase = target as CollectionBaseSO;
            EditorGUILayout.Space();
            EditorGUILayout.PrefixLabel("Count: " + colBase.Count.ToString());
        }
    }
}