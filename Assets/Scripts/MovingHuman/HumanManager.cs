using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HumanManager : MonoBehaviour
{
    public GameObject humanPrefab;
    public Transform generateLeftMost; // left most human position
    public float intervalLength; // interval between each human
    public int pathNumber = 10; // same as human number
    private GameObject[] humans;

    private void Awake() {
    }

    private void Start() {
        GenerateHuman();
        StartCoroutine(ResetHuman());
        
    }
    void GenerateHuman(){
        //  clear out global height list
        PubVar.humanHeights = new List<int>(new int[pathNumber]);
        humans = new GameObject[pathNumber];

        //  generate huamns
        for(int i = 0; i < pathNumber; i++){
            Debug.Log($"create human number.{i}");
            GameObject newHuman = Instantiate(humanPrefab, generateLeftMost.position + i * Vector3.right * intervalLength, Quaternion.identity);
            newHuman.GetComponent<HumanCode>().index = i;
            humans[i] = newHuman;

            //  set height to global
            PubVar.humanHeights[i] = newHuman.GetComponent<HumanCode>()._humanScale.height;

            //  check gameover
            InvokeRepeating("CheckGameOver", 3f, 0.2f);
        }
    }

    //  check if all human die out
    void CheckGameOver(){
        foreach(var i in humans){
            if(i != null) return;
        }
        Debug.Log("Gameover");
        SceneManager.LoadScene("end");
    }
    
    IEnumerator ResetHuman(){
        while(true){
            yield return new WaitForSeconds(6f);
            for(int i = 0; i < pathNumber; i++){
                if(humans[i] != null){
                    humans[i].transform.position = generateLeftMost.position + i * Vector3.right * intervalLength;
                }
            }
        }
    }

}
