using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class LeaderBoard : MonoBehaviour
{
    int leaderboardID = 3423;

    void Start()
    {
        
    }

    public IEnumerator SubmitScoreRoutine(int scoreToUpload){
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderboardID, (response) =>{
            if(response.success){
                Debug.Log("Sucessfully uploaded score");
                done = true;
            }
            else{
                Debug.Log("Failed" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(()=> done == false);
    }
}
