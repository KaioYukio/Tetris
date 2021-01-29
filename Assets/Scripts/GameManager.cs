using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Tetro activeTetro;

    public List<TetroList> listTetro = new List<TetroList>();

    [Header("References")]
    public Transform mainMenu;
    public Transform howToPlay;
    public Transform pause;
    public Transform gameOver;

    [Space]
    [Header("Stats")]
    public int nextTetro;
    public float downTime;
    public float highSpeed;
    public float regularSpeed;
    public bool isBoosting;
    public float previousTime;
    public Vector2 maxBounds;
    public int height = 25;
    public int width = 9;

    public int highScore;
    public int score;

    public Transform[,] grid;

    public event Action ScoreEvent;
    public event Action HighScoreEvent;
    public event Action GameOverEvent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        grid = new Transform[width, height];
        HighScoreEvent?.Invoke();
    }

    // Update is called once per frame
    void Update()
    { 

    }

    public void RemoveTetroActive()
    {
        activeTetro = null;
    }

    public void MakeTetroActive(Tetro tetro)
    {
        activeTetro = tetro;
    }   

    public void StartGame()
    {
        for (int y = 0; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                grid[j, y] = null;
            }
        }

        score = 0;
        ScoreEvent?.Invoke();

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(mainMenu.DOLocalMoveY(17, 0.2f));
        mySequence.Append(mainMenu.DOLocalMoveY(-1672, 0.3f));


        int rand = UnityEngine.Random.Range(0, listTetro.Count);
        listTetro[rand].EnableTetro();
        RollNextTetro();
    }

    void RollNextTetro()
    {
        int rand = UnityEngine.Random.Range(0, listTetro.Count);

        nextTetro = rand;

        TetroContainer.instance.EnableTetro(nextTetro);
    }

    public void SpawnTetro()
    {
        listTetro[nextTetro].EnableTetro();
        RollNextTetro();
    }

    public void MoveEvent(float direction)
    {
        activeTetro.MoveTetro(direction);
    }

    public void RotateEvent()
    {
        activeTetro.RotateTetro();
    }

    public void BoostSpeed()
    {
        downTime = highSpeed / (1 + score/80);
        isBoosting = true;
    }
    public void StopBoostSpeed()
    {
        downTime = regularSpeed;
        isBoosting = false;
    }

    public void AddScore()
    {
        score += 10;
        ScoreEvent?.Invoke();
    }

    public void OpenHowToPlay()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(howToPlay.DOScale(new Vector2(1.6163f, 1.6163f), 0.2f));
        mySequence.Append(howToPlay.DOScale(new Vector2(1.36917f, 1.36917f), 0.3f));
    }

    public void CloseHowToPlay()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(howToPlay.DOScale(new Vector2(1.6163f, 1.6163f), 0.3f));
        mySequence.Append(howToPlay.DOScale(new Vector2(0f, 0), 0.2f));
    }

    public void OpenPause()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(pause.DOScale(new Vector2(1.2f, 1.2f), 0.2f));
        mySequence.Append(pause.DOScale(new Vector2(1, 1), 0.3f)).OnComplete(() => Time.timeScale = 0);
    }

    public void ClosePause()
    {
        Time.timeScale = 1;

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(pause.DOScale(new Vector2(1.2f, 1.2f), 0.2f));
        mySequence.Append(pause.DOScale(new Vector2(0, 0), 0.3f));
    }

    public void OpenGameOver()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(gameOver.DOScale(new Vector2(1.2f, 1.2f), 0.2f));
        mySequence.Append(gameOver.DOScale(new Vector2(1, 1), 0.3f));
    }

    public void CloseGameOver()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(gameOver.DOScale(new Vector2(1.2f, 1.2f), 0.2f));
        mySequence.Append(gameOver.DOScale(new Vector2(0, 0), 0.3f));
    }

    public void GameOver()
    {
        OpenGameOver();

        if (score > highScore)
        {         
            highScore = score;
            HighScoreEvent?.Invoke();
        }
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        for (int y = 0; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                grid[j, y] = null;
            }
        }

        GameOverEvent?.Invoke();
        CloseGameOver();
        ClosePause();
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(mainMenu.DOLocalMoveY(17, 0.2f));
        mySequence.Append(mainMenu.DOLocalMoveY(-66.8f, 0.3f)).OnComplete(() => GameOverEvent?.Invoke());
    }

}
