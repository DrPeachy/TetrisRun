using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardMover : MonoBehaviour
{
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // Get speed from boardHandler
        moveSpeed = BoardHandler.Instance.moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // move board backward
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        if (transform.position.z < Camera.main.transform.position.z)
        {
            Destroy(gameObject);
            BoardHandler.Instance.hasBoard = false;
            HumanManager.Instance.AddRoundScore();
        }
    }
}
