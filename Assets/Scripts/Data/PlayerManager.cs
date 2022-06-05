using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

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
        StartCoroutine(SetupRoutine());

    }

    IEnumerator SetupRoutine(){
        yield return LoginRoutine();
        yield return LeaderBoard.Instance.FetchTopFiftyscoresRoutine();
    }

    public void SetPlayerName(){
        LootLockerSDKManager.SetPlayerName(DataManager.Instance.playerName, (response) =>
        {
            if(response.success){
                Debug.Log("Successfully set player name");
            }else{
                Debug.Log("Could not set player name" + response.Error);
            }
        });
    }

    IEnumerator LoginRoutine(){
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) => 
        {
            if(response.success){
                Debug.Log("Player was logged in");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else{
                Debug.Log("Could not start session");
                done = true;
            }
        });
        yield return new WaitWhile(()=>done == false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
