using UnityEngine;

[CreateAssetMenu(fileName = "NewSpeechBubbleType", menuName = "Speech/Speech Bubble Type")]
public class SpeechBubbleType : ScriptableObject
{
    public GameObject prefab;

    [Header("Style")]
    public Sprite background;
    public Font font;

    [Header("Colour")]
    public Color textColour;
    public Color bubbleTint;
}
