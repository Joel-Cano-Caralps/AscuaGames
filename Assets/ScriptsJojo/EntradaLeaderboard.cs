using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class EntradaLeaderboard : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI scoreFinal; // Score jugador final
    [SerializeField] GameObject PanelscoreFinal; // Score jugador final
    [SerializeField] GameObject LeaderboardGM; // Gameobject leaderboard
    public LeaderboardUI leaderboardUI; //Script para el archivo
    [SerializeField] GameController gameController; // Controlador juego
    [SerializeField] GameObject ButtonsRestart_Main; // Boton resetera y main menu
    [SerializeField] string playerName; // Name Player
    [SerializeField] PlayerName plName; // Name Player

    public bool inRank = false;


    private void Start()
    {
        leaderboardUI.Refresh();
        plName = FindAnyObjectByType<PlayerName>();
        playerName = plName.playerName;
        PanelscoreFinal.SetActive(false);
    }

    private void Update()
    {
        leaderboardUI.ResetJsonFile();
    }

    public IEnumerator ShowIfQualifies(int score)
    {
        ScoreFinal();
        scoreFinal.text = "Score: " + score;
        yield return new WaitForSeconds(5f);
        PanelscoreFinal.SetActive(false);
        score = gameController.getScore();        
        LeaderboardGM.SetActive(true);        
        ButtonsRestart_Main.SetActive(true);
        leaderboardUI.Refresh();
        ButtonsRestart_Main.SetActive(true);
        Time.timeScale = 0;
    }

    void Save()
    {
        Leaderboard.Add(playerName, gameController.getScore());        
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