using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceSelection : MonoBehaviour
{
    public string[] availableTracks;

    public void SelectTrack(int trackIndex)
    {
        if (trackIndex >= 0 && trackIndex < availableTracks.Length)
        {
            PlayerPrefs.SetString("SelectedTrack", availableTracks[trackIndex]);
            PlayerPrefs.Save();
            SceneManager.LoadScene(availableTracks[trackIndex]);
        }
        else
        {
            Debug.LogError("Race not valid.");
        }
    }
}
