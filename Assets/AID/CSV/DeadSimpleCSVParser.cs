using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Text;

namespace AID
{
    /*
     * Reads a comma seperated csv, empty cells with a newline indicate the end of a row.
     */
    public class DeadSimpleCSVParser
    {

        public List<string[]> data = new List<string[]>();
        //public int longestRow = 0;
        public bool trimWhiteSpace = true;

        private int curIndex = 0;
        private string sourceString;


        public void Parse(string csv)
        {
            sourceString = csv;
            List<string> row = null;

            while ((row = _parseCurRow()) != null)
            {
                data.Add(row.ToArray());
            }

        }

        private List<string> _parseCurRow()
        {
            if (curIndex >= sourceString.Length)
                return null;

            List<string> retval = new List<string>();
            char curChar = '\0';//sourceString[curIndex];
            int loc = 0;

            char[] newlineOrComma = "\n,".ToArray();
            char prevChar = '\0';



            while (curIndex < sourceString.Length)
            {
                curChar = sourceString[curIndex];

                switch (curChar)
                {
                    case '\n':
                        curIndex++;
                        return retval;
                    //	break;
                    case '\"':
                        //read till close of "
                        curIndex++;
                        int curSearchForClose = curIndex;

                        //handle " in the middle of a string
                        while (true)
                        {
                            loc = sourceString.IndexOf('\"', curSearchForClose);

                            if (sourceString[loc + 1] != '\"')
                            {
                                break;
                            }
                            else
                            {
                                //not the true end keep looking
                                curSearchForClose = loc + 2;
                            }
                        }

                        prevChar = '\"';
                        _addStringMoveForward(retval, sourceString.Substring(curIndex, loc - curIndex));
                        //and over the "
                        curIndex++;
                        break;
                    case ',':
                        if(prevChar == ',' || prevChar == '\0')
                        {
                            //we found an empty cell
                            _addStringMoveForward(retval, "");
                        }
                        curIndex++;
                        break;
                    default:

                        if (prevChar == '\"' && curChar == '\r')
                        {
                            curIndex++;
                            break;
                        }
                        //				if(curChar == ',')
                        //					curIndex++;

                        //				if(sourceString[curIndex] == '\"')
                        //					continue;	//handled elsewhere

                        //read till next ,
                        loc = sourceString.IndexOfAny(newlineOrComma, curIndex);

                        //found something
                        if (loc != -1)
                        {
                            _addStringMoveForward(retval, sourceString.Substring(curIndex, loc - curIndex));
                        }
                        else
                        {
                            //handle end of file
                            _addStringMoveForward(retval, sourceString.Substring(curIndex));
                            curIndex = sourceString.Length;
                            return retval;
                        }
                        break;
                }

                prevChar = curChar;
            }

            //handle trailing empty cell without a newline
            if (prevChar == ',')
            {
                _addStringMoveForward(retval, "");
            }

            return retval;
        }

        private void _addStringMoveForward(List<string> addto, string s)
        {
            int len = s.Length;

            var r = DeadSimpleCSV.DecodeFromCSV(s);
            
            addto.Add(trimWhiteSpace ? r.Trim() : r);

            curIndex += len;
        }
    }
}