using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] GameObject icons;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject Leaderboard;
    [SerializeField] Timer timer;
    [SerializeField] GameObject[] sprites;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        desactivateUI();
    }

    void desactivateUI()
    {
        if(timer.GetTimerStop())
        {
            icons.SetActive(false);
            scoreText.enabled = false;

            for(int i = 0; i < sprites.Length; i++)
            {
                sprites[i].SetActive(false);
            }
        }
    }
}
