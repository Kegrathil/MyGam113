using UnityEngine;

public static partial class StringExtensionMethods
{
    public static char Back(this string s)
    {
        return s.Length > 0 ? s[s.Length - 1] : '\0';
    }

    public static string RemoveBack(this string s, int numToRemoveFromBack = 1)
    {
        if (s.Length > numToRemoveFromBack)
            return s.Substring(0,s.Length - numToRemoveFromBack);
        else
            return string.Empty;
    }
}
