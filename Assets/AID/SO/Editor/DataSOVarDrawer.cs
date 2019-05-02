using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AID
{
    /// <summary>
    /// Property drawer that handles showing the local and ref values of a DataTypeSOVar on 1 line.
    /// 
    /// Due to DataTypeSOVarBase most DataTypeSOVar<T,U> that have simple drawers will work via the
    /// base drawer, should you need more control you can inherit from this and specialise DrawLocal
    /// or entirely replace OnGUI
    /// </summary>
    [CustomPropertyDrawer(typeof(DataTypeSOVarBase), true)]
    public class DataSOVarDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var refval = property.FindPropertyRelative("refVal");

            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            var width = (position.width - 5) / 4;
            var remWidth = position.width;

            //if local show that larger, if ref show that larger
            if (refval.objectReferenceValue == null)
            {
                position.width = width * 3;
                DrawLocalVar(position, property);// EditorGUI.PropertyField(position, localVal, GUIContent.none);
            }
            else
            {
                //we have a reference so show a preview of current value
                position.width = width;
                var refValStringified = refval.objectReferenceValue.ToString();
                GUI.Label(position, refValStringified);
            }

            //take remaining space to show the var itself
            position.x += position.width + 5;
            position.width = remWidth - position.width - 5;
            EditorGUI.PropertyField(position, refval, GUIContent.none);
            EditorGUI.EndProperty();
        }

        public virtual void DrawLocalVar(Rect position, SerializedProperty property)
        {
            var localVal = property.FindPropertyRelative("localVal");
            EditorGUI.PropertyField(position, localVal, GUIContent.none);
        }
    }
}