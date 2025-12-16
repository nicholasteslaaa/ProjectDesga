using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1f;
        if (SaveSystem.loadSaveFile() == null)
        {
            SaveSystem.NewSaveFile();
        }
    }
    public void changeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void deleteSave()
    {
        SaveSystem.DeleteSaveFile();
        SaveSystem.NewSaveFile();
    }

}
