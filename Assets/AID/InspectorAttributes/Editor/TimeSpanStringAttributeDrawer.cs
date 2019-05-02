using UnityEngine;
using UnityEditor;
using System.Collections;

namespace AID
{
    /*
        Attribute Drawer for strings in unity inspector to force them to be of TimeSpan Format: [-][d.]hh:mm:ss[.ff]
    */

    [CustomPropertyDrawer(typeof(TimeSpanStringAttribute))]
    public class TimeSpanStringAttributeDrawer : PropertyDrawer
    {

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            //is string valid now, if not make it valid
            string val = property.stringValue;
            try
            {
                System.TimeSpan.Parse(val);
            }
            catch (System.Exception)
            {
                val = new System.TimeSpan().ToString();
            }

            //draw it
            var content = new GUIContent(label.text, "Format: [-][d.]hh:mm:ss[.ff]");
            EditorGUI.PropertyField(position, property, content);


            //if the changes are invalid then revert
            string updatedVal = property.stringValue;
            try
            {
                System.TimeSpan.Parse(updatedVal);
            }
            catch (System.Exception)
            {
                Debug.Log(updatedVal + " is not a valid timespan, See System.TimeSpan ToString reference");
                updatedVal = val;
            }

            property.stringValue = updatedVal;

            EditorGUI.EndProperty();
        }
    }
}