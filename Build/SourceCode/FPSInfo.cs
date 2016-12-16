using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FPSInfo : MonoBehaviour {

    // Attach this to a GUIText to make a frames/second indicator.
    //
    // It calculates frames/second over each updateInterval,
    // so the display does not keep changing wildly.
    //
    // It is also fairly accurate at very low FPS counts (<10).
    // We do this not by simply counting frames per interval, but
    // by accumulating FPS for each frame. This way we end up with
    // correct overall FPS even if the interval renders something like
    // 5.5 frames.

    public float updateInterval = 0.5F;

    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval

    string fpsString;
    float halfwidth;
    float halfheight;
    void Start()
    {
        halfwidth = Screen.width/2;
        halfheight = Screen.height/2;

        timeleft = updateInterval;
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            float fps = accum / frames; //this will get the average FPS.
            string format = System.String.Format("{0:F2} FPS", fps);
            fpsString = format;

            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
    }

    void OnGUI() {
        GUI.Label(new Rect(halfwidth, halfheight, 200, 50), "Hold on F To Fire!");
        GUI.Label(new Rect(500, 10, 100, 20), fpsString);
    }
}
