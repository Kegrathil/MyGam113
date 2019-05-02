using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AID
{
    /// <summary>
    /// One of these per project, contains all ExternalVarsSO in the project.
    /// 
    /// Is auto-generated and updated when ExternalVarsSOs are created and destroyed.
    /// </summary>
    public class ExternalVarsSOManager : BaseSO
    {
        [SerializeField] protected List<ExternalVarsSO> allExternalVars = new List<ExternalVarsSO>();

        public void Clear()
        {
            allExternalVars = new List<ExternalVarsSO>();
        }

        [ContextMenu("FindAll")]
        public void FindAll()
        {
            var res = AssetDatabase.FindAssets("t:ExternalVarsSO");
            foreach (var item in res)
            {
                var extVarLoc = AssetDatabase.GUIDToAssetPath(item);
                var extVar = AssetDatabase.LoadAssetAtPath<ExternalVarsSO>(extVarLoc);
                if(extVar != null)
                {
                    allExternalVars.Add(extVar);
                }
            }

            allExternalVars = allExternalVars.Distinct().ToList();
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

        public ExternalVarsSO FindByExternalFile(string fileAndPath)
        {
            foreach (var item in allExternalVars)
            {
                if (AssetDatabase.GetAssetPath(item).Contains(fileAndPath))
                    return item;
            }

            return null;
        }

        public ExternalVarsSO FindByExternalFile(TextAsset txtasset)
        {
            foreach (var item in allExternalVars)
            {
                if (item.localCSV == txtasset)
                    return item;
            }

            return null;
        }

        public void NotifyExists(ExternalVarsSO extVar)
        {
            if (!allExternalVars.Contains(extVar))
            {
                allExternalVars.Add(extVar);
            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

        public void NotifyNoLongerExists(ExternalVarsSO extVar)
        {
            allExternalVars.Remove(extVar);
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

#if UNITY_EDITOR
        /// <summary>
        /// This is not intended to be used at runtime. Runtime objects should have a direct reference.
        /// </summary>
        /// <returns></returns>
        public static ExternalVarsSOManager Instance()
        {
            var assetRes = AssetDatabase.FindAssets("t:ExternalVarsSOManager");

            if (assetRes == null || assetRes.Length == 0)
            {
                //none create and return
                var newMan = CreateInstance<ExternalVarsSOManager>();
                newMan.FindAll();
                AssetDatabase.CreateAsset(newMan, "Assets/ExternalVarsSOManager.asset");
                EditorUtility.SetDirty(newMan);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                return newMan;
            }
            else if (assetRes.Length > 1)
            {
                //too many
                for (int i = 1; i < assetRes.Length; i++)
                {
                    AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(assetRes[i]));
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            var loc = AssetDatabase.GUIDToAssetPath(assetRes[0]);
            return AssetDatabase.LoadAssetAtPath<ExternalVarsSOManager>(loc);
        }
#endif
        public override void OnValidate()
        {
            base.OnValidate();
            allExternalVars = allExternalVars.Where(x => x != null).ToList();
        }
    }
}