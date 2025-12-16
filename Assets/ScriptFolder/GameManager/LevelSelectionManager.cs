using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class LevelSelectionManager : MonoBehaviour
{

    public int unlockedLevel = 0;
    public int[] levelScore = {0,0,0,0,0,0,0};

    public Button[] buttons;

    public GameObject[] stageSelection;
    public int currentIdx = 0;
    public CinemachineCamera cinemachineCamera;


    void Start()
    {
        Time.timeScale = 1f;
        unlockedLevel = SaveSystem.loadSaveFile().unlockedLevel;
        for (int i = 0; i < buttons.Length; i++)
        {        
            buttons[i].interactable = int.Parse(buttons[i].name) <= unlockedLevel;
        }
    }

    void Update()
    {
        cinemachineCamera.Target.TrackingTarget = stageSelection[currentIdx].transform;
    }

    public void ChangeStage(int idx)
    {
        if ((idx < 0 && currentIdx <= 0) || 
            (idx > 0 && currentIdx >= stageSelection.Length - 1))
        {
            return;
        }

        currentIdx += idx;
    }

    public void changeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void saveCurrentLevel(int level)
    {
        GameManagerScript gameManager = new GameManagerScript();
        gameManager.load();
        gameManager.currentLevel = level;
        gameManager.saving();
    }
}
