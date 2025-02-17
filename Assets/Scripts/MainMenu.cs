using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string carSelectionScene = "CarSelection";

    public void StartGame()
    {
        SceneManager.LoadScene(carSelectionScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
