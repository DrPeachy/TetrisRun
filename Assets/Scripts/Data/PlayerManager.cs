using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoginRoutine());
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
