using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultsManager : MonoBehaviour
{
    public TMP_Text resultsText;
    private string raceToReturn;

    private void Start()
    {
        raceToReturn = PlayerPrefs.GetString("CurrentRace", "RaceScene");

        string results = "";
        for (int i = 1; i <= 3; i++)
        {
            string lapTime = PlayerPrefs.GetString($"LapTime_{i}", "N/A");
            results += $"Lap {i}: {lapTime}\n";
        }

        string totalTime = PlayerPrefs.GetString("TotalTime", "N/A");
        results += $"\nTotal Time: {totalTime}";

        resultsText.text = results;
    }

    public void RestartRace()
    {
        SceneManager.LoadScene(raceToReturn);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
