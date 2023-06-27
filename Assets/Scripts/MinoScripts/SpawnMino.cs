using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMino : MonoBehaviour
{
    public GameObject[] Minos;
    // Start is called before the first frame update
    void Start()
    {
        NewMino(Random.Range(0, Minos.Length));
    }

    public void NewMino(int i)
    {
        //Quaternion.identity‚Í–³‰ñ“]‚Å‚ ‚é‚±‚Æ‚ğ¦‚µ‚Ä‚¢‚éB
        Instantiate(Minos[i], transform.position, Quaternion.identity);
    }
}
