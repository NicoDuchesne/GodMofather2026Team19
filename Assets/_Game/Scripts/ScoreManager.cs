using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance { get; private set; }
    void Awake()
    {
        instance = this;
    }

    [SerializeField] TextMeshProUGUI scoreLabel;
    [SerializeField] TextMeshProUGUI timerLabel;
    [SerializeField] TextMeshProUGUI ballLabel;
    [SerializeField] public GameObject ballList;
    
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
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
        
        timer -= Time.deltaTime;
        
        float rounded = Mathf.Round(timer);
        float minute = Mathf.Floor(rounded / 60);
        float second = rounded % 60;
        
        string minuteStr = minute.ToString("00");
        string secondStr = second.ToString("00");
        timerLabel.text = minuteStr + ":" + secondStr;
            
        //timerLabel.text = (Mathf.Round(timer)).ToString();

        if (timer <= 0 && !timeOut)
        {
            timeOut = true;
            GameOver();
        }

        if (ballList.transform.childCount == 0)
        {
            GameOver();
        }

        ballLabel.text = ballList.transform.childCount.ToString();
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
