using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

//TODO
//	needs example index page
//  Tool to auto gen class structure from a given csv
//  Tool to auto gen a csv template from a given class

namespace AID
{
    public enum CSVWranglerState
    {
        NotReady,
        Staging,
        IndexOnly,
        CheckingIndex,
        ProcessingIndex,
        Downloading,
        Ready,
        Loading,
        ErrorNoIndex
    }


    /*
     *  As the name suggests this class wrangles a collection of csvs (probably google spreadsheets or
     *  custome hosted) it does this by checking an index page which tells it all the other tables that are required
     * 
     * See CSVWranglerStartUp for more information
     */
    public class CSVWrangler : MonoBehaviour
    {

        [System.Serializable]
        public class Settings
        {
            public string liveURL;
            public bool useLiveUpdates = true;//if false will only init not check against live index
            public bool alwaysDownloadTables = false;// setting to true will dl live tables regardless of versioning
            public int updateIfXDaysStale = 1;
        }

        public Settings settings = new Settings();


        [SerializeField]
        public DeadSimpleCSV indexTable;

        [SerializeField]
        private Dictionary<string, DeadSimpleCSV> tables = new Dictionary<string, DeadSimpleCSV>();


        //delegate formate to subscribe to state changes, most likely want to know when it is ready so app can progress from splash
        public delegate void CSVWranglerChangeState(CSVWranglerState state);

        //+= your handler to this to be notified when state is changed
        public event CSVWranglerChangeState CSVWranglerChange;


        private List<WWW> activeDownloads = new List<WWW>();
        public List<WWW> ActiveDownloads
        {
            get
            {
                return activeDownloads;
            }
        }

        private CSVWranglerState state = CSVWranglerState.NotReady;
        public CSVWranglerState State
        {
            get
            {
                return state;
            }
            set
            {
                if (state != value)
                {
                    state = value;
                    OnCSVWranglerChange();
                }
            }
        }

        private void OnCSVWranglerChange()
        {
            if (CSVWranglerChange != null)
                CSVWranglerChange(State);
        }

        //find table of given name. Should be called sparingly, if you need the table a lot then cache it
        //	DeadSimpleCSV myTable = CSVWrangler.Instance().GetTable("Example");
        public DeadSimpleCSV GetTable(string name)
        {
            DeadSimpleCSV retval = null;
            tables.TryGetValue(name, out retval);

            if (retval == null)
                Debug.LogWarning("No table with name " + name);

            return retval;
        }

        //if you are using live udpates, then use the CSVWranglerStartUp to pass settings down
        public void InitFromSettings(Settings s)
        {
            settings = s;

            if (!string.IsNullOrEmpty(settings.liveURL) && settings.useLiveUpdates)
            {
                UpdateFromIndexSmart(settings.liveURL);
            }
            else
            {
                Init();
            }
        }

        //if you are not using live data then you only need to call this, loads default tables
        public void Init()
        {
            if (State != CSVWranglerState.NotReady)
                return;

            ForceInit();
        }

        //this checks the local index against the live url index given, downloads new or updated or all if forced csvs 
        //	and stores in Application persist data path any newer versions that are listed in the index
        public void UpdateFromIndexSmart(string indexURL)
        {
            DateTime cur = DateTime.Now;
            DateTime prevLoad = cur;
            //force stale
            prevLoad.AddDays(-2);
            DateTime.TryParse(PlayerPrefs.GetString("CSVWrangler.lastLiveIndexCSVCheck", ""), out prevLoad);

            if ((cur - prevLoad).Days >= settings.updateIfXDaysStale || settings.alwaysDownloadTables)
            {
                UpdateFromIndex(indexURL);
            }
            else
            {
                Init();
            }
            PlayerPrefs.SetString("CSVWrangler.lastLiveIndexCSVCheck", DateTime.Now.ToString("o"));
        }

        //this checks the local index against the live url index given, downloads new or updated or all if forced csvs 
        //	and stores in Application persist data path any newer versions that are listed in the index
        public void UpdateFromIndex(string indexURL)
        {
            ForceInitIndex();

            State = CSVWranglerState.CheckingIndex;
            StartCoroutine(_updateFromIndex(indexURL));
        }

        //checks that files exist in persistent storage area and load the csvWrangler csv into curTable, then load those tables
        //	currently synchronous, may become async in the future
        public void ForceInit()
        {
            ForceInitIndex();
            if(State == CSVWranglerState.IndexOnly)
                LoadTables();
        }

        public void ForceInitIndex()
        {
            string csvIndexLoc = Application.persistentDataPath + "/csvWrangler.csv";
            //ForceCacheLocalFiles();
            if(!ForceCacheIndex())
            {
                Debug.LogError("CSV Wrangler is trying to be used without a local cached csvWrangler.csv in the Resources. Did you remember to cache your csvs?");
                State = CSVWranglerState.ErrorNoIndex;
                return;
            }

            //load previous csvwrangler index csv file
            indexTable = new DeadSimpleCSV(UTIL.ReadAllText(csvIndexLoc), true);

            //something terrible happened fake an empty one
            if (indexTable.headers == null || indexTable.headers.Length == 0)
            {
                indexTable.headers = new string[] { "name", "url", "ver" };
            }

            State = CSVWranglerState.IndexOnly;
        }

        //load local tables from persist storage as described in the curtable, which is loaded from the csvwrangler.csv
        //	currently synchronous, in the future may become async
        public void LoadTables()
        {
            State = CSVWranglerState.Loading;

            //reset so we could reload
            tables = new Dictionary<string, DeadSimpleCSV>();

            //grab the row and load each file of name into the tables dict
            string[] tableNames = indexTable.GetColumn("name");

            foreach (string name in tableNames)
            {
                string tableFullLoc = Application.persistentDataPath + "/" + name + ".csv";

                //TODO could change this logic so the file contents is returned by the util function so we don't double load?
                //make sure it exists
                UTIL.EnsureTextResourceIsHere(tableFullLoc, "csvs/" + name);

                //attempt to load
                string fileContents = UTIL.ReadAllText(tableFullLoc);

                //notify of failure
                if (string.IsNullOrEmpty(fileContents))
                {
                    Debug.LogWarning("Tried to load the " + name + " csv but something went wrong");
                }
                else//success, maybe
                {
                    DeadSimpleCSV table = new DeadSimpleCSV(fileContents,true);
                    tables.Add(name, table);
                }
            }

            State = CSVWranglerState.Ready;
        }

        //flush all tables to disc
        public void SaveTables()
        {
            foreach (KeyValuePair<string, DeadSimpleCSV> item in tables)
            {
                UTIL.WriteAllText(Application.persistentDataPath + "/" + item.Key + ".csv", item.Value.GetAsCSVString(true));
            }

            _flushUpdatedTable(false);
        }

        public void UpdateTable(string name, string URL, string ver)
        {
            StartCoroutine(_updateTable(name, URL, ver));
        }



        //singleton stuff
        static private CSVWrangler inst;

        public void Awake()
        {
            inst = this;
        }

        //this is how you get access to this classes functions
        static public CSVWrangler Instance()
        {
            if (inst != null)
                return inst;

            return inst = UTIL.GetAutoGameRules().GetOrAddComponent<CSVWrangler>();
        }



        //fetch given live url index page
        private IEnumerator _updateFromIndex(string indexURL)
        {
            WWW www = new WWW(indexURL);
            activeDownloads.Add(www);

            yield return www;

            bool isValid = string.IsNullOrEmpty(www.error);

            activeDownloads.Remove(www);

            if (isValid)
                ProcessIndex(new DeadSimpleCSV(www.text, true));
            else
            {
                Debug.LogWarning("CSVWrangler could not reach live index file " + indexURL + "\nGot error " + www.error);
                Debug.LogWarning("Loading local tables");
                LoadTables();
            }
        }

        private IEnumerator _flushUpdatedTable(bool loadWhenComplete)
        {
            while (activeDownloads.Count > 0)
            {
                yield return new WaitForSeconds(0.1f);
            }

            string updatedTable = indexTable.GetAsCSVString(true);
            //Debug.Log(updatedTable);
            UTIL.WriteAllText(Application.persistentDataPath + "/csvWrangler.csv", updatedTable);

            if (loadWhenComplete)
                LoadTables();
        }


        private void ProcessIndex(DeadSimpleCSV newTable)
        {
            State = CSVWranglerState.ProcessingIndex;

            //make a list of all name col names
            string[] curNames = indexTable.GetColumn("name"), newNames = newTable.GetColumn("name");

            List<string> colnames = new List<string>();

            colnames.AddRange(curNames.ToList());
            colnames.AddRange(newNames.ToList());

            colnames = colnames.Distinct().ToList();

            //this is all a bit too table-ey and memory trashy but it is a table and it is a one off init process

            //handles all new rows from the given index, updates, creates and destroys as required
            _processAllNewRows(colnames, newTable);

            //flush the index file back to storage
            StartCoroutine(_flushUpdatedTable(true));
        }

        private void _processAllNewRows(List<string> colnames, DeadSimpleCSV newTable)
        {
            //for all
            foreach (string targetName in colnames)
            {
                string[] curRow = indexTable.GetRowWithData("name", targetName);
                string[] newRow = newTable.GetRowWithData("name", targetName);

                if (curRow == null)
                {
                    //we don't have it add the version and download
                    if (newRow != null)
                    {
                        UpdateTable(targetName,
                                    newTable.GetCell("url", newRow),
                                    newTable.GetCell("ver", newRow));

                        Debug.Log(targetName + " did not exist, creating from live.");
                    }
                }
                else if (newRow == null)
                {
                    Debug.Log(targetName + " no longer exists in live, removing.");
                    //this table doesn't exist anymore, drop it
                    indexTable.RemoveRow(curRow);

                    //we only try to load files that are in the latest csvwrangler index so we can delete this old one now
                    File.Delete(Application.persistentDataPath + "/" + targetName + ".csv");
                }
                else
                {
                    //if the version in ours is lower download 
                    int curVer = int.Parse(indexTable.GetCell("ver", curRow));// curRow[curTable.ColumnNames["ver"]]);
                    int newVer = int.Parse(newTable.GetCell("ver", newRow));

                    if (newVer > curVer || settings.alwaysDownloadTables)
                    {

                        Debug.Log(targetName + (newVer > curVer ? " was old, updating" : " was forced to update"));
                        //download new ver
                        UpdateTable(targetName,
                                    newTable.GetCell("url", newRow),
                                    newTable.GetCell("ver", newRow));
                    }
                }
            }
        }

        private IEnumerator _updateTable(string name, string URL, string ver)
        {
            State = CSVWranglerState.Downloading;

            Debug.Log(name + ".csv is downloading");

            WWW www = new WWW(URL);
            activeDownloads.Add(www);
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                UTIL.WriteAllText(Application.persistentDataPath + "/" + name + ".csv", www.text);

                //we know the order its name, url, ver
                //get the row
                string[] targetRow = indexTable.GetRowWithData("name", name);

                //doesn't exist create it
                if (targetRow == null)
                {
                    targetRow = new string[3];
                    targetRow[0] = name;
                    indexTable.AddRow(targetRow);
                }

                targetRow[1] = URL;
                targetRow[2] = ver;
            }

            activeDownloads.Remove(www);
        }

        private bool ForceCacheIndex()
        {
            return UTIL.EnsureTextResourceIsHere(Application.persistentDataPath + "/csvWrangler.csv", "csvs/csvWrangler");
        }

        //semi internal class represents 1 row of the csvWrangler index file
        public class IndexRow
        {
            public string name, url;
            public int ver;
        }
    }
}