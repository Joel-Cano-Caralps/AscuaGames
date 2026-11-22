using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private int playerScore = 0;
    [SerializeField] private RandomEffect gameModGenerator;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Timer timer;
    [SerializeField] private bool Finish;
    [SerializeField] private EntradaLeaderboard entradaLeaderboard;


    void Start()
    {
        Finish = false;
        Score();
    }

    private void Update()
    {
        if(timer.GetTimerStop()){StartCoroutine(waitOnFinish());}
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

    IEnumerator waitOnFinish()
    {
        yield return new WaitForSeconds(5f);
        Finish = true;
        entradaLeaderboard.ShowIfQualifies(playerScore);
        Time.timeScale = 0;

    }

    public bool getFinish(){return Finish;}

    public void ButtonRestart(bool primera)
    {
        if(primera)
            SceneManager.LoadScene(1);
        else 
            SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

}
