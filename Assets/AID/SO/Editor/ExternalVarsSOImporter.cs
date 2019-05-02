using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace AID
{
    /// <summary>
    /// When a ExtVar compatible text file is found, causes a matching ExternalVarsSO be
    /// either created when one does not exist or updated in the case that one does
    /// </summary>
    public class ExternalVarsSOImporter : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var item in importedAssets)
            {
                ProcessIfValidExtVarAsset(item);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        static bool ProcessIfValidExtVarAsset(string item)
        {
            //if it is a csv and it loads and it has the required cols
            if(item.EndsWith(".csv"))
            {
                var txt = AssetDatabase.LoadAssetAtPath<TextAsset>(item);
                if(txt == null)
                    return false;

                DeadSimpleCSV csv = null;
                try
                {
                    csv = new DeadSimpleCSV(txt.text, true);
                }
                catch (Exception)
                {
                }
                ProcessAsset(item, txt, csv);
            }
            return false;
        }

        //needs to assign the text asset as part of its creation
        static void ProcessAsset(string origLocation, TextAsset txt, DeadSimpleCSV csv)
        {
            //if matching file exists, then get it
            ExternalVarsSO extvar = ExternalVarsSOManager.Instance().FindByExternalFile(txt);

            var fileName = origLocation.Substring(0, origLocation.Length - (".csv".Length)) + ".asset";
            bool createdAsset = false;
            //if not create it an
            if (extvar == null)
            {
                extvar = ExternalVarsSO.CreateInstance<ExternalVarsSO>();
                extvar.localCSV = txt;
                AssetDatabase.CreateAsset(extvar, fileName);
                createdAsset = true;
            }

            try
            {
                extvar.GenerateVars(csv.ConvertRowsToObjects<ExternalVarsSO.VarSpec>());
            }
            catch (Exception)
            {
                if(createdAsset)
                {
                    AssetDatabase.DeleteAsset(fileName);
                    UnityEngine.Object.DestroyImmediate(extvar);
                }
            }
        }
    }
}