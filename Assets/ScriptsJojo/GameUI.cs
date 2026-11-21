using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] Image icons;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject Laderboard;
    [SerializeField] GameController gameController;
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
        if(GetComponent<Timer>().GetTimerStop())
        {
            icons.enabled = false;
            scoreText.enabled = false;
        }
    }

    void activateUI()
    {
        Laderboard.SetActive(true);
    }
}
