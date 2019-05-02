using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace AID
{
    //inspector allows for easier set up of local copies of live csv files. It simple adds buttons that can 
    //	 be used to download the starting assets into the resources folder so that they can be copied to the 
    //	 staging area on game start. This allows the user to play the game the first time it is opened regardless
    //	 of their internet connectivity. Also means you can choose not to use live csv at all and only give
    //   out the ones bundled in the build.
    [CustomEditor(typeof(CSVWranglerStartUp))]
    [CanEditMultipleObjects]
    public class CSVWranglerStartUpInspector : Editor
    {


        public override void OnInspectorGUI()
        {
            CSVWranglerStartUp my = (CSVWranglerStartUp)target;

            //EditorGUIUtility.LookLikeControls();
            DrawDefaultInspector();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Download index file"))
            {
                FileDownloadWindow.Init();

                FileDownloadWindow.instance.AddToDlQueue(my.settings.liveURL, "CSVs/csvWrangler.csv", FileDownloadWindow.DownloadLocation.ResourceFolder);
            }

            if (File.Exists(Application.dataPath + "/Resources/CSVs/csvWrangler.csv") && GUILayout.Button("Download referenced csvs"))
            {
                //open the csvWrangler.csv into deadsimple csv
                DeadSimpleCSV indexTable = new DeadSimpleCSV(File.ReadAllText(Application.dataPath + "/Resources/CSVs/csvWrangler.csv"), true);

                //dump into classlist
                List<CSVWrangler.IndexRow> rows = indexTable.ConvertRowsToObjects<CSVWrangler.IndexRow>();

                FileDownloadWindow.Init();

                foreach (CSVWrangler.IndexRow row in rows)
                {
                    FileDownloadWindow.instance.AddToDlQueue(row.url, "CSVs/" + row.name + ".csv", FileDownloadWindow.DownloadLocation.ResourceFolder);
                }

            }

            EditorGUILayout.EndHorizontal();
        }

    }
}