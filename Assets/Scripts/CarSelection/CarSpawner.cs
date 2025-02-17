using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject[] availableCars;

    void Start()
    {
        string selectedCarName = PlayerPrefs.GetString("SelectedCar", "");

        if (!string.IsNullOrEmpty(selectedCarName))
        {
            GameObject selectedCarPrefab = System.Array.Find(availableCars, car => car.name == selectedCarName);
            if (selectedCarPrefab != null)
            {
                GameObject carInstance = Instantiate(selectedCarPrefab, spawnPoint.position, spawnPoint.rotation);

                CarCameraController camController = FindObjectOfType<CarCameraController>();
                if (camController != null)
                {
                    camController.SetTarget(carInstance.transform);
                }

                CarCheckpointHandler checkpointHandler = carInstance.GetComponent<CarCheckpointHandler>();
                if (checkpointHandler != null)
                {
                    checkpointHandler.SetReferences(FindObjectOfType<CheckpointsManager>(), FindObjectOfType<LapTimer>());
                }
            }
            else
            {
                Debug.LogError("Car not found.");
            }
        }
    }
}
