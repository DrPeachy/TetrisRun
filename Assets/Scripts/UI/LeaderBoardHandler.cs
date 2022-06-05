using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LeaderBoardHandler : MonoBehaviour
{
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI playerScoreText;
    void Start()
    {
        StartCoroutine(LoadBoard());

    }
    public void BackToMenu(){
        SceneManager.LoadScene("StartMenu");
    }
    
    IEnumerator LoadBoard(){
        playerNameText.text = "Loading..";
        yield return LeaderBoard.Instance.FetchTopFiftyscoresRoutine();
        yield return new WaitForSeconds(1f);
        playerNameText.text = LeaderBoard.Instance.leaderboardPlayerNameText;
        playerScoreText.text = LeaderBoard.Instance.leaderboardPlayerScoreText;
    }


}


