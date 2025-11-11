using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject pausedScreen;
    public bool isPaused = false;

    public GameObject scoreScreen;
    public bool isScoreScreenEnabled = false;

    PlayerComponentManager playerComponent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pausedScreen.SetActive(isPaused);
        scoreScreen.SetActive(isScoreScreenEnabled);
        playerComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerComponentManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("NPC").Length <= 0)
        {
            isScoreScreenEnabled = true;
            scoreScreen.SetActive(isScoreScreenEnabled);
            Time.timeScale = 0f;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            pausedScreen.SetActive(isPaused);
            if (isPaused)
            {
                Time.timeScale = 0f;
            }
            else if (!isScoreScreenEnabled)
            {
                Time.timeScale = 1f;
            }
        }
        

    }
}
