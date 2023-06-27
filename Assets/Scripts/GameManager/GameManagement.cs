using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class GameManagement : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int score;
    public int clearScore = 1500;

    public TextMeshProUGUI timerText;

    public float gameTime = 60f;
    int seconds;

    public GameObject gamePauseUI;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        TimeManagement();
    }

    private void Initialize()
    {
        score = 0;
    }

    public void TimeManagement()
    {
        gameTime -= Time.deltaTime;
        seconds = (int)gameTime;
        timerText.text = seconds.ToString();

        if(seconds == 0)
        {
            Debug.Log("TimeOut");
            GameOver();
        }
    }

    // ��񕪂����_�𑫂�
    public void AddScore()
    {
        score += 100;
        scoreText.text = "Score: " + score.ToString();

        Debug.Log(score); 

        if(score >= clearScore)
        {
            GameClear();
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameClear()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GamePause()
    {
        GamePauseToggle();
    }

    public void GamePauseToggle()
    {
        gamePauseUI.SetActive(!gamePauseUI.activeSelf);

        if (gamePauseUI.activeSelf)
        {
            Time.timeScale = 0f; //timeScale�̓^�C���̎ړx��\��
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

}
