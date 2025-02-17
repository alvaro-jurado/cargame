using UnityEngine;

public class CheckpointsManager : MonoBehaviour
{
    public Checkpoint[] checkpoints;
    private int currentCheckpointIndex = 0;
    public LapTimer lapTimer;
    private bool allCheckpointsPassed = false;

    private void Start()
    {
        UpdateCheckpoints();
    }

    public void CheckpointPassed(Checkpoint checkpoint)
    {
        if (checkpoint == checkpoints[currentCheckpointIndex])
        {
            currentCheckpointIndex++;

            if (currentCheckpointIndex >= checkpoints.Length)
            {
                allCheckpointsPassed = true;
                currentCheckpointIndex = 0;
            }

            UpdateCheckpoints();
        }
    }

    public bool AllCheckpointsPassed()
    {
        if (allCheckpointsPassed)
        {
            allCheckpointsPassed = false;
            return true;
        }
        return false;
    }

    private void UpdateCheckpoints()
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (i == currentCheckpointIndex)
                checkpoints[i].SetActive();
            else
                checkpoints[i].SetInactive();
        }
    }
}
