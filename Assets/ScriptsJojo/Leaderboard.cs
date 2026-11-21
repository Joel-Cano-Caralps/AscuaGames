using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable] public class Entry { public string name; public int score; }
[System.Serializable] class Data { public List<Entry> entries = new(); }

public static class Leaderboard
{
    const int Max = 10;
    static string Path => System.IO.Path.Combine(Application.persistentDataPath, "leaderboard.json");

    public static List<Entry> Load()
    {
        if (!File.Exists(Path)) return new List<Entry>();
        return JsonUtility.FromJson<Data>(File.ReadAllText(Path)).entries;
    }

    public static void Add(string name, int score)
    {
        var list = Load();
        list.Add(new Entry { name = name, score = score });
        list = list.OrderByDescending(e => e.score).Take(Max).ToList();
        File.WriteAllText(Path, JsonUtility.ToJson(new Data { entries = list }));
    }

    public static bool QualifiesForTop(int score)
    {
        var list = Load();
        return list.Count < Max || score > list.Last().score;
    }

    public static void Resetjson()
    {
        File.Delete(Path);
    }
}