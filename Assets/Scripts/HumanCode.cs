using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HumanCode : MonoBehaviour
{
    // human movement
    [SerializeField] private float speed = 1f;
    public float speedIncrement = 0.08f;

    // obstacle detect
    public LayerMask tetrisMask;
    [SerializeField] private Rigidbody _rigidBody;
    private Collider _collider;
    public int height;

    //  animator
    [SerializeField] private Animator _animator;

    private void Start() {
        // set height
        height = Random.Range(1, 4);
        transform.localScale = new Vector3(1, height, 1);

        _collider = GetComponent<Collider>();
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

    }
    private void Update() {
        if(true){
            // RaycastHit hit;
            // Debug.DrawRay(transform.position, Vector3.up * _collider.bounds.size.y, Color.blue);
            // if(Physics.Raycast(transform.position, Vector3.up, out hit, _collider.bounds.size.y, tetris)){
            //     //gameObject.SetActive(false);
            //     Destroy(gameObject);
            // }
            Collider[] tetris = Physics.OverlapBox(transform.position, new Vector3(0.5f, _collider.bounds.size.y, 0.5f), Quaternion.identity, tetrisMask);
            if(tetris.Length > 0){
                Destroy(gameObject);
            }
        }

        speed += Time.deltaTime * speedIncrement;
        _rigidBody.velocity = Vector3.forward * speed;
        _animator.SetFloat("Speed", speed);
    }


}
