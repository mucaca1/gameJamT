using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    private static GameData instance;

    public static GameData GetInstance()
    {
        if (instance == null)
            instance = new GameData();

        return instance;
    }

    private readonly string KEY = "high_score";
    private readonly char SCORE_SEPARATOR = ';';

    public static readonly int MAX_HIGHSCORES = 5;

    private HighScoreEntry[] highScoreEntries = new HighScoreEntry[MAX_HIGHSCORES];

    public GameData()
    {   
        Load();
    }

    private HighScoreEntry[] Deserialize(string scores)
    {
        HighScoreEntry[] entries = new HighScoreEntry[MAX_HIGHSCORES];

        string[] separated = scores.Split(SCORE_SEPARATOR);

        for (int i = 0; i < Mathf.Min(separated.Length, MAX_HIGHSCORES); i++)
        {   
            // 3 for name 1 for separator and at least 1 for one score
            if (separated[i].Length < 5)
                continue;
            string[] name_score = separated[i].Split(':');
            entries[i] = new HighScoreEntry(name_score[0], int.Parse(name_score[1]));
        }

        return entries;
    }

    private string Serialize(HighScoreEntry[] scores)
    {
        string serialized = "";

        for (int i = 0; i < scores.Length; i++)
        {
            serialized += scores[i].name + ":" + scores[i].score + SCORE_SEPARATOR;
        }

        return serialized;
    }

    public void Load()
    {
        highScoreEntries = Deserialize(PlayerPrefs.GetString(KEY));
    }

    public void Save()
    {
        PlayerPrefs.SetString(KEY, Serialize(highScoreEntries));
    }

    // If this score can be registered to the high score table
    public bool CanBeRegistered(int score)
    {
        for (int i = 0; i < highScoreEntries.Length; i++)
        {
            if (score > highScoreEntries[i].score || highScoreEntries[i] == null)
                return true;
        }

        return false;
    }

    // Returns if the score was added to table (if not the score was not high enough)
    public bool TryRegisterScore(string name, int score)
    {
        for (int i = 0; i < highScoreEntries.Length; i++)
        {
            if (score > highScoreEntries[i].score || highScoreEntries[i] == null) 
            {
                highScoreEntries[i] = new HighScoreEntry(name, score);
                return true;
            }
        }

        return false;
    }

    public HighScoreEntry[] GetHighScores()
    {
        return highScoreEntries;
    }
}
