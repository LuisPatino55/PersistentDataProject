using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct Scores        // struct for high scores list 
{
    public int score;
    public string name;
    public Difficulty difficulty;
}
public enum Difficulty      // enum for the difficulty settings 
{
    Easy = 0, 
    Medium = 1,
    Hard = 2,
}

public class Scene_Flow : MonoBehaviour
{
    public static Scene_Flow Instance;      // instance of the singleton 

    public Difficulty currentDifficulty = Difficulty.Easy;

    public List<Scores> highScores = new()
    {
    new Scores { score = 0, name = "" },
    };


    private void Awake()
    {
        if (Instance != null)               // code to create a singelton instance
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);      // don't destroy the object
    }
    public void RestartMenu()               // back to main menu
    {
        SceneManager.LoadScene(0);
    }

    public void SetHighScore(int newScore, string newName)  // adds a new high score to list 
    {
        int index = 0;                      // probably a better way to do this, but I'm a noob ;) 
                                            // it's a way to get the index number of where to insert the new entry on the list 
        foreach (Scores highscore in highScores)
        {
           if (newScore >= highscore.score) // this sets the new high score in correct sequential order on the list 
            {
                Scores newScoreEntry = new() { score = newScore, name = newName, difficulty = currentDifficulty }; // new stuct object created to add to the list 
                // debug msg verifies all info was passed correclty 
                Debug.Log("New entry created: " + newScoreEntry.name + "   Score: " + newScoreEntry.score + "  Difficulty Setting: " + newScoreEntry.difficulty + "  index: " + index);
                highScores.Insert(index, newScoreEntry);    // insert at proper index
                if (highScores.Count >= 6) highScores.RemoveAt(5);  // if list is bigger than 6 remove at index 5 (we start at 0)
                break;
            }
           index++;     // increases index if the high score in iteration is higher than the new addition
        }
    }
}
