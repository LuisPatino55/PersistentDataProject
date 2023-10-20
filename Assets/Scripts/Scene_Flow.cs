using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct Scores
{
    public int score;
    public string name;
    public Difficulty difficulty;
}
public enum Difficulty
{
    Easy = 0, 
    Medium = 1,
    Hard = 2,
}

public class Scene_Flow : MonoBehaviour
{
    public static Scene_Flow Instance;      // for singelton instance

    public Difficulty currentDifficulty = Difficulty.Easy;

    public List<Scores> highScores = new()
    {
    new Scores { score = 0, name = "" },
    };


    private void Awake()
    {
        if (Instance != null)               // singleton pattern
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void RestartMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SetHighScore(int newScore, string newName)
    {
        int index = 0;

        foreach (Scores highscore in highScores)
        {
           if (newScore >= highscore.score)
            {
                Scores newScoreEntry = new() { score = newScore, name = newName, difficulty = currentDifficulty };
                Debug.Log("New entry created: " + newScoreEntry.name + "   Score: " + newScoreEntry.score + "  Difficulty Setting: " + newScoreEntry.difficulty + "  index: " + index);
                highScores.Insert(index, newScoreEntry);
                if (highScores.Count >= 6) highScores.RemoveAt(5);
                break;
            }
           index++;
        }
    }
}
