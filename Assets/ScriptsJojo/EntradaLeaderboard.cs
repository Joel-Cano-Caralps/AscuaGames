using System.Collections;
using TMPro;
using UnityEngine;

public class EntradaLeaderboard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreFinal;   // Score jugador final
    [SerializeField] GameObject PanelscoreFinal;   // Panel del score final
    [SerializeField] GameObject LeaderboardGM;     // GameObject del leaderboard
    public LeaderboardUI leaderboardUI;            // Script de la UI del ranking
    [SerializeField] GameController gameController;
    [SerializeField] GameObject ButtonsRestart_Main;

    public bool inRank = false;

    void Start()
    {
        PanelscoreFinal.SetActive(false);
    }

    public IEnumerator ShowIfQualifies(int score)
    {
        ScoreFinal();
        scoreFinal.text = "Score: " + score;
        yield return new WaitForSeconds(5f);

        PanelscoreFinal.SetActive(false);
        LeaderboardGM.SetActive(true);
        ButtonsRestart_Main.SetActive(true);
        leaderboardUI.Refresh();
        Time.timeScale = 0;
    }

    async void Save()
    {
        await OnlineLeaderboard.SubmitAsync(gameController.getScore());
        leaderboardUI.Refresh();
    }

    public void ScoreFinal()
    {
        if (!gameController.getFinish())
        {
            PanelscoreFinal.SetActive(true);
            Save();
            gameController.setFinish(true);
        }
    }
}