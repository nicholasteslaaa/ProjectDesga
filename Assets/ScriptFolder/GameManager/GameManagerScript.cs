using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public int unlockedLevel = 1;
    public int currentLevel = 1;
    public int[] levelScore = {0,0,0,0,0,0,0};

    public string[] levelName = {"1","2","3","4","5","6","7"};

    public GameObject pausedScreen;
    public GameObject gameOverScreen;
    public bool isPaused = false;

    public GameObject scoreScreen;
    public bool isScoreScreenEnabled = false;
    string currentLevelName;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI rescuedText;
    
    PlayerComponentManager playerComponent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resume();
        currentLevelName = SceneManager.GetActiveScene().name;
        load();

        Debug.Log(SaveSystem.loadSaveFile().levelScore.ToString());

        if (levelName.Contains(currentLevelName))
        {
            pausedScreen.SetActive(isPaused);
            scoreScreen.SetActive(isScoreScreenEnabled);
            playerComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerComponentManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (levelName.Contains(currentLevelName))
        {
            GameObject[] NPCS = GameObject.FindGameObjectsWithTag("NPC");
            int NPC_Count = 0;
            for (int i = 0;i < NPCS.Length; i++)
            {
                NPC_Movement npc = NPCS[i].GetComponent<NPC_Movement>();
                if (npc.getHealth() > 0)
                {
                    NPC_Count += 1;
                }
            }

            if (NPC_Count <= 0 && currentLevelName != "7")
            {
                isScoreScreenEnabled = true;
                scoreScreen.SetActive(isScoreScreenEnabled);
                

                load();

                int currentLevelInt = int.Parse(currentLevelName);

                PlayerGameManager playerGameManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerComponentManager>().getPlayerGameManager();
                
                int currentScore = playerGameManager.getNumRescued()/playerGameManager.getTotalNPC();
                
                scoreText.text = $"Score: {(int) (Mathf.Floor(currentScore * 100f) / 100f)*100f}%";
                rescuedText.text = $"Rescued: {playerGameManager.getNumRescued()}/{playerGameManager.getTotalNPC()}";

                if (levelScore[currentLevelInt-1] <= currentScore)
                {
                    levelScore[currentLevelInt-1] = currentScore;
                }
                if (unlockedLevel <= currentLevelInt)
                {
                    unlockedLevel += 1;
                }
                saving();
                
                Time.timeScale = 0f;
            }
            if (playerComponent.getPlayerHealthHandler().getHealth() <= 0)
            {
                gameOverScreen.SetActive(true);
                Time.timeScale = 0f;
            }
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

    public void resume()
    {
        isPaused = false;
        pausedScreen.SetActive(isPaused);
        if (!isScoreScreenEnabled)
        {
            Time.timeScale = 1f;
        }
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void saving()
    {
        SaveSystem.saveFileData(this);
    }

    public void retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void load()
    {
        SaveFile saveFile = SaveSystem.loadSaveFile();
        unlockedLevel = saveFile.unlockedLevel;
        currentLevel = saveFile.currentLevel;
        levelScore = saveFile.levelScore;
    }

    public void changeScene(string scneName)
    {
        SceneManager.LoadScene(scneName);
    }
}
