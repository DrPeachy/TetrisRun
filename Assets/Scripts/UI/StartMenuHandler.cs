using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using LootLocker.Requests;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartMenuHandler : MonoBehaviour
{

    public TMP_InputField playerNameText;



    public void SaveName(){
        DataManager.Instance.playerName = playerNameText.text;
        PlayerManager.Instance.SetPlayerName();
    }

    public void LoadScene(string sceneName)
    {
        if(playerNameText.text == ""){
            DataManager.Instance.playerName = "Player" + PlayerPrefs.GetString("PlayerID");
            PlayerManager.Instance.SetPlayerName();
        }
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
