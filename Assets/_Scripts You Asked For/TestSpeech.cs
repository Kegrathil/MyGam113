using SpeechGeneration;
using UnityEngine;

public class TestSpeech : MonoBehaviour
{
    public SpeechBubbleType type;

    public GameObject player;

    private void Update()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < 4)
        {
            SpeechBubbleGenerator.GenerateSpeechBubble(type, new Vector3(0, 1.7f, 0), transform, "Yay! You made it!");

            GetComponent<TestSpeech>().enabled = false;
            /*
            every time you would like to create a speech bubble, you must call:
            SpeechBubbleGenerator.GenerateSpeechBubble([ScriptableObjectOfWhatYouWantItToLookLike], Vector3[OffsetPosition], Transform[ThingThatIsSpeaking], "[WhatYouWantToSay");

            also, you must include: using SpeechGeneration; at the top of the script.

            Enjoy!
            */
        }
    }
}
