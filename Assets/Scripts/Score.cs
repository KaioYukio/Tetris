using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;
    public Text highScoreText;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.ScoreEvent += UpdateScoreUI;
        GameManager.instance.HighScoreEvent += UpdateHighScoreUI;
    }

    void UpdateScoreUI()
    {
        if (scoreText == null)
            return;
        scoreText.text = "Score: " +  GameManager.instance.score.ToString();
    }

    void UpdateHighScoreUI()
    {
        if (highScoreText == null)
            return;
        highScoreText.text = "HIGHSCORE \n" +  GameManager.instance.highScore.ToString();
    }
}
