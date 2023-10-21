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
        NewDirectory();
    }

    public void NewDirectory()              // creates save directory if it doesn't exist
    {
        if (Directory.Exists(_dataPath))
        {
            Debug.Log("Save directory already exists...");
            LoadData(_xmlScores);         // calls the load data method
        }
        else
        {
            Directory.CreateDirectory(_dataPath);
            Debug.Log("New save directory created!");
            NewDirectory();
        }
    }

    public void DeleteDirectory()           // should you ever need to delete the directory
    {
        if (!Directory.Exists(_dataPath))
        {
            Debug.Log("Directory doesn't exist or has already been deleted...");
            return;
        }
        Directory.Delete(_dataPath, true);
        Debug.Log("Directory sucessfully deleted!");
    }
    private void LoadData(string filename)
    {
        if (File.Exists(filename)) DeserializeXML(filename);   //load data if save file exists

        else                                                   // otherwise make new file
        {
            FileStream xmlStream = File.Create(filename);
            XmlWriter xmlWriter = XmlWriter.Create(xmlStream);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Game_Settings_And_High_Score");
            xmlWriter.WriteEndElement();
            xmlWriter.Close();
            xmlStream.Close();
            Debug.Log("New game save file created ");
            SerializeXML(filename);                        // then save default data
        }
    }
    public void SerializeXML(string filename)       // saves high score list to XML
    {
        var xmlSerializer = new XmlSerializer(typeof(List<Scores>));
        using FileStream stream = File.Create(filename);
        xmlSerializer.Serialize(stream, Scene_Flow.Instance.highScores);
        Debug.Log("Game data saved!");
    }
    public void DeserializeXML(string file)         // loads data to high score list
    {
        if (File.Exists(file))
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Scores>));
            using FileStream stream = File.OpenRead(file);
            var Scores = (List<Scores>)xmlSerializer.Deserialize(stream);
            foreach (var score in Scores)
            {
                Debug.LogFormat("Weapon: {0} - Damage: {1}");  // create a list item...
            }
            Debug.Log("Game data loaded!");
        }   
    }
}
