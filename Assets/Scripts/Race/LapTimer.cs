using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LapTimer : MonoBehaviour
{
    public TMP_Text lapTimeText;

    private float lapStartTime;
    private float totalTime;
    private int lapCount = 0;
    private List<float> lapTimes = new List<float>();

    private void Start()
    {
        lapStartTime = Time.time;
    }

    private void Update()
    {
        float currentLapTime = Time.time - lapStartTime;
        string currentLapDisplay = $"Current Lap: {FormatTime(currentLapTime)}\n";
        string totalTimeDisplay = $"Total: {FormatTime(totalTime + currentLapTime)}\n\n";
        string lapHistory = GetLapHistory();

        lapTimeText.text = lapHistory + totalTimeDisplay + currentLapDisplay;
    }

    public void LapCompleted()
    {
        float lapTime = Time.time - lapStartTime;
        lapStartTime = Time.time;
        lapCount++;
        totalTime += lapTime;

        lapTimes.Insert(0, lapTime);
    }

    private string GetLapHistory()
    {
        string history = "";
        for (int i = 0; i < lapTimes.Count; i++)
        {
            history += $"Lap {lapTimes.Count - i}: {FormatTime(lapTimes[i])}\n";
        }
        return history;
    }

    private string FormatTime(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        int milliseconds = (int)((time * 1000) % 1000);
        return $"{minutes:00}:{seconds:00}.{milliseconds:000}";
    }

    public void SaveLapTimes()
    {
        for (int i = 0; i < lapTimes.Count; i++)
        {
            PlayerPrefs.SetString($"LapTime_{i + 1}", FormatTime(lapTimes[i]));
        }
        PlayerPrefs.SetString("TotalTime", FormatTime(totalTime));
        PlayerPrefs.Save();
    }

}
