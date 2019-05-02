using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ShowCSVWranglerStartUpStatus : MonoBehaviour {

	public AID.CSVWranglerStartUp startUp;
	public TextAsset txtFile;
	public string toShow;

	void Start()
	{
        //get the call back for all dls completed

        //handles native types and enums
        // this shows loading a table of data where the col name matches a classes var name
        // the ConvertRowsToObjects uses reflection and convert to out each row as the given object
        // has not been tested with classes that have refs to other classes, this will most likely NOT work

        AID.DeadSimpleCSV csvFromFile = new AID.DeadSimpleCSV(txtFile.text, true);
		
		List<ExampleCSVSerialiseClass> listFromCSV = csvFromFile.ConvertRowsToObjects<ExampleCSVSerialiseClass>();
		
		foreach(ExampleCSVSerialiseClass o in listFromCSV)
		{
			print (o);
		}

        AID.DeadSimpleCSV csvFromList = AID.DeadSimpleCSV.CreateFromList<ExampleCSVSerialiseClass>(listFromCSV);
		print (csvFromList.GetAsCSVString(true));

        AID.CSVWrangler.Instance().CSVWranglerChange += CSVWranglerStateHasChanged;
        AID.CSVWrangler.Instance().Init();
	}

	void OnGUI()
	{
		GUI.Label( new Rect(10,10,300,300), toShow);
	}

	public void OnDowndloadsFinished()
	{
		//this means we can force other systems to update now
		print ("All csvs dl'ed");

		/*

		//CSVWrangler.Instance().GetTable("example")
		DeadSimpleCSV example = new DeadSimpleCSV();

		//we can get a row from a table
		example.GetRow(3);
		example.GetRowWithData("name","steve");

		//we can get a columb from a table
		example.GetColumn("name");
		example.GetColumn(3);

		//we can get a cell
		example.GetCell(3,4);	//or any combo of names n data in their place

		//we can get a cell with a given name in a given row
		example.GetCellWithData("name","steve","age");

		//we can convert a deadsimplecsv into a list of objects
		List<CSVWrangler.IndexRow> tableAsList = new List<CSVWrangler.IndexRow>();
		tableAsList = CSVWrangler.Instance().indexTable.ConvertRowsToObjects<CSVWrangler.IndexRow>();
		*/
	}

	public void CSVWranglerStateHasChanged(AID.CSVWranglerState newState)
	{
		toShow = newState.ToString();
		
		Debug.Log(toShow);
	}
}

public enum ExampleCSVSerialiseEnum
{
	None,
	Monday,
	Tuesday //etc.
}

public class ExampleCSVSerialiseClass
{
	public int anInt;
	public float aFloat;
	public ExampleCSVSerialiseEnum anEnum;
	public string aString;

    public override bool Equals(object y)
    {
        var lhs = this;
        var rhs = (ExampleCSVSerialiseClass)y;
        return lhs.aFloat == rhs.aFloat && lhs.anInt == rhs.anInt && lhs.anEnum == rhs.anEnum && lhs.aString == rhs.aString;
    }

    public override int GetHashCode()
    {
        return anInt.GetHashCode() + aFloat.GetHashCode() + anEnum.GetHashCode() + aString.GetHashCode();
    }

    public override string ToString ()
	{
		return string.Format ("anInt={0}, aFloat={1}, anEnum={2}, aString={3}", anInt, aFloat, anEnum, aString);	
	}
		
}