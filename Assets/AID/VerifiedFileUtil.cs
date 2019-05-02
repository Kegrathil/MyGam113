using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace AID
{
    /*
        Read and write a file along with a hash or signature file. This makes it harder for end user to trivially edit saved data.

        Make sure the salt is the same for read and write, also if you do not wish for users to share their files with 
            each other, put in some per user or per hardware id in the salt.

        This is not fool proof that simply means if you read in player coins or from a file if the user changes that value
            in your file and doesn't also figure out how to generate a valid signature file you know that they've changed
            the file and can take action.

    */
    public static class VerifiedFileUtil
    {
        public static readonly string SignatureExtension = ".signature";

        public static void SaveFileWithSignature(string fileContents, string saveTo, string salt)
        {
            //find the hash of it plus the salt
            SHA256Managed hasher = new SHA256Managed();

            var hashRes = hasher.ComputeHash(Encoding.ASCII.GetBytes((fileContents + salt).ToCharArray()));

            //save the file
            File.WriteAllText(saveTo, fileContents);
            //save the hash in a signature file
            File.WriteAllText(saveTo + SignatureExtension, Encoding.ASCII.GetString(hashRes));
        }


        public static string ReadFileCheckSignature(string loadFrom, string salt)
        {
            string fileContents = string.Empty;
            try
            {
                fileContents = File.ReadAllText(loadFrom);
            }
            catch (System.Exception)
            {

            }
            //find the hash of it plus the salt
            SHA256Managed hasher = new SHA256Managed();

            var hashRes = hasher.ComputeHash(Encoding.ASCII.GetBytes((fileContents + salt).ToCharArray()));

            //load sig
            var loadedSignature = string.Empty;
            try
            {
                loadedSignature = File.ReadAllText(loadFrom + SignatureExtension);
            }
            catch (System.Exception)
            {
            }

            if (Encoding.ASCII.GetString(hashRes) != loadedSignature)
            {
                return string.Empty;
            }

            return fileContents;
        }
    }

}