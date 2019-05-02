using UnityEngine;
using System.Collections;
using AID;

public class VerifiledFileTest : MonoBehaviour {

    public string testData = "";

	// Use this for initialization
	void Start () {
        VerifiedFileUtil.SaveFileWithSignature(testData, "output.txt", "a really good salt");

        VerifiedFileUtil.ReadFileCheckSignature("output.txt", "a really good salt");
        VerifiedFileUtil.ReadFileCheckSignature("output_nosig.txt", "a really good salt");
        VerifiedFileUtil.ReadFileCheckSignature("output_usereditted.txt", "a really good salt");
        VerifiedFileUtil.ReadFileCheckSignature("output_thatdoesntexist.txt", "a really good salt");

        VerifiedFileUtil.ReadFileCheckSignature("output.txt", "a different but potentially equally good salt that fails");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
