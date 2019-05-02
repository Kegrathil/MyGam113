using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class CSVEditModeTest {


    // Use the Assert class to test conditions.
    string fileContents = "anInt,aFloat,anEnum,aString\n1,3.14,Monday,hello world\n2,1.61,Tuesday,this is also a string\n";

    AID.DeadSimpleCSV csvFromFile;
    List<ExampleCSVSerialiseClass> listFromCSV;
    AID.DeadSimpleCSV csvFromList;
    List<ExampleCSVSerialiseClass> listFromCSVFromList;
    string csvFromListStr;

    [OneTimeSetUp]
    public void SetupTests()
    {
        csvFromFile = new AID.DeadSimpleCSV(fileContents, true);

        listFromCSV = csvFromFile.ConvertRowsToObjects<ExampleCSVSerialiseClass>();

        csvFromList = AID.DeadSimpleCSV.CreateFromList(listFromCSV);

        listFromCSVFromList = csvFromList.ConvertRowsToObjects<ExampleCSVSerialiseClass>();

        csvFromListStr = csvFromList.GetAsCSVString(true);
    }

    [Test]
    public void FileContents() {

        //check the string round trip works
        Assert.AreEqual(fileContents, csvFromListStr);
    }

    [Test]
    public void ListFromCSV()
    {
        //heck tha the elements of the list match
        for (int i = 0; i < listFromCSV.Count; i++)
        {
            if (!listFromCSV[i].Equals(listFromCSVFromList[i]))
                Assert.Fail("List elements created from csvs do not match.");
        }
    }

    [Test]
    public void CSVFromData()
    { 
        //check that the elements of the csv match
        if(!csvFromFile.Equals(csvFromList))
        {
            Assert.Fail("CSVs do not match between that from file and that from list");
        }
    }

	// A UnityTest behaves like a coroutine in PlayMode
	//// and allows you to yield null to skip a frame in EditMode
	//[UnityTest]
	//public IEnumerator CSVEditModeTestWithEnumeratorPasses() {
	//	// Use the Assert class to test conditions.
	//	// yield to skip a frame
	//	yield return null;
	//}
}
