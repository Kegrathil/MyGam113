using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]
public class SoundEffectSRR : MonoBehaviour {
	
	public AudioSource source;
	public AudioClip[] clips;
	public int previousPlayed = 0;
	public float volMin = 0.9f, volMax = 1;
	public float pitchMin = 0.95f, pitchMax = 1.05f;
	public bool asOneShot = true;
	
	public void Start()
	{
		if(source == null)
			source = GetComponent<AudioSource>();
	}
	
	public void Next()
	{
		if(clips.Length == 0)
		{
			Debug.LogWarning(gameObject.name + " wants to play srr but there are no clips");
			return;
		}
		
		int r = Random.Range(0,clips.Length-1);//1 less as we don't play the same one twice in a row
		if(r >= previousPlayed)
		{
			r++;
			if(r >= clips.Length)
				r = 0;
		}
		
		source.volume = Random.Range(volMin, volMax);
		source.pitch = Random.Range(pitchMin, pitchMax);
		//good enough for now, in future needs a collection of audio sources so we can actually control pitch
		if(asOneShot)
			source.PlayOneShot(clips[r], source.volume);
		else
		{
			source.clip = clips[r];
			source.Play();
			source.loop = false;
		}
		previousPlayed = r;
	}
}