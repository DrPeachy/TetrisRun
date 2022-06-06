using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    public bool isOvered = false;
    public GameObject pausePage;
    bool isPaused = false;

    private void Awake() {
        Instance = this;
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape) && !isOvered){
            if(isPaused){
                Resume();
            }else{
                Pause();
            }
        }
    }

    public void LoadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
    public void Pause(){
        pausePage.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void Resume(){
        pausePage.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void Menu(){
        Time.timeScale = 1;
        SceneManager.LoadScene("StartMenu");
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
}
