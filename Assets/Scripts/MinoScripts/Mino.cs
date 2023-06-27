using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Mino : MonoBehaviour
{
    public float previousTime; //�l���w�肵�Ȃ���΂O�ŏ����������B
    // mino��������^�C��
    public float fallTime = 1f;

    public GameObject backGround;

    //�X�e�[�W�̑傫��
    private static int width;
    private static int height;

    //mino��]
    public Vector3 rotationPoint;

    //�z��Ɉʒu�����i�[����
    private static Transform[] grid = new Transform[200]; //�T�C�Y�ύX�͂P���z�񂵂��ł��Ȃ�

    void Start()
    {
        backGround = GameObject.Find("Background");
        width = Mathf.RoundToInt(backGround.GetComponent<SpriteRenderer>().bounds.size.x); //SpriteRenderer�̃T�C�Y��float
        height = Mathf.RoundToInt(backGround.GetComponent<SpriteRenderer>().bounds.size.y);
        Array.Resize(ref grid, width * height);
    }

    void Update()
    {
        MinoMovement();
    }

    private void MinoMovement()
    {
        //�����orA�L�[�ō��ɓ���
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown("a"))
        {
            transform.position += new Vector3(-1, 0, 0);

            if(!ValidMovement())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }

        }
        //�E���orD�L�[�ŉE�ɓ���
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown("d"))
        {
            transform.position += new Vector3(1, 0, 0);

            if (!ValidMovement())
            {
                transform.position -= new Vector3(1, 0, 0);
            }

        }
        //�����ŉ��Ɉړ������A�����L�[�ł��ړ�����
        else if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown("s") || 
            Time.time - previousTime >= fallTime)
        {
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMovement())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                CheckLines();
                this.enabled = false;
                FindObjectOfType<SpawnMino>().NewMino(FindObjectOfType<ShowNextMino>().NextMinoUpdate()); //�����Ă���~�m�������\�b�h���ĂԂ��Ƃł݂̂��쐬����
            }
            previousTime = Time.time; //������Time.time�̓Q�[�����̎��ԂŃ��A�����ԂłȂ����Ƃɒ���

        }
        //����L�[�ŉ�]�B�O���[�o����Ԃōl����
        else if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown("w"))
        {
            //RotateAround�̓��[���h���W�̒��S�_�A��]���A�x���@�̉�]�p�x�������Ƃ��ĉ�]�ł���B
            //��]����z��(�������O)�Ȃ̂Ń~�m�̉摜�ƕ��s�Ȗʏ�ŉ�]�ł���B
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
        }
    }

    // ���C���̊m�F
    public void CheckLines()
    {
        for(int i = height-1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    // �񂪂�����Ă��邩�m�F
    bool HasLine(int i)
    {
        for(int j = 0; j < width; j++)
        {
            if (grid[j+i*width] == null)
                return false;
        }

        FindObjectOfType<GameManagement>().AddScore();
        return true;
    }

    // ���C��������
    void DeleteLine(int i)
    {
        for(int j = 0; j < width; j++)
        {
            Destroy(grid[j+i*width].gameObject);
            grid[j+i*width] = null;
        }
    }

    // ���������
    public void RowDown(int i)
    {
        for(int y = i; y < height; y++)
        {
            for(int j = 0; j < width; j++)
            {
                if(grid[j+y*width] != null)
                {
                    grid[j+(y-1)*width] = grid[j+y*width];
                    grid[j+y*width] = null;
                    grid[j+(y-1)*width].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    void AddToGrid()
    {
        foreach(Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);
            grid[roundX+roundY*width] = children;

            if(roundY >= height - 1)
            {
                FindObjectOfType<GameManagement>().GameOver();
            }
        }
    }

    // mino�̈ړ��͈͂̐���
    bool ValidMovement()
    {
        foreach(Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            //mino���X�e�[�W����͂ݏo���Ȃ��悤�ɁA�S�Ă̎q�I�u�W�F�N�g(�u���b�N)�̈ʒu���m�F����
            if (roundX < 0 || roundX >= width || roundY < 0 || roundY >= height)
            {
                return false;
            }

            if(grid[roundX+roundY*width] != null)
            {
                return false;
            }

        }
        return true;
    }
}
