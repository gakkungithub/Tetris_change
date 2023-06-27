using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextMino : MonoBehaviour
{
    public Sprite[] NextMinos;
    public Image Mino_image;
    public int minoNum;
    private int num;

    // Start is called before the first frame update
    void Start()
    {
        NextMinoUpdate();
    }

    // Update is called once per frame
    public int NextMinoUpdate()
    {
        num = minoNum;
        minoNum = Random.Range(0, NextMinos.Length);
        Mino_image.sprite = NextMinos[minoNum];
        return num;
    }
}
