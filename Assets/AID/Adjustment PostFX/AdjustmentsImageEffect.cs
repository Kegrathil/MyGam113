using UnityEngine;
using System.Collections;


//todo gamma correction built in as last step
[ExecuteInEditMode]
public class AdjustmentsImageEffect : MonoBehaviour
{

    private Material material;
    [HideInInspector]
    public Shader adjustmentsLDRShader;

    public bool enableUnsharp = false;
    [Range(0.0f, 5.0f)]
    public float unsharpRadius = 0.95f;
    [Range(0.0f, 50.0f)]
    public float unsharpInten = 1.3f;
    [Range(0.0f, 1)]
    public float unsharpThreshold = 0.015f;

    public bool enableLevels = false;
    [Range(0, 255)]
    public int min = 0, max = 255;
    [Range(0.01f, 9.99f)]
    public float midToneGamma = 1;

    public bool enableVibrance = false;
    [Tooltip("Recommend no less than -0.5 but upper limit is to taste")]
    public float vibrance = 1.22f;

    public bool enableHSV = false;
    [Range(-1.0f, 1.0f)]
    public float hueShift, saturationShift, valueShift;

    public bool enablePhotoFilter = false;
    public Color photoFilterColor = Color.red;
    [Range(0.0f, 1.0f)]
    public float photoFilterintensity;
    public bool photoFilterPreserveLuminosity;

    public bool enableGammaCorrection = false;
    public float gammaScale = 1;

    public bool enableNoise = false;
    public Texture2D noiseTex;
    public float noiseIntensity = 0.53f, noiseScale = 1;

    public bool enableVignette = false;
    public Color vignetteColor = Color.black;
    //[Range(0.0f,2.0f)]
    public float vignetteScale = 0.9f, vignetteOffset = 0.45f, vignettePower = 2.03f;
    public Vector2 vignetteSquish = Vector2.one;

    void Awake()
    {
        if (adjustmentsLDRShader == null)
            adjustmentsLDRShader = Shader.Find("Hidden/AdjustmentsImageEffectLDR");

        material = new Material(adjustmentsLDRShader);
    }

    void OnRenderImage(RenderTexture source, RenderTexture dest)
    {
        material.SetFloat("_Enable_Unsharp", enableUnsharp ? 1 : 0);
        if (enableUnsharp)
        {
            material.SetFloat("_Unsharp_Radius", unsharpRadius);
            material.SetFloat("_Unsharp_Intensity", unsharpInten);
            material.SetFloat("_Unsharp_Threshold", unsharpThreshold);
        }

        material.SetFloat("_Enable_Levels", enableLevels ? 1 : 0);
        if (enableLevels)
        {
            material.SetFloat("_Min", min / 255.0f);
            material.SetFloat("_Max", max / 255.0f);
            material.SetFloat("_MidToneGamma", 1.0f / midToneGamma);
        }

        material.SetFloat("_Enable_Vibrance", enableVibrance ? 1 : 0);
        if (enableVibrance)
        {
            material.SetFloat("_Vibrance", vibrance);
        }

        material.SetFloat("_Enable_HSV", enableHSV ? 1 : 0);
        if (enableHSV)
        {
            material.SetVector("_HSVShift", new Vector4(Mathf.Repeat(hueShift, 1), saturationShift, valueShift, 0));
        }

        material.SetFloat("_Enable_PhotoFilter", enablePhotoFilter ? 1 : 0);
        if (enablePhotoFilter)
        {
            material.SetVector("_PhotoFilter", new Vector4(photoFilterColor.r, photoFilterColor.g, photoFilterColor.b, photoFilterintensity));
            material.SetFloat("_PhotoFilter_Luma", photoFilterPreserveLuminosity ? 1 : 0);
        }

        material.SetFloat("_Enable_Vignette", enableVignette ? 1 : 0);
        if (enableVignette)
        {
            material.SetVector("_Vignette_Color", new Vector4(vignetteColor.r, vignetteColor.g, vignetteColor.b, vignetteColor.a));
            material.SetFloat("_Vignette_Scale", vignetteScale);
            material.SetFloat("_Vignette_Offset", vignetteOffset);
            material.SetFloat("_Vignette_Power", vignettePower);
            material.SetVector("_Vignette_Squish", vignetteSquish);
        }

        material.SetFloat("_Enable_Noise", enableNoise ? 1 : 0);
        if (enableNoise)
        {
            material.SetTexture("_Noise_Texture", noiseTex == null ? Texture2D.blackTexture : noiseTex);
            material.SetFloat("_Noise_Intensity", noiseIntensity);
            material.SetFloat("_Noise_Scale", noiseScale);

            //move and rotate uvs by some big primes
            Vector2 offset = new Vector2((Time.realtimeSinceStartup % 5.0f ) * 761, (Time.realtimeSinceStartup % 11.0f) * 1021);

            material.SetVector("_Noise_Offset", offset);
        }

        material.SetFloat("_Enable_Gamma_Correction", enableGammaCorrection ? 1 : 0);
        if (enableGammaCorrection)
        {
            material.SetFloat("_GammaCorrection", gammaScale);
        }

        Graphics.Blit(source, dest, material);
    }
}

//TODO
//	do we need output levels?
