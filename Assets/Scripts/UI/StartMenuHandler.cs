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

    public GameObject savedMessage;


    public void SavePlayerName(){
        StartCoroutine(SaveName());
    }
    IEnumerator SaveName(){
        if(playerNameText.text != ""){
            DataManager.Instance.playerName = playerNameText.text;
            PlayerManager.Instance.SetPlayerName();
            yield return new WaitUntil(() => !savedMessage.activeInHierarchy);
            yield return ShowSavedMessage();
        }
    }

    public void LoadScene(string sceneName)
    {
        // if(playerNameText.text == ""){
        //     DataManager.Instance.playerName = PlayerPrefs.GetString("PlayerID");
        //     PlayerManager.Instance.SetPlayerName();
        // }
        SceneManager.LoadScene(sceneName);
    }
    
    IEnumerator ShowSavedMessage(){
        savedMessage.SetActive(true);
        yield return new WaitForSeconds(2f);
        savedMessage.SetActive(false);
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
