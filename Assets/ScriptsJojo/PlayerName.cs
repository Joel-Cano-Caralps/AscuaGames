using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerName : MonoBehaviour
{
    public string playerName;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] bool OnStart;
    [SerializeField] TMP_Text errorText;   // desactivado por defecto


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        errorText.enabled = false;
        nameInput.characterLimit = 8;
        //if (FindAnyObjectByType<PlayerName>())
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame && OnStart)
        {
            CogerNombre();
        }    
            
    }

    void CogerNombre()
    {
        string n = nameInput.text.Trim();
        if (string.IsNullOrEmpty(n)) n = "Noname";

        if (n != "Noname" && Leaderboard.NameExists(n))
        {
            errorText.text = "Ese nombre ya está en el ranking";
            errorText.enabled = true;
            return;
        }

        playerName = n;
        OnStart = false;
        SceneManager.LoadScene(1);
    }

    public void PlayGame()
    {
        OnStart = !OnStart;
    }

    public void VerificarName()
    {
        //if (playerName == )
        //{

        //}
    }
}
