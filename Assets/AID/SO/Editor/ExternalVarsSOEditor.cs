using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AID
{
    /// <summary>
    /// </summary>
    [CustomEditor(typeof(ExternalVarsSO))]
    public class ExternalVarsSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            ExternalVarsSO extVar = target as ExternalVarsSO;

            if(GUILayout.Button("Generate from Local CSV"))
            {
                extVar.GenerateVarsLocalCSV();
            }
            if (GUILayout.Button("Remove & Delete All"))
            {
                extVar.RemoveVars();
            }
            if (GUILayout.Button("Write all to Local CSV"))
            {
                extVar.WriteVarsToLocal();
            }

            EditorGUILayout.Space();
            base.OnInspectorGUI();
        }
    }

}