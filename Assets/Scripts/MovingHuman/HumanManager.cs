using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class HumanManager : MonoBehaviour
{
    public static HumanManager Instance;
    //  human generate
    public GameObject humanPrefab;
    public Transform generateLeftMost; // left most human position
    public float intervalLength; // interval between each human
    public int pathNumber = 10; // same as human number

    //  human reference
    private GameObject[] humans;
    private HumanCode[] humanCodes;

    //  UI
    public TextMeshProUGUI scoreText;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        GenerateHuman();
        //StartCoroutine(ResetHuman());
        scoreText.text = $"{DataManager.Instance.playerName}'s score: {DataManager.Instance.playerScore}";
        
    }
    void GenerateHuman(){
        //  clear out global height list
        PubVar.humanHeights = new List<int>(new int[pathNumber]);
        humans = new GameObject[pathNumber];
        humanCodes = new HumanCode[pathNumber];

        //  generate huamns
        for(int i = 0; i < pathNumber; i++){
            Debug.Log($"create human number.{i}");
            GameObject newHuman = Instantiate(humanPrefab, generateLeftMost.position + i * Vector3.right * intervalLength, Quaternion.identity);
            newHuman.GetComponent<HumanCode>().index = i;
            humans[i] = newHuman;
            humanCodes[i] = newHuman.GetComponent<HumanCode>();

            //  set height to global
            PubVar.humanHeights[i] = newHuman.GetComponent<HumanCode>()._humanScale.height;

            //  check gameover
            InvokeRepeating("CheckGameOver", 3f, 0.2f);
        }
    }

    
    //  for debug
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

    public void AddRoundScore(){
        float multiplier = BoardHandler.Instance.moveSpeed;
        int sum = 0;
        for(int i = 0; i < pathNumber; i++){
            if(humans[i] != null){
                sum += humanCodes[i]._humanScale.height * (int)(100f * multiplier);
            }
        }
        DataManager.Instance.playerScore += sum;
        scoreText.text = $"{DataManager.Instance.playerName}'s score: {DataManager.Instance.playerScore}";
    }


    //  check if all human die out
    void CheckGameOver(){
        foreach(var i in humans){
            if(i != null) return;
        }
        Debug.Log("Gameover");
        StartCoroutine(LeaderBoard.Instance.SubmitScoreRoutine(DataManager.Instance.playerScore));
        SceneManager.LoadScene("EndScene");
    }

}
