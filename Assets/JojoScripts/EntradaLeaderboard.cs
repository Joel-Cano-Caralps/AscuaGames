using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EntradaLeaderboard : MonoBehaviour
{
    [SerializeField] GameObject panel;          // el panel que contiene el input y el botón
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] LeaderboardUI leaderboardUI;

    [SerializeField] int pendingScore;

    void Awake()
    {
        nameInput.characterLimit = 12;
        //panel.SetActive(false);
    }

    private void Update()
    {
        if(Keyboard.current.enterKey.wasPressedThisFrame/* && nameInput.text != null*/)
        {
            print("dsfsdf");
            Save();
        }
    }

    public void ShowIfQualifies(int score)
    {
        pendingScore = score;

        if (Leaderboard.QualifiesForTop(score))
        {
            nameInput.text = "";
            panel.SetActive(true);
            nameInput.ActivateInputField();
        }
        else
        {
            leaderboardUI.Refresh();
        }
    }

    void Save()
    {
        string playerName = nameInput.text.Trim();
        if (string.IsNullOrEmpty(playerName)) playerName = "Anónimo";

        Leaderboard.Add(playerName, pendingScore);
        panel.SetActive(false);
        leaderboardUI.Refresh();
    }
}