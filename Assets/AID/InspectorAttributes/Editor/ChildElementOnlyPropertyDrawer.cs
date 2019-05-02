using UnityEngine;
using UnityEditor;
using System.Collections;

namespace AID
{
    public abstract class ChildElementOnlyPropertyDrawer : PropertyDrawer
    {
        public abstract string GetChildElementName();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative(GetChildElementName()), label, true);
        }

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            var bucketName = property.displayName;

            property = property.FindPropertyRelative(GetChildElementName());

            EditorGUI.PropertyField(position, property, new GUIContent(bucketName), true);

            EditorGUI.EndProperty();
        }
    }
}