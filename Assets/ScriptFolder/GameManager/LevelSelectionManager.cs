using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour
{
    public GameObject[] stageSelection;
    public int currentIdx = 0;
    public CinemachineCamera cinemachineCamera;

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
}
