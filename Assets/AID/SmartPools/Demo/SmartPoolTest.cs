using UnityEngine;
using System.Collections;

public class SmartPoolTest : MonoBehaviour {

    //public PrefabSmartPool psp;
    public GameObject[] prefabs;

	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetKey(KeyCode.D))
        {
            AID.SmartPoolManager.Instance.DestroyAll();
        }
        else if (Input.GetKey(KeyCode.B))
        {
            AID.SmartPoolManager.Instance.RecallAllRented();
        }
        else if(Input.GetKey(KeyCode.S) || Input.GetKeyDown(KeyCode.O))
        {

            for (int i = 0; i < prefabs.Length; i++)
            {

                var g = prefabs[i].SmartClone();

                g.transform.parent = transform;
                g.transform.localPosition = Random.insideUnitSphere * 10;
            }
        }
    }
}
