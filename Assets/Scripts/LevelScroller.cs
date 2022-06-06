using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScroller : MonoBehaviour
{
    public Transform piece1;
    public Transform piece2;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        piece1.Translate(Vector3.right * speed * Time.deltaTime);
        piece2.Translate(Vector3.right * speed * Time.deltaTime);
        if(piece1.position.z < -78.1f)
        {
            piece1.position = piece2.position + new Vector3(0, 0, 154.8f);
        }
        else if(piece2.position.z < -78.1)
        {
            piece2.position = piece1.position + new Vector3(0, 0, 154.8f);
        }
    }
}
