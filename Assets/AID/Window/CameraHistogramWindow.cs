using UnityEngine;
using System.Collections;
using System.Reflection;
using UnityEditor;

//mostly adapted from http://wiki.unity3d.com/index.php/EditorGraphWindow


public class CameraHistogramWindow : EditorWindow
{
    Material lineMaterial;
    readonly static int WINDOW_MIN_HEIGHT = 100;
    readonly static int WINDOW_MIN_WIDTH = 256;
    readonly static int WINDOW_PADDING = 5;

    CameraHistogramHelper helper;

   

    // Add to the Window menu
    [MenuItem("Window/Camera Histogram")]
    public static void Init()
    {
        // Get existing open window or if none, make a new one:
        CameraHistogramWindow window = (CameraHistogramWindow)EditorWindow.GetWindow(typeof(CameraHistogramWindow));
        window.Show();
        window.Focus();
        window.minSize = new Vector2(WINDOW_MIN_WIDTH + 2 * WINDOW_PADDING, WINDOW_MIN_HEIGHT + 2 * WINDOW_PADDING);
    }

    void OnEnable()
    {
        EditorApplication.update += MyDelegate;
    }

    void OnDisable()
    {
        EditorApplication.update -= MyDelegate;
        if(helper != null)
            DestroyImmediate(helper.gameObject);
    }

    void MyDelegate()
    {
        //PullSceneColorData();
        Repaint();
    }

    void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            lineMaterial = (Material)typeof(GUI).GetMethod("get_blitMaterial",  BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
        }
    }

    void OnGUI()
    {
        if (Event.current.type != EventType.Repaint)
            return;

        if (helper == null)
        {
            helper = FindObjectOfType<CameraHistogramHelper>();

            if (helper == null)
                helper = new GameObject("_temp_histogram_helper").AddComponent<CameraHistogramHelper>();

            helper.chw = this;
        }

        //DrawSquare( );

        CreateLineMaterial();
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        //GL.LoadPixelMatrix();

        GL.Begin(GL.LINES);


        //for (int chan = 0; chan < Graph.MAX_CHANNELS; chan++)
        //{
        //    Channel C = Graph.channel[chan];

        //    if (C == null)
        //        Debug.Log("FOO:" + chan);

        //    if (!C.isActive)
        //        continue;

        //    GL.Color(C._color);

        //    for (int h = 0; h < Graph.MAX_HISTORY; h++)
        //{

        var yScalingFactor = (float)(this.position.height - WINDOW_PADDING*2) / helper.maxValue;

        GL.Color(Color.red);
        DrawAll(helper.red, yScalingFactor);
        GL.Color(Color.green);
        DrawAll(helper.green, yScalingFactor);
        GL.Color(Color.blue);
        DrawAll(helper.blue, yScalingFactor);
        GL.Color(Color.white);
        DrawAll(helper.lum, yScalingFactor);

        //int xPix = (W - 1) - h;

        //if (xPix >= 0)
        //{
        //    float y = C._data[h];

        //    float y_01 = Mathf.InverseLerp(Graph.YMin, Graph.YMax, y);

        //    int yPix = (int)(y_01 * H);

        //    Plot(xPix, yPix);
        //}
        //}
        //}

        GL.End();

        GL.PopMatrix();
    }

    void DrawAll(int[] data, float yScaleFactor)
    {
        float bottomPixel = this.position.height - WINDOW_PADDING;
        var xScalingFactor = (this.position.width - WINDOW_PADDING * 2) / 256.0f;

        for (int i = 0; i < data.Length; i++)
        {
            GL.Vertex3(WINDOW_PADDING + i * xScalingFactor, bottomPixel, 0);
            GL.Vertex3(WINDOW_PADDING + i * xScalingFactor, bottomPixel - (data[i]* yScaleFactor), 0);
        }
    }

}