using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] GameObject PrefabJugador;

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
    }

    void OnEnable()
    {
        Refresh();
    }
}