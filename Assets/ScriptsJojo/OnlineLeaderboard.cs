using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;

public static class OnlineLeaderboard
{
    const string Id = "ranking";   // el mismo ID que en el Dashboard

    static Task<bool> signInTask;  // una sola tarea de login compartida

    // Limpia el estado al empezar el Play (por si tienes desactivado Reload Domain)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics() => signInTask = null;

    static Task<bool> EnsureSignedIn() => signInTask ??= DoSignIn();

    static async Task<bool> DoSignIn()
    {
        try
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
                await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e);
            signInTask = null;   // permite reintentar
            return false;
        }
    }

    public static async Task SetNameAsync(string playerName)
    {
        if (!await EnsureSignedIn()) return;
        try
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
        }
        catch (System.Exception e) { Debug.LogWarning(e); }
    }

    public static async Task SubmitAsync(int score)
    {
        if (!await EnsureSignedIn()) return;
        try
        {
            await LeaderboardsService.Instance.AddPlayerScoreAsync(Id, score);
        }
        catch (System.Exception e) { Debug.LogWarning(e); }
    }

    public static async Task<List<Entry>> GetTopAsync()
    {
        if (!await EnsureSignedIn()) return new List<Entry>();
        try
        {
            var r = await LeaderboardsService.Instance.GetScoresAsync(Id, new GetScoresOptions { Limit = 10 });
            return r.Results.Select(x => new Entry
            {
                name = x.PlayerName.Split('#')[0],   // UGS añade #1234 al nombre
                score = (int)x.Score
            }).ToList();
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e);
            return new List<Entry>();
        }
    }

    public static async Task<int> GetScoreToEnterAsync()
    {
        var top = await GetTopAsync();
        if (top.Count < 10) return 0;
        return top.Last().score + 1;
    }
}