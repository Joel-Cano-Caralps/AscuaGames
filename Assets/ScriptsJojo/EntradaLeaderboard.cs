using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class EntradaLeaderboard : MonoBehaviour
{
    [SerializeField] GameObject panelInputNames; // el panel que contiene el input y el botón
    [SerializeField] GameObject panelScore; 
    [SerializeField] GameObject LeaderboardGM; 
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] LeaderboardUI leaderboardUI;

    [SerializeField] public int pendingScore;

    public bool inRank = false;

    void Awake()
    {
        nameInput.characterLimit = 12;
        panelInputNames.SetActive(false);
        panelScore.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame && inRank)
        {
            LeaderboardGM.SetActive(true);
            print("Save");
            Save();
        }

        leaderboardUI.ResetJsonFile();
    }

    public void ShowIfQualifies(int score)
    {
        pendingScore = score;

        if (Leaderboard.QualifiesForTop(score))
        {
            inRank = true;
            nameInput.text = "";
            panelInputNames.SetActive(true);
            nameInput.ActivateInputField();
        }
        else
        {
            leaderboardUI.Refresh();
        }
        panelScore.SetActive(true);
    }

    void Save()
    {
        string playerName = nameInput.text.Trim();
        if (string.IsNullOrEmpty(playerName)) playerName = "Noname";

        Leaderboard.Add(playerName, pendingScore);
        panelInputNames.SetActive(false);
        leaderboardUI.Refresh();
    }
}