using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerName : MonoBehaviour
{
    public static string Name { get; private set; } = "Noname";

    [SerializeField] TMP_InputField nameInput;
    [SerializeField] bool OnStart;

    void Start()
    {
        nameInput.characterLimit = 8;
    }

    void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame && OnStart)
            CogerNombre();
    }

    async void CogerNombre()
    {
        string n = nameInput.text.Trim();
        if (string.IsNullOrEmpty(n)) n = "Noname";

        Name = n;
        OnStart = false;

        await OnlineLeaderboard.SetNameAsync(Name);
        SceneManager.LoadScene(1);
    }

    public void PlayGame()
    {
        OnStart = !OnStart;
    }
}