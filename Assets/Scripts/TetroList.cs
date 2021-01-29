using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetroList : MonoBehaviour
{
    public List<GameObject> list;
    public Transform spawnPos;
    public Vector3 offSet;


    public void EnableTetro()
    {
        list[list.Count - 1].SetActive(true);
        list[list.Count - 1].transform.position = spawnPos.position + offSet;
        list.Remove(list[list.Count - 1]);
    }


}
