using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AID
{
    /// <summary>
    /// A collection of SO Data objects that are driven by an external csv file.
    /// 
    /// The csv needs the following columns;
    /// - 'name' this is the name of the SO Data
    /// - 'type' the internal type of SO Data that should be used to house it
    /// - 'value' stringified format of the current value of the variable desired
    /// Any other columns will be ignored but they are not disallowed. 
    ///
    /// Example csv:
    /// name,type,value
    /// myFloat,float,0
    /// myOtherFloat,float,2
    /// PI,float,3.14159265358979
    /// 
    /// Currently supports;
    /// - 'int' backed by an IntSO
    /// - 'float' backed by a FloatSO
    /// - 'string' backed by StringSO
    /// To add more types see the static List<TypeSOHandler> creators
    /// 
    /// TODO: 
    ///     more external file types, no reason not to allow json or xml.
    ///     write data back out to a file
    ///     
    /// </summary>
    [CreateAssetMenu()]
    public class ExternalVarsSO : BaseSO
    {
        //stringified rep of 1 external var, also one row from our csv
        public class VarSpec
        {
            public string name;
            public string type;
            public string value;
        }

        public TextAsset localCSV;
        //public string removeCSVURI;

        [SerializeField] protected List<DataTypeSOBase> vars = new List<DataTypeSOBase>();

        public DataTypeSOBase Find(string name)
        {
            return vars.Find(x => x.name == name);
        }

        public T Find<T>(string name) where T : DataTypeSOBase
        {
            return Find(name) as T;
        }


        #region Support Types
        //supproted types
        public class TypeSOHandler
        {
            public string typename;
            public Type SOType;
            public delegate void UpdateValueDel(DataTypeSOBase SO, string sVal);
            public UpdateValueDel updateFunc;

            public TypeSOHandler(string typeName, Type SOType, UpdateValueDel updater)
            {
                this.typename = typeName;
                this.SOType = SOType;
                this.updateFunc = updater;
            }
        }

        //collection of all supported types
        public static readonly List<TypeSOHandler> handlers = new List<TypeSOHandler>()
        {
            new TypeSOHandler("float", typeof(FloatSO), (DataTypeSOBase SO, string sVal) =>
            {
                (SO as FloatSO).Value = float.Parse(sVal);
            }),
            new TypeSOHandler("int", typeof(IntSO), (DataTypeSOBase SO, string sVal) =>
            {
                (SO as IntSO).Value = int.Parse(sVal);
            }),
            new TypeSOHandler("string", typeof(StringSO), (DataTypeSOBase SO, string sVal) =>
            {
                (SO as StringSO).Value = sVal;
            }),

        };

        private Type GetSOType(string typename)
        {
            var res = handlers.Find(x => x.typename == typename);
            if (res != null)
                return res.SOType;

            Debug.LogError("ExternalVarsSO does not have a registered hanlder for typename: " + typename);
            return null;
        }

        private void UpdateValue(DataTypeSOBase SO, string sVal)
        {
            var res = handlers.Find(x => x.SOType == SO.GetType());
            if (res != null)
            {
                res.updateFunc(SO, sVal);
            }
            else
            {
                Debug.LogError("ExternalVarsSO could not update value of " + SO.name + ". No matching updater found for type " + SO.GetType().Name);
            }
        }
        #endregion Support Types

        public void RemoveVars()
        {
#if UNITY_EDITOR
            //remove and clean up all the SO assets
            foreach (var item in vars)
            {
                if (item != null)
                {
                    Undo.RecordObject(item, "item destroyed");
                    Undo.DestroyObjectImmediate(item);
                }
            }
            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
            EditorUtility.SetDirty(this);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
            //runtime we just need to ditch our references to them
            vars = new List<DataTypeSOBase>();
        }

        public void WriteVarsToLocal()
        {
            var rows = GenerateVarSpecs();
            var csvFile = DeadSimpleCSV.CreateFromList(rows).GetAsCSVString(true);
#if UNITY_EDITOR
            System.IO.File.WriteAllText(AssetDatabase.GetAssetPath(localCSV), csvFile);
            AssetDatabase.Refresh();
#endif
        }

        public void GenerateVarsLocalCSV()
        {
            var items = DeadSimpleCSV.ConvertStringCSVToObjects<VarSpec>(localCSV.text, true);
            GenerateVars(items);
        }

        public void GenerateVars(List<VarSpec> items)
        {
            //make new ones
            foreach (var item in items)
            {
                if (Find(item.name) == null)
                {
                    var t = GetSOType(item.type);
                    if (t != null)
                    {
                        var newSO = CreateInstance(t) as DataTypeSOBase;
                        newSO.name = item.name;
                        vars.Add(newSO);
#if UNITY_EDITOR
                        //AssetDatabase.CreateFolder("Assets", "Data");
                        //AssetDatabase.CreateAsset(newSO, "Assets/Data/" + this.name + "_" + item.name + ".asset");
                        var ourLoc = AssetDatabase.GetAssetPath(this);
                        var subLoc = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(ourLoc), this.name) + System.IO.Path.DirectorySeparatorChar;

                        if (!System.IO.Directory.Exists(subLoc))
                            System.IO.Directory.CreateDirectory(subLoc);

                        var fileName = subLoc + newSO.name + ".asset";
                        AssetDatabase.CreateAsset(newSO, fileName);
                        EditorUtility.SetDirty(newSO);
#endif 
                    }
                }
            }

            //delete non-existing ones
            for (int i = vars.Count; i <= 0; i--)
            {
                if (items.Find(x => x.name == vars[i].name) == null)
                {
                    var item = vars[i];
                    vars.RemoveAt(i);
                    DestroyImmediate(item);
                    Debug.Log("External Var item " + item.name + " no longer exists and is being removed.");
                }
            }


#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif 
            //internal update values
            UpdateVars(items);
        }

        public void UpdateVars(List<VarSpec> items)
        {
            //foreach name in csv
            //find the var
            //cast to type
            //if fail error
            //assign value

            foreach (var item in items)
            {
                var v = Find(item.name);
                UpdateValue(v, item.value);
#if UNITY_EDITOR
                EditorUtility.SetDirty(v);
#endif
            }


#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        public List<VarSpec> GenerateVarSpecs()
        {
            List<VarSpec> retval = new List<VarSpec>();

            foreach (var v in vars)
            {
                var handler = handlers.Find(x => x.SOType == v.GetType());

                retval.Add(new VarSpec() { name = v.name, type = handler.typename, value = v.ToString() });
            }

            return retval;
        }

#if UNITY_EDITOR
        public override void Awake()
        {
            base.Awake();

            ExternalVarsSOManager.Instance().NotifyExists(this);
        }

        public override void OnValidate()
        {
            base.OnValidate();
            vars = vars.Where(x => x != null).ToList();
        }
#endif
    }
}