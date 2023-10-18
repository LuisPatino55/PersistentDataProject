using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Scene_Flow : MonoBehaviour
{
    public static Scene_Flow Instance;

    public int difficulty = 1;


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

}
