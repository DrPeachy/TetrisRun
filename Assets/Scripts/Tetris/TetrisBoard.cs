using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBoard : MonoBehaviour
{
    public static TetrisBoard Instance;
    float moveSpeed;
    //
    public int boardLength;
    public int boardHeight;
    public GameObject cubePrefab;
    public float gravityInterval = 1f;

    float lastAppliedGravity;

    public enum cubeStatus
    {
        placed,
        empty,
        moving,
        phantom
    }
    public struct cubeProporties
    {
        public cubeStatus cubeStatus;
        public MeshRenderer renderer;
        public BoxCollider collider;
    }

    public cubeProporties[,] board;
    (int, int)[] CMT; //current moving tetrimino
    (float, float) CMTpivot;
    tetriminos CMTtype;
    (int, int)[] PT; //phantom tetrimino
    (int, int)[] lastPT;

    public enum tetriminos
    {
        I,
        O,
        T,
        J,
        L,
        S,
        Z
    }
    public Color colorI;
    public Color colorO;
    public Color colorT;
    public Color colorJ;
    public Color colorL;
    public Color colorS;
    public Color colorZ;
    public Color colorEmpty;

    private MaterialPropertyBlock propertyBlock;

    private void Awake()
    {
        Instance = this;
        if (propertyBlock == null)
            propertyBlock = new MaterialPropertyBlock();
    }

    // Start is called before the first frame update
    void Start()
    {
        board = new cubeProporties[boardLength, boardHeight];
        for(int i = 0; i < boardLength; i++)
        {
            for(int j = 0; j < boardHeight; j++)
            {
                GameObject cube = Instantiate(cubePrefab, transform.TransformPoint(new Vector3(i, j, 0)), Quaternion.identity, transform);
                board[i, j].cubeStatus = cubeStatus.empty;
                board[i, j].renderer = cube.GetComponent<MeshRenderer>();
                board[i, j].collider = cube.GetComponent<BoxCollider>();
            }
        }

        for(int i = 0; i < boardLength; i++)
        {
            board[i, boardHeight - 1].renderer.enabled = false;
            board[i, boardHeight - 2].renderer.enabled = false;
        }
        lastPT = new (int, int)[4];

        // Get speed from boardHandler
        moveSpeed = BoardHandler.Instance.moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnTetrimino();
        CalculuatePhantom();
        HandleInput();
        ApplyGravity();
        //RefreshDisplay();

        // move board backward
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        if(transform.position.z < Camera.main.transform.position.z){
            Destroy(gameObject);
            BoardHandler.Instance.hasBoard = false;
            HumanManager.Instance.AddRoundScore();
        }

    }

    void SpawnTetrimino()
    {
        if(CMT == null)
        {
            CMT = new (int, int)[4];
            CMTtype = (tetriminos)Random.Range(0, 7);
            if (CMTtype == tetriminos.I)
            {
                CMT[0] = (3, boardHeight - 1);
                CMT[1] = (4, boardHeight - 1);
                CMT[2] = (5, boardHeight - 1);
                CMT[3] = (6, boardHeight - 1);
                for (int i = 0; i < CMT.Length; i++)
                {
                    board[CMT[i].Item1, CMT[i].Item2].cubeStatus = cubeStatus.moving;
                    propertyBlock.SetColor("_Color", colorI);
                    board[CMT[i].Item1, CMT[i].Item2].renderer.SetPropertyBlock(propertyBlock);
                }
                CMTpivot = (4.5f, (float)boardHeight - 1.5f);
            }
            else if (CMTtype == tetriminos.O)
            {
                CMT[0] = (4, boardHeight - 1);
                CMT[1] = (5, boardHeight - 1);
                CMT[2] = (4, boardHeight - 2);
                CMT[3] = (5, boardHeight - 2);
                for (int i = 0; i < CMT.Length; i++)
                {
                    board[CMT[i].Item1, CMT[i].Item2].cubeStatus = cubeStatus.moving;
                    propertyBlock.SetColor("_Color", colorO);
                    board[CMT[i].Item1, CMT[i].Item2].renderer.SetPropertyBlock(propertyBlock);
                }
                CMTpivot = (4.5f, (float)boardHeight - 1.5f);
            }
            else if (CMTtype == tetriminos.T)
            {
                CMT[0] = (3, boardHeight - 2);
                CMT[1] = (4, boardHeight - 2);
                CMT[2] = (5, boardHeight - 2);
                CMT[3] = (4, boardHeight - 1);
                for(int i = 0; i < CMT.Length; i++)
                {
                    board[CMT[i].Item1, CMT[i].Item2].cubeStatus = cubeStatus.moving;
                    propertyBlock.SetColor("_Color", colorT);
                    board[CMT[i].Item1, CMT[i].Item2].renderer.SetPropertyBlock(propertyBlock);
                }
                CMTpivot = (4f, (float)boardHeight - 2f);
            }
            else if (CMTtype == tetriminos.J)
            {
                CMT[0] = (3, boardHeight - 2);
                CMT[1] = (4, boardHeight - 2);
                CMT[2] = (5, boardHeight - 2);
                CMT[3] = (3, boardHeight - 1);
                for (int i = 0; i < CMT.Length; i++)
                {
                    board[CMT[i].Item1, CMT[i].Item2].cubeStatus = cubeStatus.moving;
                    propertyBlock.SetColor("_Color", colorJ);
                    board[CMT[i].Item1, CMT[i].Item2].renderer.SetPropertyBlock(propertyBlock);
                }
                CMTpivot = (4f, (float)boardHeight - 2f);
            }
            else if (CMTtype == tetriminos.L)
            {
                CMT[0] = (3, boardHeight - 2);
                CMT[1] = (4, boardHeight - 2);
                CMT[2] = (5, boardHeight - 2);
                CMT[3] = (5, boardHeight - 1);
                for (int i = 0; i < CMT.Length; i++)
                {
                    board[CMT[i].Item1, CMT[i].Item2].cubeStatus = cubeStatus.moving;
                    propertyBlock.SetColor("_Color", colorL);
                    board[CMT[i].Item1, CMT[i].Item2].renderer.SetPropertyBlock(propertyBlock);
                }
                CMTpivot = (4f, (float)boardHeight - 2f);
            }
            else if (CMTtype == tetriminos.S)
            {
                CMT[0] = (4, boardHeight - 2);
                CMT[1] = (5, boardHeight - 2);
                CMT[2] = (5, boardHeight - 1);
                CMT[3] = (6, boardHeight - 1);
                for (int i = 0; i < CMT.Length; i++)
                {
                    board[CMT[i].Item1, CMT[i].Item2].cubeStatus = cubeStatus.moving;
                    propertyBlock.SetColor("_Color", colorS);
                    board[CMT[i].Item1, CMT[i].Item2].renderer.SetPropertyBlock(propertyBlock);
                }
                CMTpivot = (5f, (float)boardHeight - 2f);
            }
            else if (CMTtype == tetriminos.Z)
            {
                CMT[0] = (5, boardHeight - 2);
                CMT[1] = (6, boardHeight - 2);
                CMT[2] = (4, boardHeight - 1);
                CMT[3] = (5, boardHeight - 1);
                for (int i = 0; i < CMT.Length; i++)
                {
                    board[CMT[i].Item1, CMT[i].Item2].cubeStatus = cubeStatus.moving;
                    propertyBlock.SetColor("_Color", colorZ);
                    board[CMT[i].Item1, CMT[i].Item2].renderer.SetPropertyBlock(propertyBlock);
                }
                CMTpivot = (5f, (float)boardHeight - 2f);
            }
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            bool canMove = true;
            for (int i = 0; i < CMT.Length; i++)
            {
                if (CMT[i].Item1 - 1 < 0 || board[CMT[i].Item1 - 1, CMT[i].Item2].cubeStatus == cubeStatus.placed)
                {
                    canMove = false;
                    break;
                }
            }
            if (canMove)
            {
                for (int i = 0; i < CMT.Length; i++)
                {
                    board[CMT[i].Item1, CMT[i].Item2].cubeStatus = cubeStatus.empty;
                    propertyBlock.SetColor("_Color", colorEmpty);
                    board[CMT[i].Item1, CMT[i].Item2].renderer.SetPropertyBlock(propertyBlock);
                }
                for (int i = 0; i < CMT.Length; i++)
                {
                    CMT[i].Item1 -= 1;
                    board[CMT[i].Item1, CMT[i].Item2].cubeStatus = cubeStatus.moving;
                    propertyBlock.SetColor("_Color", TetriminoColor(CMTtype));
                    board[CMT[i].Item1, CMT[i].Item2].renderer.SetPropertyBlock(propertyBlock);
                }
                CMTpivot.Item1 -= 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            bool canMove = true;
            for (int i = 0; i < CMT.Length; i++)
            {
                if (CMT[i].Item1 + 1 >= boardLength || board[CMT[i].Item1 + 1, CMT[i].Item2].cubeStatus == cubeStatus.placed)
                {
                    canMove = false;
                    break;
                }
            }
            if (canMove)
            {
                for (int i = 0; i < CMT.Length; i++)
                {
                    board[CMT[i].Item1, CMT[i].Item2].cubeStatus = cubeStatus.empty;
                    propertyBlock.SetColor("_Color", colorEmpty);
                    board[CMT[i].Item1, CMT[i].Item2].renderer.SetPropertyBlock(propertyBlock);
                }
                for (int i = 0; i < CMT.Length; i++)
                {
                    CMT[i].Item1 += 1;
                    board[CMT[i].Item1, CMT[i].Item2].cubeStatus = cubeStatus.moving;
                    propertyBlock.SetColor("_Color", TetriminoColor(CMTtype));
                    board[CMT[i].Item1, CMT[i].Item2].renderer.SetPropertyBlock(propertyBlock);
                }
                CMTpivot.Item1 += 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            (int, int)[] newCMT = new (int, int)[4];
            for (int i = 0; i < CMT.Length; i++)
            {
                float x = CMT[i].Item1 - CMTpivot.Item1;
                float y = CMT[i].Item2 - CMTpivot.Item2;

                float x1 = CMTpivot.Item1 + (y);
                float y1 = CMTpivot.Item2 + (-x);

                newCMT[i].Item1 = Mathf.RoundToInt(x1);
                newCMT[i].Item2 = Mathf.RoundToInt(y1);
            }

            bool canRotate = true;
            for (int i = 0; i < newCMT.Length; i++)
            {
                if (newCMT[i].Item1 < 0 || newCMT[i].Item1 >= boardLength || newCMT[i].Item2 < 0 || newCMT[i].Item2 >= boardHeight || board[newCMT[i].Item1, newCMT[i].Item2].cubeStatus == cubeStatus.placed)
                {
                    canRotate = false;
                    break;
                }
            }
            if (canRotate)
            {
                for (int i = 0; i < CMT.Length; i++)
                {
                    board[CMT[i].Item1, CMT[i].Item2].cubeStatus = cubeStatus.empty;
                    propertyBlock.SetColor("_Color", colorEmpty);
                    board[CMT[i].Item1, CMT[i].Item2].renderer.SetPropertyBlock(propertyBlock);
                }
                CMT = newCMT;
                for (int i = 0; i < CMT.Length; i++)
                {
                    board[CMT[i].Item1, CMT[i].Item2].cubeStatus = cubeStatus.moving;
                    propertyBlock.SetColor("_Color", TetriminoColor(CMTtype));
                    board[CMT[i].Item1, CMT[i].Item2].renderer.SetPropertyBlock(propertyBlock);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            (int, int)[] newCMT = new (int, int)[4];
            for (int i = 0; i < CMT.Length; i++)
            {
                float x = CMT[i].Item1 - CMTpivot.Item1;
                float y = CMT[i].Item2 - CMTpivot.Item2;

                float x1 = CMTpivot.Item1 + (-y);
                float y1 = CMTpivot.Item2 + (x);

                newCMT[i].Item1 = Mathf.RoundToInt(x1);
                newCMT[i].Item2 = Mathf.RoundToInt(y1);
            }

            bool canRotate = true;
            for (int i = 0; i < newCMT.Length; i++)
            {
                if (newCMT[i].Item1 < 0 || newCMT[i].Item1 >= boardLength || newCMT[i].Item2 < 0 || newCMT[i].Item2 >= boardHeight || board[newCMT[i].Item1, newCMT[i].Item2].cubeStatus == cubeStatus.placed)
                {
                    canRotate = false;
                    break;
                }
            }
            if (canRotate)
            {
                for (int i = 0; i < CMT.Length; i++)
                {
                    board[CMT[i].Item1, CMT[i].Item2].cubeStatus = cubeStatus.empty;
                    propertyBlock.SetColor("_Color", colorEmpty);
                    board[CMT[i].Item1, CMT[i].Item2].renderer.SetPropertyBlock(propertyBlock);
                }
                CMT = newCMT;
                for (int i = 0; i < CMT.Length; i++)
                {
                    board[CMT[i].Item1, CMT[i].Item2].cubeStatus = cubeStatus.moving;
                    propertyBlock.SetColor("_Color", TetriminoColor(CMTtype));
                    board[CMT[i].Item1, CMT[i].Item2].renderer.SetPropertyBlock(propertyBlock);
                }
            }
        }
    }

    void ApplyGravity()
    {
        if(Time.time > lastAppliedGravity + gravityInterval)
        {
            bool canMoveDown = true;
            for (int i = 0; i < CMT.Length; i++)
            {
                if (CMT[i].Item2 - 1 < 0 || board[CMT[i].Item1, CMT[i].Item2 - 1].cubeStatus == cubeStatus.placed)
                {
                    canMoveDown = false;
                    break;
                }
            }
            if (canMoveDown)
            {
                for (int i = 0; i < CMT.Length; i++)
                {
                    board[CMT[i].Item1, CMT[i].Item2].cubeStatus = cubeStatus.empty;
                    propertyBlock.SetColor("_Color", colorEmpty);
                    board[CMT[i].Item1, CMT[i].Item2].renderer.SetPropertyBlock(propertyBlock);
                }
                for (int i = 0; i < CMT.Length; i++)
                {
                    CMT[i].Item2 -= 1;
                    board[CMT[i].Item1, CMT[i].Item2].cubeStatus = cubeStatus.moving;
                    propertyBlock.SetColor("_Color", TetriminoColor(CMTtype));
                    board[CMT[i].Item1, CMT[i].Item2].renderer.SetPropertyBlock(propertyBlock);
                }
                CMTpivot.Item2 -= 1;
            }
            else
            {
                for (int i = 0; i < CMT.Length; i++)
                {
                    board[CMT[i].Item1, CMT[i].Item2].cubeStatus = cubeStatus.placed;
                }
                CMT = null;
            }
            lastAppliedGravity = Time.time;
        }
    }

    void CalculuatePhantom()
    {
        bool contactGround = false;
        PT = new (int, int)[CMT.Length];
        for (int i = 0; i < CMT.Length; i++)
        {
            PT[i].Item1 = CMT[i].Item1;
            PT[i].Item2 = CMT[i].Item2;
        }
        while (!contactGround)
        {
            for (int i = 0; i < PT.Length; i++)
            {
                if (PT[i].Item2 - 1 < 0 || board[PT[i].Item1, PT[i].Item2 - 1].cubeStatus == cubeStatus.placed)
                {
                    contactGround = true;
                    break;
                }
            }
            if (!contactGround)
            {
                for(int i = 0; i < PT.Length; i++)
                {
                    PT[i].Item2 -= 1;
                }
            }
        }
        for(int i = 0; i < lastPT.Length; i++)
        {
            if(board[lastPT[i].Item1, lastPT[i].Item2].cubeStatus == cubeStatus.phantom)
            {
                board[lastPT[i].Item1, lastPT[i].Item2].cubeStatus = cubeStatus.empty;
                propertyBlock.SetColor("_Color", colorEmpty);
                board[lastPT[i].Item1, lastPT[i].Item2].renderer.SetPropertyBlock(propertyBlock);
            }
        }
        for(int i = 0; i < PT.Length; i++)
        {
            if(board[PT[i].Item1, PT[i].Item2].cubeStatus != cubeStatus.moving)
            {
                board[PT[i].Item1, PT[i].Item2].cubeStatus = cubeStatus.phantom;
                Color phantomColor = TetriminoColor(CMTtype);
                phantomColor.a = 0.2f;
                propertyBlock.SetColor("_Color", phantomColor);
                board[PT[i].Item1, PT[i].Item2].renderer.SetPropertyBlock(propertyBlock);
            }
        }
        for(int i = 0; i < PT.Length; i++)
        {
            lastPT[i].Item1 = PT[i].Item1;
            lastPT[i].Item2 = PT[i].Item2;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < PT.Length; i++)
            {
                board[PT[i].Item1, PT[i].Item2].cubeStatus = cubeStatus.placed;
                propertyBlock.SetColor("_Color", TetriminoColor(CMTtype));
                board[PT[i].Item1, PT[i].Item2].renderer.SetPropertyBlock(propertyBlock);
            }
            for(int i = 0; i < CMT.Length; i++)
            {
                board[CMT[i].Item1, CMT[i].Item2].cubeStatus = cubeStatus.empty;
                propertyBlock.SetColor("_Color", colorEmpty);
                board[CMT[i].Item1, CMT[i].Item2].renderer.SetPropertyBlock(propertyBlock);
            }
            CMT = null;
            SpawnTetrimino();
        }
    }

    void RefreshDisplay()
    {
        for (int i = 0; i < boardLength; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                if (board[i, j].cubeStatus == cubeStatus.moving)
                {
                    board[i, j].renderer.material.color = Color.red;
                }
                else if (board[i, j].cubeStatus == cubeStatus.empty)
                {
                    board[i, j].renderer.material.color = Color.white;
                }
                else if (board[i, j].cubeStatus == cubeStatus.placed)
                {
                    board[i, j].renderer.material.color = Color.blue;
                }
                else if (board[i, j].cubeStatus == cubeStatus.phantom)
                {
                    board[i, j].renderer.material.color = Color.green;
                }
            }
        }
    }

    Color TetriminoColor(tetriminos tetrimino)
    {
        if(tetrimino == tetriminos.I)
        {
            return colorI;
        }
        else if(tetrimino == tetriminos.O)
        {
            return colorO;
        }
        else if (tetrimino == tetriminos.T)
        {
            return colorT;
        }
        else if (tetrimino == tetriminos.J)
        {
            return colorJ;
        }
        else if (tetrimino == tetriminos.L)
        {
            return colorL;
        }
        else if (tetrimino == tetriminos.S)
        {
            return colorS;
        }
        else
        {
            return colorZ;
        }
    }

    

}
