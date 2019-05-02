using UnityEngine;
using System.Collections;


[ExecuteInEditMode()]
public class CameraHistogramHelper : MonoBehaviour
{
#if UNITY_EDITOR
    [HideInInspector]
    public int[]
      red = new int[256],
      green = new int[256],
      blue = new int[256],
      lum = new int[256];

    [HideInInspector]
    public int maxValue = 0;
    
    public object chw;  //CameraHistogramWindow

    protected Texture2D tex;

    void ClearAll()
    {
        for (int i = 0; i < red.Length; i++)
        {
            red[i] = 0;
            green[i] = 0;
            blue[i] = 0;
            lum[i] = 0;
        }
    }

    void FindMax()
    {
        maxValue = 0;
        for (int i = 0; i < red.Length; i++)
        {
            maxValue = Mathf.Max(maxValue, red[i]);
            maxValue = Mathf.Max(maxValue, green[i]);
            maxValue = Mathf.Max(maxValue, blue[i]);
            maxValue = Mathf.Max(maxValue, lum[i]);
        }
    }


    // Update is called once per frame
    IEnumerator PullSceneColorData()
    {
        yield return new WaitForEndOfFrame();

        if (tex == null || tex.width != Screen.width || tex.height != Screen.height)
        {
            tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false, false);
        }
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        tex.Apply();

        ClearAll();
        var pixels = tex.GetPixels32();

        for (int i = 0; i < pixels.Length; i++)
        {
            var pixel = pixels[i];
            ++red[pixel.r];
            ++green[pixel.g];
            ++blue[pixel.b];
            byte lumForPixel = (byte)(0.2126f * pixel.r + 0.7152f * pixel.g + 0.0722f * pixel.b);
            ++lum[lumForPixel];
        }

        FindMax();
    }

    void Update()
    {
        if (chw != null)
        {
            StartCoroutine(PullSceneColorData());
        }
    }
#endif//UNITY_EDITOR
    }
