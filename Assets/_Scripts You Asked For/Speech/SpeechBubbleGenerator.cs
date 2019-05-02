using UnityEngine;
using UnityEngine.UI;

namespace SpeechGeneration
{
    public class SpeechBubbleGenerator : MonoBehaviour
    {
        public static void GenerateSpeechBubble(SpeechBubbleType bubble, Vector3 position, Transform speaker, string message)
        {
            GameObject go = Instantiate(bubble.prefab, speaker);
            go.transform.localPosition = position;
            go.transform.localEulerAngles = Vector3.zero;

            Image image = go.transform.GetChild(0).GetComponent<Image>();
            Text text = go.transform.GetChild(0).GetChild(0).GetComponent<Text>();

            image.sprite = bubble.background;
            image.color = bubble.bubbleTint;
            text.text = message;
        }
    }
}