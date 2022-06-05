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
    public GameObject catPrefab;
    public GameObject dogPrefab;
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
        GenerateCreature();
        //StartCoroutine(ResetHuman());
        scoreText.text = $"{DataManager.Instance.playerName}'s score: {DataManager.Instance.playerScore}";
        
    }
    void GenerateCreature(){
        //  clear out global height list
        PubVar.humanHeights = new List<int>(new int[pathNumber]);
        humans = new GameObject[pathNumber];
        humanCodes = new HumanCode[pathNumber];

        //  generate huamns
        int humanNum = 0;
        for(int i = 0; i < pathNumber; i++){
            int flag = Random.Range(0, 3);
            GameObject creatureToGenerate;
            switch(flag){
                case 0:
                    if(humanNum < 2){
                        creatureToGenerate = humanPrefab;
                        humanNum++;
                    }
                    else{
                        creatureToGenerate = catPrefab;
                        flag = 1;
                    }
                    break;
                case 1:
                    creatureToGenerate = catPrefab;
                    break;
                case 2:
                    creatureToGenerate = dogPrefab;
                    break;
                default:
                    creatureToGenerate = null;
                    break;

            }
            GameObject newCreature = Instantiate(creatureToGenerate, generateLeftMost.position + i * Vector3.right * intervalLength, creatureToGenerate.transform.rotation);
            humans[i] = newCreature;
            humanCodes[i] = newCreature.GetComponent<HumanCode>();
            switch(flag){
                case 0:
                    humanCodes[i].height = 2;
                    break;
                default:
                    humanCodes[i].height = 1;
                    break;
            }
            humanCodes[i].index = i;

            //  set height to global
            PubVar.humanHeights[i] = humanCodes[i].height;

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
                sum += humanCodes[i].height * (int)(100f * multiplier);
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
