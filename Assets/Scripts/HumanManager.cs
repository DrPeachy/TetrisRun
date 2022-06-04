using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanManager : MonoBehaviour
{
    public GameObject humanPrefab;
    public Transform generateLeftMost; // left most human position
    public float intervalLength; // interval between each human
    public int pathNumber = 10; // same as human number
    private GameObject[] humans;

    private void Awake() {
        GenerateHuman();
    }

    void GenerateHuman(){
        //  clear out global height list
        PubVar.humanHeights = new List<int>(new int[pathNumber]);
        humans = new GameObject[pathNumber];

        //  generate huamns from pool
        for(int i = 0; i < pathNumber; i++){
            Debug.Log($"create human number.{i}");
            GameObject newHuman = Instantiate(humanPrefab, generateLeftMost.position + i * Vector3.right * intervalLength, Quaternion.identity);
            humans[i] = newHuman;
            PubVar.humanHeights[i] = newHuman.GetComponent<HumanCode>().height;
            // if(newHuman != null){
            //     newHuman.transform.position = generateLeftMost.position + i * Vector3.right * intervalLength;
            //     newHuman.SetActive(true);
            // }
            InvokeRepeating("CheckGameOver", 3f, 0.2f);
        }

        
    }

    //  check if all human die out
    bool CheckGameOver(){
        foreach(var i in humans){
            if(i != null) return true;
        }
        Debug.Log("Gameover");
        return false;
    }


}
