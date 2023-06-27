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
        //Quaternion.identityは無回転であることを示している。
        Instantiate(Minos[i], transform.position, Quaternion.identity);
    }
}
