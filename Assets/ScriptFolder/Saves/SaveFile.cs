using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFile {
    public int unlockedLevel = 1;
    public int currentLevel = 1;
    public int[] levelScore = {0,0,0,0,0,0,0};

    public SaveFile(GameManagerScript gameManager)
    {
        unlockedLevel = gameManager.unlockedLevel;
        currentLevel = gameManager.currentLevel;
        levelScore = gameManager.levelScore;
    }

}
