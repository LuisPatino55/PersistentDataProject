using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;


public class DataManager : MonoBehaviour
{
    private string _dataPath;
    private string _xmlScores;

    void Awake()
    {
        _dataPath = Application.persistentDataPath + "/Player_Data/";
        _xmlScores = _dataPath + "HighScoreList.xml";
        Debug.Log(_dataPath);
    }
    private void Start()
    {
        if (Directory.Exists(_dataPath)) LoadData(_xmlScores);
        else NewDirectory();
    }

    private void NewDirectory()              // creates save directory
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
        xmlSerializer.Serialize(stream, Scene_Flow.Instance.highScores);
        Debug.Log("Game data saved!");
    }
    private void DeserializeXML(string file)         // loads data to high score list
    {
        if (File.Exists(file))
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Scores>));
            using FileStream stream = File.OpenRead(file);
            var Scores = (List<Scores>)xmlSerializer.Deserialize(stream);
            foreach (var score in Scores)
            {
                Debug.LogFormat("High Score: {0}   by: {1}   difficulty: {2}", score.score, score.name, score.difficulty);  // create a list item...
            }
            Debug.Log("Game data loaded!");
        }   
    }

    public void SaveData()
    {
        SerializeXML(_xmlScores);
    }
}
