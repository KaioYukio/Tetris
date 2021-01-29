using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetro : MonoBehaviour
{
    public Vector3 rotationPoint;
    public Vector3[] initialBlockPos;
    public bool activeTetro;
    public string fatherName;
    public float timeCheckPrevious;

    // Start is called before the first frame update
    void Start()
    {

        activeTetro = true;
        fatherName = transform.parent.name;
        GameManager.instance.GameOverEvent += ResetTetro;
    }

    private void OnEnable()
    {
        GameManager.instance.MakeTetroActive(this);
        activeTetro = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!activeTetro)
        {
            CheckChildrens();
            return;
        }

        MakeTetroFall();
    }

    void CheckRow()
    {
        for (int i = GameManager.instance.height - 1; i >= 0; i--)
        {
            if (HasFullRow(i))
            {
                DeleteRow(i);
                RowDown(i);
            }
        }
    }

    bool HasFullRow(int i)
    {
        for (int j = 0; j < GameManager.instance.width; j++)
        {
            if (GameManager.instance.grid[j, i] == null)
            {
                return false;
            }
        }

        return true;
    }

    void DeleteRow(int i)
    {
        for (int j = 0; j < GameManager.instance.width; j++)
        {
            GameManager.instance.grid[j, i].gameObject.SetActive(false);
            GameManager.instance.grid[j, i] = null;
        }

        GameManager.instance.AddScore();
    }

    void RowDown(int i)
    {
        for (int y = i; y < GameManager.instance.height; y++)
        {
            for (int j = 0; j < GameManager.instance.width; j++)
            {
                if (GameManager.instance.grid[j, y] != null)
                {
                    GameManager.instance.grid[j, y - 1] = GameManager.instance.grid[j, y];
                    GameManager.instance.grid[j, y] = null;
                    GameManager.instance.grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }

    }

    public void AddToGrid()
    {
        foreach (Transform childrenTransform in transform)
        {
            int roundX = Mathf.RoundToInt(childrenTransform.transform.position.x);
            int roundY = Mathf.RoundToInt(childrenTransform.transform.position.y);

            GameManager.instance.grid[roundX, roundY] = childrenTransform;
        }
    }

    void MakeTetroFall()
    {
        if (Time.time >= GameManager.instance.previousTime + (GameManager.instance.downTime / (1 +(GameManager.instance.score /80))))
        {
            transform.position += new Vector3(0, -1, 0);

            if (!IsValidPosition())
            {
                if (IsGameOver())
                {
                    transform.position += new Vector3(0, 1, 0);
                    Debug.Log("GameOver!");
                    GameManager.instance.GameOver();
                    activeTetro = false;
                    return;
                }

                transform.position += new Vector3(0, 1, 0);
                AddToGrid();
                CheckRow();
                GameManager.instance.SpawnTetro();
                activeTetro = false;
            }

            GameManager.instance.previousTime = Time.time;
        }
    }

    void CheckChildrens()
    {

        if (Time.time >= timeCheckPrevious + 2)
        {
            for (int o = 0; o < transform.childCount; o++)
            {
                if (transform.GetChild(o).gameObject.activeSelf)
                {
                    timeCheckPrevious = Time.time;
                    Debug.Log("Children: " + o + "active");
                    return;
                }
            }

            ResetTetro();

            timeCheckPrevious = Time.time;
        }
    }


    public void ResetTetro()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);

        for (int o = 0; o < transform.childCount; o++)
        {
            transform.GetChild(o).gameObject.SetActive(true);
            transform.GetChild(o).transform.localPosition = initialBlockPos[o];
        }



        Debug.Log("Back to List");
        if (!GameObject.Find(fatherName).GetComponent<TetroList>().list.Contains(this.gameObject))
        {
            GameObject.Find(fatherName).GetComponent<TetroList>().list.Add(gameObject);
        }

        activeTetro = true;
        gameObject.SetActive(false);
    }


    public void MoveTetro(float direction)
    {
        if (direction > 0)
        {
            transform.position += new Vector3(1, 0, 0);
            if (!IsValidPosition())
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        }
        else if (direction < 0)
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!IsValidPosition())
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }
    }

    public void RotateTetro()
    {
        transform.RotateAround(transform.position, new Vector3(0, 0, 1), 90);

        if (!IsValidPosition())
        {
            transform.RotateAround(transform.position, new Vector3(0, 0, 1), -90);
        }
    }

    bool IsGameOver()
    {
        foreach (Transform childrenTransform in transform)
        {
            int roundY = Mathf.RoundToInt(childrenTransform.transform.position.y);

            if (roundY > 19)
            {
                return true;
            }
        }

        return false;
    }

    bool IsValidPosition()
    {
        foreach (Transform childrenTransform in transform)
        {
            int roundX = Mathf.RoundToInt(childrenTransform.transform.position.x);
            int roundY = Mathf.RoundToInt(childrenTransform.transform.position.y);

            if (roundX < 0 || roundX > GameManager.instance.maxBounds.x || roundY < 0)
            {
                return false;
            }

            if (GameManager.instance.grid[roundX, roundY] != null)
            {
                return false;
            }
        }

        return true;
    }

}
