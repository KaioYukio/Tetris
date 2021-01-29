using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetroContainer : MonoBehaviour
{
    public static TetroContainer instance;

    public List<GameObject> tetros;

    private void Awake()
    {
        instance = this;
    }

    public void EnableTetro(int index)
    {
        for (int i = 0; i < tetros.Count; i++)
        {
            if (i == index)
            {
                tetros[i].SetActive(true);
            }
            else
            {
                tetros[i].SetActive(false);
            }
        }
    }
}
