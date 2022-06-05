using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    //  data to save
    public string playerName;
    public int playerScore;

    private void Awake() {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else{
            Destroy(gameObject);
            return;
        }
    }

    

}
