using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public TMP_Text countText;
    public float updateInterval = 0.5f; //How often should the number update

    float accum = 0.0f;
    int frames = 0;
    float timeleft;
    float fps;

    GUIStyle textStyle = new GUIStyle();

    // Use this for initialization
    void Start()
    {
        timeleft = updateInterval;

        textStyle.fontStyle = FontStyle.Bold;
        textStyle.normal.textColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            countText.SetText(fps.ToString("F2") + " FPS");
            fps = (accum / frames);
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }
}
