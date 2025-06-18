using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void Start()
    {
        MusicManager.instance.PlayMusic("MenuMusic");
    }
    public void Play()
    {
        LevelManager.instance.LoadScene("Level1", "CrossFade");
        MusicManager.instance.PlayMusic("Gameplay");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
