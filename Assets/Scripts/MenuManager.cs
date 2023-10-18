using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainButtons;
    public GameObject difficultyButtons;
    public TextMeshProUGUI difficultyText;

    private void OnEnable()
    {
        mainButtons.SetActive(true);
        difficultyButtons.SetActive(false);
        
    }
    private void Start()
    {
        difficultyText.text = "Difficulty: " + Scene_Flow.Instance.difficulty;
    }
    public void DifficultyMenuOn()
    {
        mainButtons.SetActive(false);
        difficultyButtons.SetActive(true);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }


    public void DifficultyChoice(int difficulty)
    {
        Scene_Flow.Instance.difficulty = difficulty;
        mainButtons.SetActive(true);
        difficultyButtons.SetActive(false);
        difficultyText.text = "Difficlty: " + Scene_Flow.Instance.difficulty;
    }
}
