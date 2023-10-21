using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

public class DataManager : MonoBehaviour
{
    private string _dataPath;
    private string _xmlDifficulty;
    private string _xmlScores;

    void Awake()
    {
        _dataPath = Application.persistentDataPath + "/Player_Data/";
        Debug.Log(_dataPath);
        _xmlDifficulty = _dataPath + "Difficulty.xml";
        _xmlScores = _dataPath + "HighScoreList.xml";
    }
    private void FilesystemInfo()
    {
        Debug.LogFormat("Path separator character: {0}", Path.PathSeparator);
        Debug.LogFormat("Directory separator character: {0}", Path.DirectorySeparatorChar);
        Debug.LogFormat("Current directory: {0}", Directory.GetCurrentDirectory());
        Debug.LogFormat("Temporary path: {0}", Path.GetTempPath());
    }
    public void NewDirectory()
    {
        if (Directory.Exists(_dataPath))
        {
            Debug.Log("Directory already exists...");
            return;
        }
        Directory.CreateDirectory(_dataPath);
        Debug.Log("New directory created!");
    }
    public void DeleteDirectory()
    {
        if (!Directory.Exists(_dataPath))
        {
            Debug.Log("Directory doesn't exist or has already been deleted...");
            return;
        }
        Directory.Delete(_dataPath, true);
        Debug.Log("Directory sucessfully deleted!");
    }
    public void WriteToXML(string filename)
    {
        if (!File.Exists(filename))
        {
            FileStream xmlStream = File.Create(filename);
            XmlWriter xmlWriter = XmlWriter.Create(xmlStream);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("level_progress");
            for (int i = 1; i < 5; i++)
            {
                xmlWriter.WriteElementString("level", "Level-" + i);
            }
            xmlWriter.WriteEndElement();
            xmlWriter.Close();
            xmlStream.Close();
        }
    }
    public void SerializeXML()
    {
        var xmlSerializer = new XmlSerializer(typeof(List<Scores>));
        using FileStream stream = File.Create(_xmlScores);
        xmlSerializer.Serialize(stream, Scene_Flow.Instance.highScores);
    }
    public void DeserializeXML(string file)
    {
        if (File.Exists(file))
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Scores>));
            using FileStream stream = File.OpenRead(file);
            var weapons = (List<Scores>)xmlSerializer.Deserialize(stream);
            foreach (var weapon in weapons)
            {
                Debug.LogFormat("Weapon: {0} - Damage: {1}");
            }
        }
    }

}
