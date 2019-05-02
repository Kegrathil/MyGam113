using UnityEngine;
using UnityEditor;

public class GradientContainer : MonoBehaviour
{
    public Gradient grad = new Gradient();
}

//TODO
//  split out static create texture ramp functions with many params so they can be called from anywhere else
namespace AID
{

    public class TextureRampCreatorWindow : EditorWindow
    {
        public enum TextureWidth
        {
            X16 = 16,
            X32 = 32,
            X64 = 64,
            X128 = 128,
            X256 = 256,
            X512 = 512,
        }

        enum Mode { GrayScale, FullColour };

        public static TextureRampCreatorWindow instance;

        private Mode mode = Mode.GrayScale;
        public AnimationCurve curve;// = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

        public string saveAs = "ramp";
        public TextureWidth textureWidth = TextureWidth.X32;

        public GradientContainer gradCont;
        SerializedObject serialColGradObj;
        SerializedProperty serialColGrad;



        // Add to the Window menu
        [MenuItem("Window/Texture Ramp Creator")]
        public static void Init()
        {
            if (instance == null)
            {
                // Get existing open window or if none, make a new one:
                TextureRampCreatorWindow window = (TextureRampCreatorWindow)EditorWindow.GetWindow(typeof(TextureRampCreatorWindow));
                instance = window;
                instance.curve = AnimationCurve.Linear(0, 0, 1, 1);


                //window = window;
            }

            if (instance.gradCont == null)
            {
                instance.PrepareGradient();
            }

            instance.Show();
            instance.Focus();
        }

        void OnGUI()
        {
            //don't actually care about something being selected but if its null curve field throws errors
            if (Selection.activeGameObject == null)
                Selection.activeGameObject = Editor.FindObjectOfType<GameObject>();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Grayscale"))
            {
                mode = Mode.GrayScale;
            }

            if (GUILayout.Button("Gradient"))
            {
                mode = Mode.FullColour;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            if (mode == Mode.GrayScale)
                curve = EditorGUILayout.CurveField("curve", curve);
            else if (mode == Mode.FullColour)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(serialColGrad, true, null);
                if (EditorGUI.EndChangeCheck())
                {
                    serialColGradObj.ApplyModifiedProperties();
                }
            }
            textureWidth = (TextureWidth)EditorGUILayout.EnumPopup("Texture ramp width", (System.Enum)textureWidth);



            EditorGUILayout.Space();
            saveAs = EditorGUILayout.TextField("Save as: ", saveAs);

            if (GUILayout.Button("Create?"))
            {
                CreateTexture();
            }
        }

        void CreateTexture()
        {
            int twPi = (int)textureWidth;

            TextureFormat textFormat = mode == Mode.GrayScale ? TextureFormat.Alpha8 : TextureFormat.RGBA32;
            //texture
            Texture2D newRamp = new Texture2D(twPi, 1, textFormat, false);

            // interp from animcurve
            for (int i = 0; i < twPi; i++)
            {
                float p = i / (float)twPi;
                Color col = Color.black;
                if (mode == Mode.GrayScale)
                {
                    float curveRes = curve.Evaluate(p);
                    col = new Color(0, 0, 0, curveRes);
                }
                else if (mode == Mode.FullColour)
                {
                    col = gradCont.grad.Evaluate(p);
                }
                newRamp.SetPixel(i, 0, col);
            }

            //serialise it
            string localPath = "/" + saveAs + ".png";
            string finalPath = Application.dataPath + localPath;
            localPath = "Assets" + localPath;
            System.IO.File.WriteAllBytes(finalPath, newRamp.EncodeToPNG());


            //we now have to force assetdb to notice it
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

            //force it to be readable
            TextureImporter textImp = AssetImporter.GetAtPath(localPath) as TextureImporter;
            textImp.isReadable = true;
            AssetDatabase.ImportAsset( localPath, ImportAssetOptions.ForceUpdate);

            TextureImporterSettings settings = new TextureImporterSettings();
            textImp.ReadTextureSettings(settings);
            settings.filterMode = FilterMode.Bilinear;
            settings.mipmapEnabled = false;
            settings.textureFormat = mode == Mode.GrayScale ? TextureImporterFormat.Alpha8 : TextureImporterFormat.RGBA32;
            settings.wrapMode = TextureWrapMode.Clamp;
            textImp.SetTextureSettings(settings);

            //force it to notice the changes to import settings
            AssetDatabase.ImportAsset(localPath, ImportAssetOptions.ForceUpdate);

            Selection.activeObject = textImp;
            EditorGUIUtility.PingObject(Selection.activeObject);
        }


        void OnDestroy()
        {
            UnityEngine.Object.DestroyImmediate(gradCont.gameObject);
        }

        public void PrepareGradient()
        {
            //terrible hack to make gradient editor appear
            gradCont = new GameObject("_temp_gradient_container").AddComponent<GradientContainer>();
            serialColGradObj = new SerializedObject(gradCont);
            serialColGrad = serialColGradObj.FindProperty("grad");

            GradientColorKey[] gck = new GradientColorKey[2];
            gck[0].color = Color.black;
            gck[0].time = 0.0f;
            gck[1].color = Color.white;
            gck[1].time = 1.0f;

            GradientAlphaKey[] gak = new GradientAlphaKey[2];
            gak[0].alpha = 1.0f;
            gak[0].time = 0.0f;
            gak[1].alpha = 1.0f;
            gak[1].time = 1.0f;

            gradCont.grad.SetKeys(gck, gak);
            EditorUtility.SetDirty(gradCont);
        }
    }
}