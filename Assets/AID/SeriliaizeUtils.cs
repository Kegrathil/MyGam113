using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using System.Linq;

namespace AID
{
    /*
	 * Helper for simple serialisation of a data bundle to xml and back again
	 * 
	 * To use create an instance of this class with the desired type in the <>
	 * To serialise to xml, then set the obj inside the instance you just made
	 * to the instance of the data bundle you want to serialise. Call 
	 * SerializeObject. Now you can take the .str attribute from the instance
	 * of this class.
	 * 
	 * Deserialise is the reverse, set the str to the xml (probably from a file).
	 * Call DeserializeObject. Take the .obj from this class instance and copy it
	 * over to wherever you need
	 */
    public struct SerialiseXMLToString<T>
    {
        // mutilated Generic version of http://www.unifycommunity.com/wiki/index.php?title=Save_and_Load_from_XML

        public T obj;
        public string str;
        public System.Type[] subtypes;

        //this is expensive, if you are going to do a lot of it, keep the SerialiseXMLToString around so it is cached
        //http://stackoverflow.com/questions/857705/get-all-derived-types-of-a-type
        public System.Type[] FindSubClasses()
        {
            if (subtypes == null)
            {
                subtypes = (from lAssembly in AppDomain.CurrentDomain.GetAssemblies()
                            from lType in lAssembly.GetTypes()
                            where typeof(T).IsAssignableFrom(lType)
                            select lType).ToArray();
            }
            return subtypes;
        }

        public void SerializeObject(bool findSubClasses)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            if (findSubClasses)
            {
                xs = new XmlSerializer(typeof(T), FindSubClasses());
            }

            StringWriter sw = new StringWriter();
            xs.Serialize(sw, obj);
            str = sw.ToString();
        }

        // Here we deserialize it back into its original form
        public void DeserializeObject(bool findSubClasses)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            if (findSubClasses)
            {
                xs = new XmlSerializer(typeof(T), FindSubClasses());
            }

            StringReader sr = new StringReader(str);
            try
            {
                obj = (T)xs.Deserialize(sr);
            }
            catch (SystemException e)
            {
                Debug.Log(e.Message);
            }
        }
    };
    
    static public partial class UTIL
    {
        

        /*
	     * Turn a simple class instance into a xml string
	     */
        static public string ToXML<T>(T obj)
        {
            SerialiseXMLToString<T> serialiser = new SerialiseXMLToString<T>();
            serialiser.obj = obj;
            serialiser.SerializeObject(false);
            return serialiser.str;
        }

        /*
	     * Turn a simple class instance into a xml string
	     */
        static public string ToXML<T>(T obj, bool findSubClasses)
        {
            SerialiseXMLToString<T> serialiser = new SerialiseXMLToString<T>();
            serialiser.obj = obj;
            serialiser.SerializeObject(findSubClasses);
            return serialiser.str;
        }


        /*
	     * Turn an string back into a class instance
	     */
        static public T FromXML<T>(string xmlstr)
        {
            SerialiseXMLToString<T> serialiser = new SerialiseXMLToString<T>();
            serialiser.str = xmlstr;
            try
            {
                serialiser.DeserializeObject(false);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error DeserializeObject: " + e.Message);
            }
            return serialiser.obj;
        }


        /*
		* Turn an string back into a class instance
		*/
        static public T FromXML<T>(string xmlstr, bool findSubClasses)
        {
            SerialiseXMLToString<T> serialiser = new SerialiseXMLToString<T>();
            serialiser.str = xmlstr;
            try
            {
                serialiser.DeserializeObject(findSubClasses);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error DeserializeObject: " + e.Message);
            }
            return serialiser.obj;
        }





    }
}
