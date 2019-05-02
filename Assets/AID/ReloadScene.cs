using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ReloadScene : MonoBehaviour {

	public float reloadIn = 1f;

	// Use this for initialization
	void Start () {
		StartCoroutine(Reload());
	}
	
	IEnumerator Reload()
	{
		yield return new WaitForSeconds(reloadIn);

        //Application.LoadLevel(Application.loadedLevel);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
