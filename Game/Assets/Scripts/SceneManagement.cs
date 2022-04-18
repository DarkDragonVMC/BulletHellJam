using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneManagement : MonoBehaviour
{
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
