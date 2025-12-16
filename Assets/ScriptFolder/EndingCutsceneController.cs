using UnityEngine;

public class EndingCutsceneController : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        Time.timeScale = 1f;
    }

    public void nextCurrentScene()
    {
        animator.SetTrigger("Next");
    }
}
