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
    public int index;
    public humanScale _humanScale;
    //  human scale
    public class humanScale{
        public static readonly humanScale Short = new humanScale(0.5f);
        public static readonly humanScale Medium = new humanScale(1f);
        public static readonly humanScale Tall = new humanScale(1.5f);
        public static humanScale GetRandomHeight(){
            int i = Random.Range(0, 3);
            switch(i){
                case 0:
                    return humanScale.Short;
                case 1:
                    return humanScale.Medium;
                case 2:
                    return humanScale.Tall;
                default:
                    return humanScale.Medium;
            }
        }
        private humanScale(float scale)
            {
                this.scale = scale;
                this.height = (int)(2 * scale);
            }
        public float scale;
        public int height;
    }


    //  animator
    [SerializeField] private Animator _animator;

    private void Awake() {
        // set height
        _humanScale = humanScale.GetRandomHeight();
        transform.localScale = new Vector3(_humanScale.scale, _humanScale.scale, _humanScale.scale);

        _collider = GetComponent<Collider>();
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Start() {

    }
    private void Update() {
        if(true){
            //Collider[] tetris = Physics.OverlapBox(transform.position, new Vector3(0.5f, _collider.bounds.size.y, 0.5f), Quaternion.identity, tetrisMask);
            if(Physics.Raycast(transform.position, Vector3.forward, 0.1f, tetrisMask)){
                for(int i = 0; i < _humanScale.height; i++){
                    if(TetrisBoard.Instance.board[index, i].cubeStatus != TetrisBoard.cubeStatus.phantom &&
                        TetrisBoard.Instance.board[index, i].cubeStatus != TetrisBoard.cubeStatus.empty){
                            Destroy(gameObject);
                        }
                }
            }
        }
        speed += Time.deltaTime * speedIncrement;
        _rigidBody.velocity = Vector3.forward * speed;
        _animator.SetFloat("Speed", speed);
    }


}
