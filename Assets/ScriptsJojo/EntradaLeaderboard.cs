using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class EntradaLeaderboard : MonoBehaviour
{
    [SerializeField] GameObject panelInputNames; // El panel que contiene el input y el botón
    [SerializeField] TextMeshProUGUI scoreFinalInput; // Score que se muestra en lo del input
    [SerializeField] TextMeshProUGUI scoreFinalNoI; // Score que se muestra en lo del input
    [SerializeField] GameObject scoreText;  // Score del jugador final
    [SerializeField] GameObject LeaderboardGM; // Gameobject leaderboard
    [SerializeField] TMP_InputField nameInput; // El input donde escribe el jugador
    [SerializeField] LeaderboardUI leaderboardUI; //Script para el archivo
    [SerializeField] GameController gameController; // Controlador juego
    [SerializeField] GameObject ButtonsRes_Main; // Controlador juego

    public bool inRank = false;

    void Awake()
    {
        nameInput.characterLimit = 12;
        panelInputNames.SetActive(false);
        scoreText.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame && inRank)
        {
            LeaderboardGM.SetActive(true);
            print("Save");
            Save();
            ButtonsRes_Main.SetActive(true);
            panelInputNames.SetActive(false);
        }

        leaderboardUI.ResetJsonFile();
    }

    public void ShowIfQualifies(int score)
    {
        score = gameController.getScore();

        if (Leaderboard.QualifiesForTop(score))
        {
            inRank = true;
            nameInput.text = "";
            panelInputNames.SetActive(true);
            nameInput.ActivateInputField();
            scoreFinalInput.text = "Score: " + score.ToString();
        }
        else
        {
            scoreText.SetActive(true);
            scoreFinalNoI.text = "Score: " + score.ToString();
            
            leaderboardUI.Refresh();
            ButtonsRes_Main.SetActive(true);
        }

        
    }

    void Save()
    {
        string playerName = nameInput.text.Trim();
        if (string.IsNullOrEmpty(playerName)) playerName = "Noname";

        Leaderboard.Add(playerName, gameController.getScore());
        
        leaderboardUI.Refresh();
        inRank = false;
    }
}