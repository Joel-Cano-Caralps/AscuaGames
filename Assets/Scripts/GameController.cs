using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private int playerScore = 0;
    [SerializeField] private RandomEffect gameModGenerator;
    [SerializeField] TextMeshProUGUI scoreToBeat; // Score jugador final
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Timer timer;
    [SerializeField] private bool Finish;
    [SerializeField] private EntradaLeaderboard entradaLeaderboard;
    [SerializeField] int BeforeBall = -1;

    private void Awake()
    {
        ShowScoreToBeat();
    }
    void Start()
    {
        Finish = false;
        Score();
    }

    private void Update()
    {
        if(timer.GetTimerStop()){waitOnFinish();}

        if(Keyboard.current.rKey.wasPressedThisFrame)
        {
            ButtonRestart(true);
        }
    }

    public void Score(int score = 0)
    {
        playerScore += score;
        scoreText.text = playerScore.ToString();
        gameModGenerator.StartModRoulette();
    }

    public int getScore()
    {
        return playerScore;
    }

    public void waitOnFinish()
    {
        StartCoroutine(entradaLeaderboard.ShowIfQualifies(playerScore));
    }

    public bool getFinish(){return Finish;}
    public bool setFinish(bool fin){ Finish = fin; return Finish;}

    public void ButtonRestart(bool primera)
    {
        if(primera)
            SceneManager.LoadScene(1);
        else 
            SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public int setNumBall(int bb)
    {
        BeforeBall = bb;
        return BeforeBall;
    }
    public int GetNumBall()
    {
        return BeforeBall;
    }

    async void ShowScoreToBeat()
    {
        int min = await OnlineLeaderboard.GetScoreToEnterAsync();
        if (this == null) return;
        scoreToBeat.text = min.ToString();
    }
}
