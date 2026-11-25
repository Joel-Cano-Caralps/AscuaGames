using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;

public static class OnlineLeaderboard
{
    const string Id = "ranking";   // el mismo ID del Dashboard
    static bool ready;

    public static async Task<bool> InitAsync(string playerName)
    {
        if (ready) return true;
        try
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
                await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

            await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
            ready = true;
        }
        catch (System.Exception e) { Debug.LogWarning(e); }
        return ready;
    }

    public static async Task SubmitAsync(int score)
    {
        if (!ready) return;
        try { await LeaderboardsService.Instance.AddPlayerScoreAsync(Id, score); }
        catch (System.Exception e) { Debug.LogWarning(e); }
    }

    public static async Task<List<Entry>> GetTopAsync()
    {
        if (!ready) return new List<Entry>();
        try
        {
            var r = await LeaderboardsService.Instance.GetScoresAsync(Id, new GetScoresOptions { Limit = 10 });
            return r.Results.Select(x => new Entry
            {
                name = x.PlayerName.Split('#')[0],   // UGS añade #1234 al nombre
                score = (int)x.Score
            }).ToList();
        }
        catch (System.Exception e) { Debug.LogWarning(e); return new List<Entry>(); }
    }
}