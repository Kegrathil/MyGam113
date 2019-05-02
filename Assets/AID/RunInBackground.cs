using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunInBackground : MonoBehaviour
{
	void Start ()
    {
        Application.runInBackground = true;
	}
}
