using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] GameObject PrefabJugador;
    [SerializeField] TextMeshProUGUI ScoreASuperar;
    public int lastRank;

    private void Start()
    {
        if (ScoreASuperar != null) ScoreASuperar = null;
    }

    private void Update()
    {
        ResetJsonFile();
    }

    public void Refresh()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        var entries = Leaderboard.Load();
        for (int i = 0; i < entries.Count; i++)
        {
            var row = Instantiate(PrefabJugador, container);
            var texts = row.GetComponentsInChildren<TMP_Text>();
            texts[0].text = (i + 1).ToString();
            texts[1].text = entries[i].name;
            texts[2].text = entries[i].score.ToString();
        }
        TakeLast();
        
        if(ScoreASuperar != null) ScoreASuperar.text = lastRank.ToString();

    }

    void OnEnable()
    {
        Refresh();
    }

    public void ResetJsonFile()
    {
        if (Keyboard.current.deleteKey.wasPressedThisFrame)
        {
            print("Reset Json");
            Leaderboard.Resetjson();
            Refresh();
        }
    }

    public void TakeLast()
    {
        var list = Leaderboard.Load();
        lastRank = list.Count == 0 ? 0 : list.Last().score;
    }
}