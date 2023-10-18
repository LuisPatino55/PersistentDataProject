using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Scene_Flow : MonoBehaviour
{
    public static Scene_Flow Instance;

    public int difficulty = 0;

    public Dictionary<int, string> highScores = new();

    private void Awake()
    {
        if (Instance != null)
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
    public void SetHighScore(int score, string name)
    {
        if (highScores.Count < 5) EnterScore();
        else
        {
            foreach (int key in highScores.Keys)
            {
                if (key <= score)
                {
                    
                }
            }
        }


    }
    private void EnterScore()
    {



    }

}
