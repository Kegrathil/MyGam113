using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(Generator))]
public class GeneratorDrawer : PropertyDrawer
{
	private static readonly float VERT_STEP = 20;
	
	public override void OnGUI (Rect StartPos, SerializedProperty property, GUIContent label) 
	{
		if(property.FindPropertyRelative("generationWindow") == null)
			return;	//silly unity that's not us
			
			
		Rect pos = StartPos;
		pos.height = VERT_STEP;
		
		
		EditorGUI.PrefixLabel(pos, label);
		property.isExpanded = EditorGUI.Foldout(pos,property.isExpanded, GUIContent.none);
		if(!property.isExpanded)
			return;
		
		EditorGUIUtility.labelWidth = 150;
		
		EditorGUI.BeginProperty(pos, label, property);
		EditorGUI.PrefixLabel(pos, label);
		pos.y += VERT_STEP;
		pos.width = EditorGUIUtility.currentViewWidth - 20;

		EditorGUI.indentLevel++;

		EditorGUI.PropertyField(pos, property.FindPropertyRelative("generationWindow"), new GUIContent("Gen Window"));
		pos.y += VERT_STEP;
		EditorGUI.PropertyField(pos, property.FindPropertyRelative("rerangeOutGoing"), new GUIContent("ReRange Output"));
		pos.y += VERT_STEP;
		EditorGUI.PropertyField(pos, property.FindPropertyRelative("clampToOutputRange"), new GUIContent("Clamp Output"));
		pos.y += VERT_STEP;

		bool reranging = property.FindPropertyRelative("rerangeOutGoing").boolValue;
		bool clamping = property.FindPropertyRelative("clampToOutputRange").boolValue;
		if(reranging || clamping)
		{
			EditorGUI.PropertyField(pos, property.FindPropertyRelative("outgoingWindow"), new GUIContent("Out Window"));
			pos.y += VERT_STEP;
		}
		
		EditorGUI.PropertyField(pos, property.FindPropertyRelative("shift"), new GUIContent("Shift"));
		pos.y += VERT_STEP;
		
		EditorGUI.PropertyField(pos, property.FindPropertyRelative("combineOp"), new GUIContent("Combine Operation"));
		pos.y += VERT_STEP;
		EditorGUI.PropertyField(pos, property.FindPropertyRelative("genMethod"), new GUIContent("Generation Method"));
		pos.y += VERT_STEP;
		
		bool showCurve = property.FindPropertyRelative("genMethod").enumNames[property.FindPropertyRelative("genMethod").enumValueIndex] == "AnimCurve";
		bool showPerlin = property.FindPropertyRelative("genMethod").enumNames[property.FindPropertyRelative("genMethod").enumValueIndex] == "PerlinNoise";
		
		if(showCurve)
		{
			EditorGUI.PropertyField(pos, property.FindPropertyRelative("curve"), GUIContent.none);
			pos.y += VERT_STEP;
		}
		
		if(showPerlin)
		{
			EditorGUI.PropertyField(pos, property.FindPropertyRelative("perlinOffset"), new GUIContent("Perlin Offset"));
			pos.y += VERT_STEP;
		}
		
		EditorGUI.PropertyField(pos, property.FindPropertyRelative("speed"), new GUIContent("Speed"));
		pos.y += VERT_STEP;
		
		EditorGUI.EndProperty();
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		if(!property.isExpanded)
			return VERT_STEP;
	
		float height = 160;
		
		bool reranging = property.FindPropertyRelative("rerangeOutGoing").boolValue;
		bool clamping = property.FindPropertyRelative("clampToOutputRange").boolValue;
		if(reranging || clamping)
		{
			height += VERT_STEP;
		}
		
		bool showCurve = property.FindPropertyRelative("genMethod").enumNames[property.FindPropertyRelative("genMethod").enumValueIndex] == "AnimCurve";
		if(showCurve)
		{
			height += VERT_STEP;
		}
		bool showPerlin = property.FindPropertyRelative("genMethod").enumNames[property.FindPropertyRelative("genMethod").enumValueIndex] == "PerlinNoise";
		if(showPerlin)
		{
			height += VERT_STEP;
		}
		return height;
	}
}