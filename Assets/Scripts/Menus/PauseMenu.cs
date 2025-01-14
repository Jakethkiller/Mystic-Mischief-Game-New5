using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public bool Paused;

    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Button defaultBtn;

    private ControlsforPlayer playerControls;
    private Reload reload;
    private Scene scene;
    public string activeScene;

    //public ASyncLoadManager asyncLoadManager; 

    private void Awake()
    {
       //asyncLoadManager = new ASyncLoadManager();
        scene = SceneManager.GetActiveScene();
        playerControls = new ControlsforPlayer();
        if (GameObject.Find("Saves"))
        {
            reload = GameObject.Find("Saves").GetComponent<Reload>();
        }
    }

    void Update()
    {
        activeScene = PlayerPrefs.GetString("sceneName", "Level 1");
            if (GameIsPaused == true)
            {
                Paused = true;
            }
            if (GameIsPaused == false)
            {
                Paused = false;
            }

            if (playerControls.Pause.PauseGame.triggered)
            {

                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }

            if (playerControls.Pause.QuitGame.triggered)
            {
                QuitGame();
            }
        
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; //resumes game time
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        AudioListener.pause = false;
    }

    void Pause()
    {
        defaultBtn.Select();
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; //stops the game
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;

        AudioListener.pause = true;
    }

    public void LoadMenu()
    {
        Resume();

        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Level 1")
        {
            SetString("SceneName", scene.name);
        }
        if (scene.name == "Wizard Hub")
        {
            SetString("SceneName", scene.name);
        }
        if (scene.name == "Wiz Hub Variant")
        {
            SetString("SceneName", scene.name);
        }

        if (scene.name == "Level 2")
        {
            SetString("SceneName", scene.name);
        }
        if (scene.name == "Level 3")
        {
            SetString("SceneName", scene.name);
        }


        //SceneManager.LoadScene("Main Menu");
        //asyncLoadManager.GoToLevel("Main Menu");
        Debug.Log("Loading menu...");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void Focus() 
    {
        defaultBtn.Select();
    }

    private void OnApplicationFocus(bool focus)
    {
        Focus();
    }


    public void ChangeDefaultBtn(Button button)
    {
        defaultBtn = button;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void SetString(string KeyName, string Text)
    {
        PlayerPrefs.SetString(KeyName, Text);
    }

    public string GetString(string KeyName)
    {
        return PlayerPrefs.GetString(KeyName);
    }
}
