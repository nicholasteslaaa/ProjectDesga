using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    int currentLevel = 1;
    public Animator[] animators;
    Animator currentAnimator;
    void Start()
    {
        SaveFile saves = SaveSystem.loadSaveFile();
        currentLevel = saves.currentLevel;

        string currentCutsceneName = $"Cutscene{currentLevel}";
        for (int i = 0; i < animators.Length; i++)
        {
            if (animators[i].gameObject.name == currentCutsceneName)
            {
                animators[i].gameObject.SetActive(true);
                currentAnimator = animators[i];
            }
            else
            {
                animators[i].gameObject.SetActive(false);
            }
        }
    }

    public void nextCurrentScene()
    {
        currentAnimator.SetTrigger("Next");
    }
    
}
