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
        StartCoroutine(LeaderBoard.Instance.FetchTopFiftyscoresRoutine());
        playerNameText.text = LeaderBoard.Instance.leaderboardPlayerNameText;
        playerScoreText.text = LeaderBoard.Instance.leaderboardPlayerScoreText;
    }

    public void BackToMenu(){
        SceneManager.LoadScene("StartMenu");
    }


}


