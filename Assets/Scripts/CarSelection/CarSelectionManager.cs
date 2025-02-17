using UnityEngine;
using UnityEngine.SceneManagement;

public class CarSelectionManager : MonoBehaviour
{
    public GameObject[] availableCars;

    public void SelectCar(int carIndex)
    {
        if (carIndex >= 0 && carIndex < availableCars.Length)
        {
            PlayerPrefs.SetString("SelectedCar", availableCars[carIndex].name);
            PlayerPrefs.Save();
            SceneManager.LoadScene("RaceSelection");
        }
        else
        {
            Debug.LogError("Car not valid.");
        }
    }
}
