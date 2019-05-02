using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Diagnostics;

namespace AID
{
    public class FileDownloadWindow : EditorWindow
    {
        public static FileDownloadWindow instance;

        public enum DownloadLocation
        {
            AssetFolder,
            ResourceFolder,
            TempFolder,
            PersistFolder
        };

        [System.Serializable]
        public class FDInfo
        {
            public WWW www;
            public string name;
            public DownloadLocation saveLocation;
        };

        public string URLToDl = "http://google.com";
        public string nameToSaveAs = "google.html";
        public DownloadLocation saveLocation;
        List<FDInfo> infos = new List<FDInfo>();


        // Add to the Window menu
        [MenuItem("Window/File Downloader")]
        public static void Init()
        {
            // Get existing open window or if none, make a new one:
            FileDownloadWindow window = (FileDownloadWindow)EditorWindow.GetWindow(typeof(FileDownloadWindow));
            window.Show();
            window.Focus();
            instance = window;
            //window = window;
        }

        void OnGUI()
        {
            //		GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
            URLToDl = EditorGUILayout.TextField("URL", URLToDl);
            nameToSaveAs = EditorGUILayout.TextField("Save as", nameToSaveAs);

            EditorGUILayout.BeginHorizontal();
            saveLocation = (DownloadLocation)EditorGUILayout.EnumPopup("Save to", saveLocation);
            if (GUILayout.Button("Open Location"))
            {
                string toOpen = null;
                switch (saveLocation)
                {
                    case DownloadLocation.AssetFolder:
                        toOpen = Application.dataPath + "/";
                        break;
                    case DownloadLocation.ResourceFolder:
                        toOpen = Application.dataPath + "/Resources/";
                        break;
                    case DownloadLocation.TempFolder:
                        toOpen = Application.temporaryCachePath + "/";
                        break;
                    case DownloadLocation.PersistFolder:
                        toOpen = Application.persistentDataPath + "/";
                        break;
                }

                System.Diagnostics.Process.Start(toOpen);
            }

            EditorGUILayout.EndHorizontal();
            //		
            //		groupEnabled = EditorGUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);
            //		myBool = EditorGUILayout.Toggle ("Toggle", myBool);
            //		myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
            //		EditorGUILayout.EndToggleGroup ();


            if (GUILayout.Button("Download"))
            {
                AddToDlQueue(URLToDl, nameToSaveAs, saveLocation);
            }



            GUILayout.Label("Active Downloads:");

            for (int i = 0; i < infos.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(infos[i].name);

                //FKN annoying, www class doesn't give in progress information in a useful way.
                //	in fact some of its properties turn it into a blocking download:(
                //	GUILayout.Label(infos[i].www.bytesDownloaded.ToString());
                //	GUILayout.Label(infos[i].www.progress.ToString());
                EditorGUILayout.EndHorizontal();
            }
        }

        public void AddToDlQueue(string url, string saveas, DownloadLocation saveloc)
        {
            FDInfo newInfo = new FDInfo();
            newInfo.www = new WWW(url);
            newInfo.name = saveas;
            newInfo.saveLocation = saveloc;

            infos.Add(newInfo);
        }

        void OnInspectorUpdate()
        {
            for (int i = 0; i < infos.Count;)
            {
                if (infos[i].www.isDone)
                {
                    HandleFileFinished(infos[i]);
                    infos.RemoveAt(i);
                }
                else
                {
                    ++i;
                }
            }
        }

        public void HandleFileFinished(FDInfo info)
        {
            if (!string.IsNullOrEmpty(info.www.error))
            {
                UnityEngine.Debug.LogError("File download failed - " + info.name + "\n" + info.www.error);
                infos.Remove(info);
            }
            else
            {
                string saveTo = null;
                string localSave = null;
                switch (info.saveLocation)
                {
                    case DownloadLocation.AssetFolder:
                        localSave = "/" + info.name;
                        saveTo = Application.dataPath + localSave;
                        break;
                    case DownloadLocation.ResourceFolder:
                        localSave = "/Resources/" + info.name;
                        saveTo = Application.dataPath + localSave;
                        break;
                    case DownloadLocation.TempFolder:
                        saveTo = Application.temporaryCachePath + "/" + info.name;
                        break;
                    case DownloadLocation.PersistFolder:
                        saveTo = Application.persistentDataPath + "/" + info.name;
                        break;
                }
                UnityEngine.Debug.Log("File: " + info.name + "downloaded!");
                UTIL.WriteAllText(saveTo, info.www.text);

                if (info.saveLocation == DownloadLocation.AssetFolder || info.saveLocation == DownloadLocation.ResourceFolder)
                {
                    AssetDatabase.ImportAsset("Assets/" + localSave, ImportAssetOptions.ForceUpdate);
                }
            }
        }
    }
}