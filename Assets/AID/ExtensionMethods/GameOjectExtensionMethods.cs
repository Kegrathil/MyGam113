using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static partial class GameOjectExtensionMethods
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        T retval = go.GetComponent<T>();

        return retval != null ? retval : go.AddComponent<T>();
    }

    static public void FlipActivation<T>(this GameObject g, T goa) where T : IEnumerable
    {
        foreach (GameObject go in goa)
            g.FlipActivation(go);
    }

    static public void ToggleActive<T>(this GameObject g, T goa) where T : IEnumerable
    {
        g.FlipActivation(goa);
    }

    static public void SetActiveAll<T>(this GameObject g, T goa, bool state) where T : IEnumerable
    {
        foreach (GameObject go in goa)
            if (go != null) go.SetActive(state);
    }

    static public void ActivateAll<T>(this GameObject g, T goa) where T : IEnumerable
    {
        g.SetActiveAll(goa, true);
    }

    static public void DeactivateAll<T>(this GameObject g, T goa) where T : IEnumerable
    {
        g.SetActiveAll(goa, false);
    }

    static public void FlipActivation(this GameObject g, GameObject go)
    {
        if (go != null) go.SetActive(!go.activeInHierarchy);
    }


    static public void SendMessageToAll(this GameObject g, GameObject[] gos, string msgName, SendMessageOptions opts)
    {
        //foreach(GameObject go in gos)
        for (int i = 0; i < gos.Length; i++)
            if (gos[i] != null) gos[i].SendMessage(msgName, opts);
    }

    static public void SendMessageToAll(this GameObject g, List<GameObject> gos, string msgName, SendMessageOptions opts)
    {
        //foreach(GameObject go in gos)
        for (int i = 0; i < gos.Count; i++)
            if (gos[i] != null) gos[i].SendMessage(msgName, opts);
    }

    static public void SendMessageToAll(this GameObject g, GameObject[] gos, string msgName, object param, SendMessageOptions opts)
    {
        for (int i = 0; i < gos.Length; i++)
            if (gos[i] != null) gos[i].SendMessage(msgName, param, opts);
    }

    static public void SendMessageToAll(this GameObject g, List<GameObject> gos, string msgName, object param, SendMessageOptions opts)
    {
        for (int i = 0; i < gos.Count; i++)
            if (gos[i] != null) gos[i].SendMessage(msgName, param, opts);
    }

    static public bool CompareTagCollection(this GameObject g, GameObject go, List<string> tags)
    {
        //foreach(string s in tags)
        for (int i = 0; i < tags.Count; i++)
        {
            if (go.CompareTag(tags[i]))
                return true;
        }
        return false;
    }
}
