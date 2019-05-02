using UnityEngine;
using UnityEditor;
using System.Collections;


namespace AID
{
    /*
        Attribute Drawer for strings in unity inspector to force them to be of DateTime Format 'o': 2008-06-15T21:15:07.0000000
    */
    [CustomPropertyDrawer(typeof(DateTimeStringAttribute))]
    public class DateTimeStringAttributeDrawer : PropertyDrawer
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
                System.DateTime.Parse(val);
            }
            catch (System.Exception)
            {
                val = new System.DateTime().ToString("o");
            }

            //draw it
            var content = new GUIContent(label.text, "Format: 2008-06-15T21:15:07.0000000");
            EditorGUI.PropertyField(position, property, content);


            //if the changes are invalid then revert
            string updatedVal = property.stringValue;
            try
            {
                System.DateTime.Parse(updatedVal);
            }
            catch (System.Exception)
            {
                Debug.Log(updatedVal + " is not a valid DateTime, See System.DateTime ToString reference");
                updatedVal = val;
            }

            property.stringValue = updatedVal;

            EditorGUI.EndProperty();
        }
    }
}