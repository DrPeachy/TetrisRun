using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBoard : MonoBehaviour
{
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
    tetriminos CMTtype;

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
    }

    // Update is called once per frame
    void Update()
    {
        SpawnTetrimino();
        HandleInput();
        ApplyGravity();
        //RefreshDisplay();

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
                    board[CMT[i].Item1, CMT[i].Item2].renderer.material.color = colorI;
                }
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
                    board[CMT[i].Item1, CMT[i].Item2].renderer.material.color = colorO;
                }
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
                    board[CMT[i].Item1, CMT[i].Item2].renderer.material.color = colorT;
                }
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
                    board[CMT[i].Item1, CMT[i].Item2].renderer.material.color = colorJ;
                }
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
                    board[CMT[i].Item1, CMT[i].Item2].renderer.material.color = colorL;
                }
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
                    board[CMT[i].Item1, CMT[i].Item2].renderer.material.color = colorS;
                }
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
                    board[CMT[i].Item1, CMT[i].Item2].renderer.material.color = colorZ;
                }
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
                    board[CMT[i].Item1, CMT[i].Item2].renderer.material.color = colorEmpty;
                }
                for (int i = 0; i < CMT.Length; i++)
                {
                    CMT[i].Item1 -= 1;
                    board[CMT[i].Item1, CMT[i].Item2].cubeStatus = cubeStatus.moving;
                    board[CMT[i].Item1, CMT[i].Item2].renderer.material.color = TetriminoColor(CMTtype);
                }
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
                    board[CMT[i].Item1, CMT[i].Item2].renderer.material.color = colorEmpty;
                }
                for (int i = 0; i < CMT.Length; i++)
                {
                    CMT[i].Item1 += 1;
                    board[CMT[i].Item1, CMT[i].Item2].cubeStatus = cubeStatus.moving;
                    board[CMT[i].Item1, CMT[i].Item2].renderer.material.color = TetriminoColor(CMTtype);
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
                    board[CMT[i].Item1, CMT[i].Item2].renderer.material.color = colorEmpty;
                }
                for (int i = 0; i < CMT.Length; i++)
                {
                    CMT[i].Item2 -= 1;
                    board[CMT[i].Item1, CMT[i].Item2].cubeStatus = cubeStatus.moving;
                    board[CMT[i].Item1, CMT[i].Item2].renderer.material.color = TetriminoColor(CMTtype);
                }
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
