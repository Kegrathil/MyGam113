#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace AID
{
    /*
        Attribute Drawer for strings in unity inspector to force them to be of TimeSpan Format: [-][d.]hh:mm:ss[.ff]
    */
    [CustomPropertyDrawer(typeof(NormalisedCurveAttribute))]
    public class NormalisedCurveAttributeDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);
            
            EditorGUI.PropertyField(position, property, label);

            var an = property.animationCurveValue;

            NormalisedCurveAttribute.ClampAnimCurve(an);

            property.animationCurveValue = an;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }
    }
}
#endif//UNITY_EDITOR