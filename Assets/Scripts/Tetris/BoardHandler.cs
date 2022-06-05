using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHandler : MonoBehaviour
{
    public static BoardHandler Instance;
    public GameObject boardPrefab;
    public float moveSpeed = 3f;
    [SerializeField] private float speedIncrement = 0.3f;
    public bool hasBoard = false;
    public Transform generatePosition;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        generateBoard();
    }


    void generateBoard(){
        if(!hasBoard){
            Instantiate(boardPrefab, generatePosition.position, Quaternion.identity);
            moveSpeed += speedIncrement;
            hasBoard = true;
        }
    }

}
