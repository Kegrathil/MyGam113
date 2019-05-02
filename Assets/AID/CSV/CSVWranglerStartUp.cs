using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

//TODO

namespace AID
{
    /*
        This script handles the basics of starting up the CSVWrangler and updating from remote live csvs. It
        can also be used to set up the local copies of the csvs for shipping with the game, this is done by setting
        the live index csv, that matches something like https://docs.google.com/spreadsheet/pub?key=0AnIiDGqoOqoSdER6RmNFc25ObzJQRzVLbTZPazJQSnc&single=true&gid=7&output=csv
        hit the download index file, once this is complete you can hit the download referenced csvs button
        to download all of the csvs the index points to. These are all stored in the Resources/CSVs folder.
        This is the staging area, this script then ensures that these are in the perm assets folder on start
        and from there checks for newer versions that are live, this is all handled within the CSVWrangler scripts
        and uses only the www class so should work fine on all platforms (tested on pc, iOS and Android).

        My demos use googlespreadsheets that have been published on the older format, the new format is
         https://docs.google.com/spreadsheets/d/<KEY>/export?gid=0&format=csv
         see http://stackoverflow.com/questions/21189665/new-google-spreadsheets-publish-limitation

        Note that with google spreadsheets you need to change the sharing to anyone with link can view 
        as well as publishing them.
     */
    public class CSVWranglerStartUp : MonoBehaviour
    {

        public CSVWrangler.Settings settings;

        private bool allInitStarted = false;
        private bool allInitComplete = false;

        public float initInX = 0; //initialisation delay


        public GameObject[] flipActiveWhenInited;


        // Use this for initialization
        void Start()
        {

            Invoke("InitialiseAll", initInX);
        }

        void InitialiseAll()
        {
            OnForceTablesUpdate();
        }

        void Update()
        {

            //todo this could move to using the event instead of polling
            if (allInitStarted && CSVWrangler.Instance().ActiveDownloads.Count == 0 && !allInitComplete)
            {
                UpdateOfCSVsComplete();
            }
        }

        void UpdateOfCSVsComplete()
        {
            allInitComplete = true;

            foreach (GameObject go in flipActiveWhenInited)
                if (go != null) go.SetActive(!go.activeInHierarchy);
        }

        public void OnForceTablesUpdate()
        {
            if (allInitComplete)
            {
                //already finished and forced reset
                allInitComplete = false;
                foreach (GameObject go in flipActiveWhenInited)
                    if (go != null) go.SetActive(!go.activeInHierarchy);
            }

            CSVWrangler.Instance().InitFromSettings(settings);
            allInitStarted = true;
        }

    }
}