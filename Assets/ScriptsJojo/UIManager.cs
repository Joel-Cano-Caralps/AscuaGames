using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject Leaderboard;
    public GameObject InputRank; // Donde el jugador pone el nombre
    public GameObject Scoretext;
    EntradaLeaderboard el;
    public GameObject butonAtras;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Leaderboard.SetActive(false);
        InputRank.SetActive(false);
        Scoretext.SetActive(false);
        el = GetComponent<EntradaLeaderboard>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FinishButton()
    {
        el.ShowIfQualifies(el.pendingScore);
    }

    public void RestartGame()
    {
        Leaderboard.SetActive(false);
        InputRank.SetActive(false);
        Scoretext.SetActive(false);
        el.inRank = false;
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        print("Exit");
        Application.Quit();
    }
}
