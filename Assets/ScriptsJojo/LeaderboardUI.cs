using System.Linq;
using TMPro;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] GameObject PrefabJugador;
    [SerializeField] TextMeshProUGUI ScoreASuperar;
    public int lastRank;

    void OnEnable()
    {
        Refresh();
    }

    public async void Refresh()
    {
        var entries = await OnlineLeaderboard.GetTopAsync();
        if (this == null) return;   // por si el objeto se destruyó mientras esperaba

        foreach (Transform child in container) Destroy(child.gameObject);

        for (int i = 0; i < entries.Count; i++)
        {
            var row = Instantiate(PrefabJugador, container);
            var texts = row.GetComponentsInChildren<TMP_Text>();
            texts[0].text = (i + 1).ToString();
            texts[1].text = entries[i].name;
            texts[2].text = entries[i].score.ToString();
        }

        lastRank = entries.Count == 0 ? 0 : entries.Last().score;
        if (ScoreASuperar != null) ScoreASuperar.text = lastRank.ToString();
    }
}