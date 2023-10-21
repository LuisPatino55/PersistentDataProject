using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainButtons;      //main menu
    public GameObject difficultyButtons; // difficulty menu
    public TextMeshProUGUI difficultyText;

    private void OnEnable()             // all ths stuff is pretty basic, so i won't get into it
    {
        mainButtons.SetActive(true);
        difficultyButtons.SetActive(false);
        
    }
    private void Start()
    {
        difficultyText.text = "Difficulty: " + Scene_Flow.Instance.currentDifficulty;
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
    public void DifficultyChoice(int difficulty)    // i can't remember why I used an int for difficulty here, but those are set when I declared the struct. 
    {
        Scene_Flow.Instance.currentDifficulty = (Difficulty)difficulty;
        mainButtons.SetActive(true);
        difficultyButtons.SetActive(false);
        difficultyText.text = "Difficulty: " + Scene_Flow.Instance.currentDifficulty;
    }
}
