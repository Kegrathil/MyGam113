using UnityEngine;
using System.Collections.Generic;

public class BasicPerf : MonoBehaviour {

    public Color color = Color.white;
    public int fontSize = 24;

    protected long seconds = 0;
    protected System.DateTime startOfFrame, startOfRender;
    protected List<int> updateToUpdateMS = new List<int>(), cpuPerFrame = new List<int>(), gpuPerFrame = new List<int>();

    protected string toDisplay = "test";
    protected GUIStyle gs = new GUIStyle();

	void OnEnable ()
    {
        startOfFrame = System.DateTime.Now;
    }
	
	// Update is called once per frame
	void Update ()
    {
        var curStartOfFrame = System.DateTime.Now;

        updateToUpdateMS.Add( (int) ((curStartOfFrame - startOfFrame).Ticks) );

        startOfFrame = curStartOfFrame;
        var curSeconds = (long)AID.UTIL.SinceEpoch().TotalSeconds;

        if (seconds != curSeconds)
        {
            var avFrameTime = 0;
            for (int i = 0; i < updateToUpdateMS.Count; i++)
            {
                avFrameTime += updateToUpdateMS[i];
            }
            avFrameTime /= updateToUpdateMS.Count;


            var avCPUTime = 0;
            for (int i = 0; i < cpuPerFrame.Count; i++)
            {
                avCPUTime += cpuPerFrame[i];
            }
            avCPUTime /= cpuPerFrame.Count;


            var avGPUTime = 0;
            for (int i = 0; i < gpuPerFrame.Count; i++)
            {
                avGPUTime += gpuPerFrame[i];
            }
            avGPUTime /= gpuPerFrame.Count;

            updateToUpdateMS.Clear();
            cpuPerFrame.Clear();
            gpuPerFrame.Clear();

            //udpate display
            toDisplay = "AV. Upd: " + avFrameTime.ToString() +
                        "\nAv. CPU: " + avCPUTime.ToString() +
                        "\nAv. GPU: " + avGPUTime.ToString();
        }

        seconds = curSeconds;
    }

    void LateUpdate()
    {
        cpuPerFrame.Add((int)((System.DateTime.Now - startOfFrame).Ticks));
    }

    void OnPreRender()
    {
        startOfRender = System.DateTime.Now;
    }

    void OnPostRender()
    {
        gpuPerFrame.Add((int)((System.DateTime.Now - startOfRender).Ticks));
    }

    void OnGUI()
    {
        gs.fontSize = fontSize;
        gs.normal.textColor = color;
        GUI.Label(new Rect(Screen.width / 2, 0, Screen.width / 2, Screen.height), toDisplay, gs);
    }
}
