using UnityEngine;
using UnityEngine.SceneManagement;

public class CarCheckpointHandler : MonoBehaviour
{
    public CheckpointsManager checkpointsManager { get; private set; }
    public LapTimer lapTimer { get; private set; }

    private int completedLaps = 0;
    private const int totalLaps = 3;

    public void SetReferences(CheckpointsManager checkpointManagerRef, LapTimer lapTimerRef)
    {
        this.checkpointsManager = checkpointManagerRef;
        this.lapTimer = lapTimerRef;
    }

    private void Start()
    {
        PlayerPrefs.SetString("CurrentRace", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (checkpointsManager == null || lapTimer == null)
        {
            Debug.LogError("CheckpointsManager o LapTimer not asigned.");
            return;
        }

        Checkpoint checkpoint = other.GetComponent<Checkpoint>();
        if (checkpoint != null)
        {
            checkpointsManager.CheckpointPassed(checkpoint);
            if (checkpointsManager.AllCheckpointsPassed())
            {
                lapTimer.LapCompleted();
                completedLaps++;
                Debug.Log(completedLaps);
                if (completedLaps >= totalLaps)
                {
                    lapTimer.SaveLapTimes();
                    SceneManager.LoadScene("PostRace");
                }
            }
        }
    }
}
