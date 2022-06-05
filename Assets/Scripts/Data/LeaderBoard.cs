using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class LeaderBoard : MonoBehaviour
{
    public static LeaderBoard Instance;

    public string leaderboardPlayerNameText;
    public string leaderboardPlayerScoreText;
    int leaderboardID = 3423;
    private void Awake() {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(Instance);
        }else{
            Destroy(gameObject);
            return;
        }
    }
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

    public IEnumerator FetchTopFiftyscoresRoutine(){
        bool done = false;
        LootLockerSDKManager.GetScoreListMain(leaderboardID, 50, 0, (response) =>{
            if(response.success){
                string tempPlayerNames = "Names\n";
                string tempPlayerScores = "Scores\n";

                LootLockerLeaderboardMember[] members = response.items;

                for(int i = 0; i < members.Length; i++){
                    tempPlayerNames += members[i].rank + ".";
                    if(members[i].player.name != ""){
                        tempPlayerNames += members[i].player.name;
                    }else{
                        tempPlayerNames += members[i].player.id;
                    }
                    tempPlayerScores += members[i].score + "\n";
                    tempPlayerNames += "\n";
                }
                done = true;
                leaderboardPlayerNameText = tempPlayerNames;
                leaderboardPlayerScoreText = tempPlayerScores;
            }else{
                Debug.Log("Failed" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
}
