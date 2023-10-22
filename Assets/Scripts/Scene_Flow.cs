using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Xml.Serialization;

[Serializable]
public struct Scores        // struct for high scores list 
{
    public int score;
    public string name;
    public Difficulty difficulty;
}
[Serializable]
public enum Difficulty      // enum for the difficulty settings 
{
    Easy = 0, 
    Medium = 1,
    Hard = 2,
}

public class Scene_Flow : MonoBehaviour
{
    public static Scene_Flow Instance;      // instance of the singleton 

    public MenuManager manager;

    public Difficulty currentDifficulty = Difficulty.Easy; // default

    public List<Scores> highScores = new();

    private string _dataPath;               // save data path
    private string _xmlScores;              // save file name

    private void Awake()
    {
        if (Instance != null)               // code to create a singelton instance
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);      // don't destroy the object
        
        _dataPath = Application.persistentDataPath + "/Player_Data/";   // load save path & file
        _xmlScores = _dataPath + "HighScoreList.xml";
        Debug.Log(_dataPath);
    }

    private void Start()
    {
        if (Directory.Exists(_dataPath)) LoadData(_xmlScores);
        else NewDirectory();
        
        if (PlayerPrefs.HasKey("CurrentDifficulty")) // Load last difficutly setting
        {
            currentDifficulty = (Difficulty)PlayerPrefs.GetInt("CurrentDifficulty");
            Debug.Log("Saved difficulty loaded: " + currentDifficulty);
            manager.DifficultyUI();
        }
    }

    private void NewDirectory()              // creates save directory as needed
    {
        Directory.CreateDirectory(_dataPath);
        Debug.Log("New save directory created!");
        LoadData(_xmlScores);
    }

    private void LoadData(string filename)
    {
        if (File.Exists(filename))
        {
            Debug.Log("Save file found, loading...");
            DeserializeXML(filename);   //load data if save file exists
        }
        else
        {
            Debug.Log("New xml save file created!");
            SerializeXML(filename);     // otherwise create a new xml file
        }
    }
    private void SerializeXML(string filename)       // overwrites new high score list to XML file (creates a file if none exists)
    {
        var xmlSerializer = new XmlSerializer(typeof(List<Scores>));
        using FileStream stream = File.Create(filename);
        xmlSerializer.Serialize(stream, Instance.highScores);
        Debug.Log("Game data saved!");
    }
    private void DeserializeXML(string file)         // loads data to high score list
    {
        if (File.Exists(file))
        {
            highScores.Clear();
            var xmlSerializer = new XmlSerializer(typeof(List<Scores>));
            using FileStream stream = File.OpenRead(file);
            var Scores = (List<Scores>)xmlSerializer.Deserialize(stream);
            foreach (var score in Scores)
            {
                highScores.Add(score);
                Debug.LogFormat("High Score: {0}   by: {1}   difficulty: {2}", score.score, score.name, score.difficulty);  // create a list item...
            }
            Debug.Log("Game data loaded!");
            Debug.Log("High Scores List Count: " + highScores.Count);
        }
    }

    public void RestartMenu()               // back to main menu
    {
        SceneManager.LoadScene(0);
        SerializeXML(_xmlScores);
    }
    public void SaveDifficulty(int difficulty)            // Save the current difficulty to PlayerPrefs
    {
        currentDifficulty = (Difficulty)difficulty;
        PlayerPrefs.SetInt("CurrentDifficulty", difficulty);
        PlayerPrefs.Save();
        Debug.Log("Difficulty set & saved: " + currentDifficulty);
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
