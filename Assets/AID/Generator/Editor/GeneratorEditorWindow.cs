//using UnityEngine;
//using UnityEditor;
//using System.Collections;
//using System.Reflection;
//
//
//namespace AID{
//	public class GeneratorEditorWindow : EditorWindow {
//		
//		public static GeneratorEditorWindow instance;
//		
//		public MonoBehaviour targetedScript;
//		public string generatorVarName;
//		
//		public Collector gen;
//		
//		
//		// Add to the Window menu
//		[MenuItem ("Window/Generator Creator")]
//		public static void Init () {
//			if(instance == null)
//			{
//				// Get existing open window or if none, make a new one:
//				GeneratorEditorWindow window = (GeneratorEditorWindow)EditorWindow.GetWindow (typeof (GeneratorEditorWindow));
//				instance = window;
//			}
//			
//			if(instance.gen == null)
//				instance.gen = new Collector();
//			
//			instance.Show();
//			instance.Focus();
//		}
//		
//		void OnGUI () {
//			targetedScript = (MonoBehaviour)EditorGUILayout.ObjectField("Component with Collector:",targetedScript, typeof(MonoBehaviour),true);
//			generatorVarName = EditorGUILayout.TextField("Collector variable name: ", generatorVarName);
//			
//			DoButtons();
//			
//			DrawPreview();
//		}
//		
//		void DoButtons()
//		{
//			EditorGUILayout.BeginHorizontal();
//			if(targetedScript != null && !string.IsNullOrEmpty(generatorVarName) && GUILayout.Button("load from Component"))
//			{
//				PropertyInfo info = targetedScript.GetType().GetProperty(generatorVarName);
//				
//				if(info != null)
//				{
//					gen = (Collector)info.GetValue(targetedScript,null);
//				}
//				else
//				{
//					FieldInfo finfo = targetedScript.GetType().GetField(generatorVarName);
//					
//					if(finfo != null)
//					{
//						gen = (Collector)finfo.GetValue(targetedScript);
//					}
//					else
//					{
//						Debug.Log("No Collector of name " + generatorVarName + " found in component.");
//					}
//				}
//			}
//			if(targetedScript != null && !string.IsNullOrEmpty(generatorVarName) && GUILayout.Button("Save To Component"))
//			{
//				PropertyInfo info = targetedScript.GetType().GetProperty(generatorVarName);
//				
//				if(info != null)
//				{
//					info.SetValue(targetedScript,gen,null);
//				}
//				else
//				{
//					FieldInfo finfo = targetedScript.GetType().GetField(generatorVarName);
//					
//					if(finfo != null)
//					{
//						finfo.SetValue(targetedScript,gen);
//					}
//					else
//					{
//						Debug.Log("No Collector of name " + generatorVarName + " found in component.");
//					}
//				}
//			}
//			
//			EditorGUILayout.EndHorizontal();
//		}
//		
//		void DrawPreview()
//		{
//			if(gen == null)
//				gen = new Collector();
//				
//			const float numSamples = 100;
//			
//			float xstep = Screen.width/numSamples;
//			
//			float starty = 200;
//			float yScale = 120;
//			float prevGen = 0;
//			for(int i = 0; i < numSamples-1; i++)
//			{
//				float generated = gen.Generate(1/xstep)*yScale;
//				Handles.DrawLine(new Vector2(xstep*i, starty+prevGen),
//				                 new Vector2(xstep*(i+1), starty+generated));
//				prevGen = generated;
//			}
//			
//		}
//	}
//}	