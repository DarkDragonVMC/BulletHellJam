using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SceneManagement : MonoBehaviour
{

    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") SettingsManager.loadSettings(GameObject.Find("Global Volume (Effects)").GetComponent<Volume>());
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings");
    }
    public void backToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void openTutorial()
    {
        SceneManager.LoadScene("HowToPlay");
    }
    public void startGame()
    {
        SceneManager.LoadScene("Prototype");
    }
    public void quit()
    {
        Application.Quit();
    }
}
