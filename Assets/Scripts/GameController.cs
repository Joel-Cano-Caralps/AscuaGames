using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private int playerScore = 0;
    [SerializeField] private RandomEffect gameModGenerator;
    [SerializeField] private TextMeshProUGUI scoreText;

    void Start()
    {
        Score();
    }
    public void Score(int score = 0)
    {
        playerScore += score;
        scoreText.text = playerScore.ToString();
        gameModGenerator.StartModRoulette();
    }
}
