using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartMenuHandler : MonoBehaviour
{

    public TMP_InputField playerNameText;



    public void SaveName(){
        DataManager.Instance.playerName = playerNameText.text;
    }

    public void LoadScene(string sceneName)
    {
        if(playerNameText.text == null) DataManager.Instance.playerName = "Player";
        SceneManager.LoadScene(sceneName);
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
