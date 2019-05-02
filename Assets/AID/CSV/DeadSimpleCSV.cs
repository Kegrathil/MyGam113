using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Text;
using System.Reflection;


namespace AID
{
    [System.Serializable]
    public class DeadSimpleCSV
    {
        
        public string[] headers = new string[0];
        
        private Dictionary<string, int> colNames = new Dictionary<string, int>();
        public Dictionary<string, int> ColumnNames
        {
            get
            {
                return colNames;
            }
        }
        
        private List<string[]> rows = new List<string[]>();
        public List<string[]> Rows
        {
            get
            {
                return rows;
            }
        }

        public DeadSimpleCSV() { }

        public DeadSimpleCSV(string csvSource, bool genColLookup)
        {
            #region old csv extraction
            //		StringReader sr = new StringReader(csvSource);
            //		rows = new List<string[]>();
            //
            //		//read first line
            //		string line = sr.ReadLine();
            //
            //		//given an empty or invalid string it seems
            //		if(string.IsNullOrEmpty(line))
            //			return;
            //
            //		// split
            //		headers = line.Split(',');
            //		headers = headers.Select(el => el.Trim()).ToArray();
            //
            //		colNames = new Dictionary<string, int>();
            //
            //		for(int i = 0; i < headers.Length; ++i)
            //		{
            //			colNames.Add(headers[i], i);
            //		}
            //
            //		//read remaining lines
            //		while((line = sr.ReadLine()) != null)
            //		{
            //			rows.Add(line.Split(',').Select(el => el.Trim()).ToArray());
            //		}
            #endregion

            DeadSimpleCSVParser parser = new DeadSimpleCSVParser();
            parser.Parse(csvSource);
            headers = parser.data[0];

            if(genColLookup)
                GenerateColumnLookup();

            rows = colNames.Count > 0 ? (parser.data.GetRange(1, parser.data.Count - 1)) : (parser.data.GetRange(0, parser.data.Count));
        }

        public bool HasColumn(string v)
        {
            return headers.Contains(v);
        }

        //create a copy and remove a bunch of cols
        public DeadSimpleCSV SkipCols(int skipOverTheFirstXCols)
        {
            var retval = this.Copy();

            for (int i = 0; i < retval.Rows.Count; i++)
            {
                retval.rows[i] = retval.rows[i].Skip(skipOverTheFirstXCols).ToArray();
            }

            return retval;
        }

        //create a copy and remove a bunch of cols
        public DeadSimpleCSV TakeCols(int takeFirstXCols)
        {
            var retval = this.Copy();

            for (int i = 0; i < retval.Rows.Count; i++)
            {
                retval.rows[i] = retval.rows[i].Take(takeFirstXCols).ToArray();
            }

            return retval;
        }

        internal void AppendRows(int skipRows, DeadSimpleCSV t)
        {
            rows.AddRange(t.rows.GetRange(skipRows, t.rows.Count- skipRows));
        }

        public void AppendCols(int skipCols, DeadSimpleCSV appendFrom)
        {
            var appendThis = appendFrom.SkipCols(skipCols);

            for (int i = 0; i < rows.Count && i < appendThis.rows.Count; i++)
            {
                List<string> tmp = new List<string>();
                tmp.AddRange(rows[i]);
                tmp.AddRange(appendThis.rows[i]);

                rows[i] = tmp.ToArray();
            }
        }

        public void GenerateColumnLookup()
        {
            colNames = new Dictionary<string, int>();

            for (int i = 0; i < headers.Length; ++i)
            {
                if(headers[i].Length == 0)
                {
                    //abort, not named entirely
                    colNames = new Dictionary<string, int>();
                    return;
                }
                colNames.Add(headers[i], i);
            }
        }

        public string GetAsCSVString(bool includeHeaders)
        {
            StringBuilder sb = new StringBuilder();

            if(includeHeaders)
                AppendCSVRow(headers, sb);

            for (int i = 0; i < rows.Count; ++i)
                AppendCSVRow(rows[i], sb);

            return sb.ToString();
        }

        //zero indexed string row from the csv
        public string[] GetRow(int rowIndex)
        {
            return rows[rowIndex];
        }

        public string[] GetRowWithData(int columnToCheck, string targetData)
        {
            string[] col = GetColumn(columnToCheck);

            int loc = Array.IndexOf(col, targetData);

            if (loc != -1)
            {
                return GetRow(loc);
            }

            return null;
        }

        public string GetCellWithData(int columnToCheck, string targetData, int columnToReturn)
        {
            string[] row = GetRowWithData(columnToCheck, targetData);

            if (row != null)
            {
                return row[columnToReturn];
            }

            return null;
        }

        public string GetCellWithData(string columnToCheck, string targetData, int columnToReturn)
        {
            int colindex = GetIndexByName(columnToCheck);
            if (colindex != -1)
                return GetCellWithData(colindex, targetData, columnToReturn);

            return null;
        }

        public string GetCellWithData(string columnToCheck, string targetData, string columnToReturn)
        {
            int colindex = GetIndexByName(columnToCheck);
            int colretindex = GetIndexByName(columnToReturn);
            if (colindex != -1 && colretindex != -1)
                return GetCellWithData(colindex, targetData, colretindex);

            return null;
        }

        public string GetCellWithData(int columnToCheck, string targetData, string columnToReturn)
        {
            int colretindex = GetIndexByName(columnToReturn);
            if (colretindex != -1)
                return GetCellWithData(columnToCheck, targetData, colretindex);

            return null;
        }

        public string[] GetRowWithData(string columnToCheckName, string targetData)
        {
            return GetRowWithData(GetIndexByName(columnToCheckName), targetData);
        }

        public string[] GetColumn(int col)
        {
            if (col >= headers.Length)
                return new string[0];

            string[] ret = new string[rows.Count];

            for (int i = 0; i < rows.Count; ++i)
            {
                ret[i] = rows.ElementAt(i)[col];
            }

            return ret;
        }

        public int GetIndexByName(string colName)
        {
            int retval = -1;
            colNames.TryGetValue(colName, out retval);
            return retval;
        }

        public string[] GetColumn(string colName)
        {
            return GetColumn(GetIndexByName(colName));
        }

        public string GetCell(int col, int row)
        {
            if (col < headers.Length && row < rows.Count)
                return rows.ElementAt(row)[col];
            else
                return null;
        }

        public string GetCell(string colName, int row)
        {
            return GetCell(GetIndexByName(colName), row);
        }

        public string GetCell(string colName, string[] actualRow)
        {
            int colIndex = GetIndexByName(colName);
            return colIndex == -1 ? null : actualRow[colIndex];
        }

        //this is only here for the sake of completeness
        public string GetCell(int col, string[] actualRow)
        {
            return actualRow[col];
        }


        public void RemoveRow(string[] row)
        {
            rows.Remove(row);
        }

        public void AddRow(string[] row)
        {
            rows.Add(row);
        }

        public bool IsValidRow(string[] row)
        {
            return row.Length == headers.Length;
        }

        //uses row headers as field and property names, this is entirely untested on anything other than classes 
        // and structs of basic types, ints, floats, strings. Enums will probably work, classes within classes may not
        public List<T> ConvertRowsToObjects<T>() where T : class, new()
        {
            List<T> objs = new List<T>();

            for (int i = 0; i < Rows.Count; i++)
            {
                T obj = new T();
                Type type = obj.GetType();

                for (int j = 0; j < headers.Length; j++)
                {
                    FieldInfo f = type.GetField(headers[j]);

                    if (f != null)
                    {
                        //enums are special apparently
                        if (f.FieldType.IsEnum)
                        {
                            object enumFromObj = Enum.Parse(f.FieldType, Rows[i][j]);
                            f.SetValue(obj, enumFromObj);
                        }
                        else
                        {
                            f.SetValue(obj, Convert.ChangeType(Rows[i][j], f.FieldType));
                        }

                        continue;
                    }

                    PropertyInfo p = obj.GetType().GetProperty(headers[j]);
                    if (p != null)
                    {
                        p.SetValue(obj, Convert.ChangeType(Rows[i][j], p.PropertyType), null);
                        continue;
                    }


                }

                objs.Add(obj);
            }

            return objs;
        }

        //create an instance from a list of objects
        static public DeadSimpleCSV CreateFromList<T>(List<T> l) where T : class, new()
        {
            DeadSimpleCSV retval = new DeadSimpleCSV();
            Type t = typeof(T);
            //create header
            FieldInfo[] fields = t.GetFields();
            PropertyInfo[] props = t.GetProperties();


            retval.headers = new string[fields.Length + props.Length];

            int i = 0;

            for (; i < l.Count; i++)
            {
                retval.rows.Add(new string[fields.Length + props.Length]);
            }


            i = 0;

            foreach (FieldInfo f in fields)
            {
                //filling in headers
                retval.headers[i] = f.Name;

                for (int j = 0; j < l.Count; j++)
                {
                    retval.rows[j][i] = (string)Convert.ChangeType(f.GetValue(l[j]), typeof(string));
                }

                i++;
            }

            foreach (PropertyInfo p in props)
            {
                //filling in headers
                retval.headers[i] = p.Name;

                for (int j = 0; j < l.Count; j++)
                {
                    retval.rows[j][i] = (string)Convert.ChangeType(p.GetValue(l[j], null), typeof(string));
                }

                i++;
            }


            retval.GenerateColumnLookup();

            return retval;
        }

        public static List<T> ConvertStringCSVToObjects<T>(string csvStr, bool generateColumnLookup)  where T : class, new()
        {
            var csv = new DeadSimpleCSV(csvStr, generateColumnLookup);
            return csv.ConvertRowsToObjects<T>();
        }

        //convert a string[] to a csv row
        static public void AppendCSVRow(string[] row, StringBuilder sb)
        {
            for (int i = 0; i < row.Length; i++)
            {
                sb.Append(EncodeForCSV(row[i]));
                if (i < row.Length - 1) sb.Append(',');
            }
            sb.Append('\n');
        }

        static public string ToCSVRow(string[] row)
        {
            StringBuilder sb = new StringBuilder();
            AppendCSVRow(row, sb);
            return sb.ToString();
        }

        //all " become "" and entire string is surrounded with " "
        static public string EncodeForCSV(string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            if (s.IndexOfAny("\",\n".ToCharArray()) != -1)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append('\"');
                sb.Append(s.Replace("\"", "\"\""));
                sb.Append('\"');
                return sb.ToString();
            }

            return s;
        }

        //convert all "" back to "
        static public string DecodeFromCSV(string s)
        {
            if (s.Length == 0)
                return s;

            int start = s[0] == '\"' ? 1 : 0;
            int end = s[s.Length - 1] == '\"' ? s.Length : s.Length - 1;
            return s.Substring(start, end - start + 1).Replace("\"\"", "\"");

        }

        public override bool Equals(object obj)
        {
            var lhs = this;
            var rhs = (DeadSimpleCSV)obj;

            if (lhs.headers.Length != rhs.headers.Length)
                return false;

            for (int i = 0; i < lhs.headers.Length; i++)
            {
                if (!lhs.headers[i].Equals(rhs.headers[i]))
                    return false;
            }

            if (lhs.rows.Count != rhs.rows.Count)
                return false;

            for (int i = 0; i < lhs.rows.Count; i++)
            {
                var lhsRow = lhs.rows[i];
                var rhsRow = rhs.rows[i];

                if (lhsRow.Length != rhsRow.Length)
                    return false;

                for (int j = 0; j < lhsRow.Length; j++)
                {
                    if (!lhsRow[j].Equals(rhsRow[j]))
                        return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}