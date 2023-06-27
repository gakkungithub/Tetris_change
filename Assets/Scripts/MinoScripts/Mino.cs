using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Mino : MonoBehaviour
{
    public float previousTime; //値を指定しなければ０で初期化される。
    // minoが落ちるタイム
    public float fallTime = 1f;

    public GameObject backGround;

    //ステージの大きさ
    private static int width;
    private static int height;

    //mino回転
    public Vector3 rotationPoint;

    //配列に位置情報を格納する
    private static Transform[] grid = new Transform[200]; //サイズ変更は１次配列しかできない

    void Start()
    {
        backGround = GameObject.Find("Background");
        width = Mathf.RoundToInt(backGround.GetComponent<SpriteRenderer>().bounds.size.x); //SpriteRendererのサイズはfloat
        height = Mathf.RoundToInt(backGround.GetComponent<SpriteRenderer>().bounds.size.y);
        Array.Resize(ref grid, width * height);
    }

    void Update()
    {
        MinoMovement();
    }

    private void MinoMovement()
    {
        //左矢印orAキーで左に動く
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown("a"))
        {
            transform.position += new Vector3(-1, 0, 0);

            if(!ValidMovement())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }

        }
        //右矢印orDキーで右に動く
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown("d"))
        {
            transform.position += new Vector3(1, 0, 0);

            if (!ValidMovement())
            {
                transform.position -= new Vector3(1, 0, 0);
            }

        }
        //自動で下に移動させつつ、下矢印キーでも移動する
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
                FindObjectOfType<SpawnMino>().NewMino(FindObjectOfType<ShowNextMino>().NextMinoUpdate()); //落ちてからミノ発生メソッドを呼ぶことでみのを作成する
            }
            previousTime = Time.time; //ここのTime.timeはゲーム内の時間でリアル時間でないことに注意

        }
        //上矢印キーで回転。グローバル空間で考える
        else if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown("w"))
        {
            //RotateAroundはワールド座標の中心点、回転軸、度数法の回転角度を引数として回転できる。
            //回転軸はz軸(奥から手前)なのでミノの画像と平行な面上で回転できる。
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
        }
    }

    // ラインの確認
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

    // 列がそろっているか確認
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

    // ラインを消す
    void DeleteLine(int i)
    {
        for(int j = 0; j < width; j++)
        {
            Destroy(grid[j+i*width].gameObject);
            grid[j+i*width] = null;
        }
    }

    // 列を下げる
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

    // minoの移動範囲の制御
    bool ValidMovement()
    {
        foreach(Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            //minoがステージからはみ出さないように、全ての子オブジェクト(ブロック)の位置を確認する
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
