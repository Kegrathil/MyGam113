using UnityEngine;
using UnityEditor;
using System.Collections;

//TODO 

[CustomEditor(typeof(GeneratorDriver))]
[CanEditMultipleObjects]
public class GeneratorDriverInspector : Editor {
	
	Texture2D tex2d = null;
	bool overrideShowPreview = false;
	float min, max;
	float previewLength = 1;

	public void OnDestroy()
	{
		DestroyImmediate(tex2d);
	}

	public override void OnInspectorGUI ()
	{
		GeneratorDriver my = (GeneratorDriver)target;
		
		//EditorGUIUtility.LookLikeControls();
		DrawDefaultInspector();

		if(GUILayout.Button("Add Generator"))
		{
			my.generators.Add(new Generator());
		}
			
		DrawGeneratorPreview(my);
	}

	public void DrawGeneratorPreview(GeneratorDriver dr)
	{
		previewLength = EditorGUILayout.FloatField("Preview length scale", previewLength);
		int numSamples = (int)((EditorGUIUtility.currentViewWidth-20)*previewLength);
		int height = 100;
		if(tex2d == null || tex2d.width != numSamples)
		{
			DestroyImmediate(tex2d);
			tex2d = new Texture2D(numSamples,height,TextureFormat.ARGB4444,false);
			tex2d.hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSave | HideFlags.HideInInspector;
		}

		if(dr == null)
			return;
		
		float xstep = 1f/height;
		//Debug.Log(xstep);

		if(Application.isPlaying)
		{
			overrideShowPreview = GUILayout.Toggle(overrideShowPreview,"Preview even in play mode (Alters data)");
		}
		
		if(!Application.isPlaying || overrideShowPreview)
		{
			dr.Reset();
			
			for(int i = 0; i < tex2d.width; i++)
				for(int j = 0; j < tex2d.height; j++)
					tex2d.SetPixel(i,j,new Color(0, 0, 0, 0));
			
			float[] fs = new float[numSamples];
			
			for(int i = 0; i < numSamples; i++)
			{
				float generated = dr.GenerateIncrement(xstep);//gen.Generate(1/xstep)*yScale;
				//Debug.Log(generated);
				//generated = 0;
				fs[i] = generated;
			}
			
			min = Mathf.Min(fs);
			max = Mathf.Max(fs);
			
			for(int i = 0; i < numSamples; i++)
			{
				float f = fs[i];
				int fi = Mathf.Clamp((int)AID.UTIL.ReRange(f,min, max, 0, height),0,height-1);
				int fiu = Mathf.Clamp(fi+1,0,height-1);
				int fid = Mathf.Clamp(fi-1,0,height-1);
				tex2d.SetPixel(i,fi,new Color(1,1,0,1));
				tex2d.SetPixel(i,fiu,new Color(1,1,0,1));
				tex2d.SetPixel(i,fid,new Color(1,1,0,1));
			}
			
			tex2d.Apply();
		}
			
		GUILayout.Label("Window: " + min.ToString() + "-" + max.ToString());
		GUILayout.Label("Length: " + (numSamples/(float)height).ToString());
		GUILayout.Label(new GUIContent(tex2d),GUILayout.Height(height), GUILayout.MinHeight(height), GUILayout.MaxHeight(height));
	}

}
