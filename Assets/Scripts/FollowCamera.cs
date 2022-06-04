using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    Vector3 difference;
    private void Start() {
        //transform.LookAt(target);
        
        difference = target.position - transform.position;
    }
    void LateUpdate()
    {
        if(target != null)
            transform.position = target.position - difference;
    }
}
