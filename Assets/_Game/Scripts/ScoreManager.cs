using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using AmplifyShaderEditor;
using JetBrains.Annotations;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance { get; private set; }
    void Awake()
    {
        instance = this;
    }

    [SerializeField] TextMeshProUGUI scoreLabel;
    [SerializeField] TextMeshProUGUI timerLabel;
    int score;
    float timer = 60;
    bool timeOut = false;
    float fxTime = 0.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        timerLabel.text = (Mathf.Round(timer)).ToString();

        if (timer <= 0 && !timeOut)
        {
            timeOut = true;
            GameOver();
        }
    }

    public void IncrementScore(int scoreIncrement)
    {
        score += scoreIncrement;
        Mathf.Max(score, 99999);

        scoreLabel.text = score.ToString();
        while (scoreLabel.text.Length < 5)
        {
            scoreLabel.text = scoreLabel.text.Insert(0, "0");
        }
        scoreLabel.color = Color.purple;
        scoreLabel.DOColor(Color.white, fxTime);

    }

    public void GameOver()
    {
        Time.timeScale = 0;
    }
}
