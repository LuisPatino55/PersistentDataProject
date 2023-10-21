using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MainManager : MonoBehaviour
{
    [Header("                                         Game Set-up  ")]
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    [Header("                            In Game Score & Top Score ")]
    public Text ScoreText;
    public Text TopScoreText;
    [Header("                                High Score List Items ")]
    public GameObject HighScoreTitle;
    public TextMeshProUGUI HighScoreText;
    public TextMeshProUGUI DifficultyText;

    [Header("                   High Score Input Text & Input Field ")]
    public GameObject highScoreEntryBox;
    public TMP_InputField highScoreInput;
    [Space(20)]
    public GameObject GameOverText;
    private float ballForce = 2f;
    [Space(20)]
    public int m_Points;

    public bool m_Started = false; 
    public bool m_GameOver = false;
    private bool m_ResetKey = false;

    private void OnEnable()                     // this makes sure we reload the level every time we ewnter the scene
    {
        const float step = 0.6f;                // this was project code, I learned a bit from this here!
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }
    private void Start()
    {
        if (Scene_Flow.Instance.currentDifficulty == Difficulty.Hard) ballForce *= 3;   // speeds up ball for hard setting
        PrintHighScores();
        HighScoreTitle.SetActive(true);
    }
    private void Update()
    {
        if (!m_Started)                             // If round has not started
        {
            if (Input.GetKeyDown(KeyCode.Space))    // space bar sets ball in motion
            {
                HighScoreTitle.SetActive(false);    // hide High Score list
                
                m_Started = true;                   // disables spacebar action

                                                    // Ball stuff (i didn't write this piece)
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new(randomDirection, 1, 0);
                forceDir.Normalize();
                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * ballForce, ForceMode.VelocityChange); 
            }
        }
        else if (m_GameOver && m_ResetKey)          // if game is over (and high score field is not enabled), space bar will take you back to menu
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Scene_Flow.Instance.RestartMenu();  // Back to menu scene
            }
        }
    }
    void AddPoint(int point)                        // points
    {
        m_Points += point;
        ScoreText.text = $"Score: {m_Points}";
    }
    public void GameOver()                          // if ball hits bottom border
    {
        m_GameOver = true;

        HighScoreTitle.SetActive(true);             // shows high score list then checks if current score is higher that the lowest high score on list 
                                                    // Count -1 because index starts at 0
        if (m_Points > Scene_Flow.Instance.highScores[Scene_Flow.Instance.highScores.Count - 1].score) highScoreEntryBox.SetActive(true);
        
        else GameReset();                           // if not a new high-score
    }
    public void RecordScore()                       // if new high score, send input field string and current points to Scene Flow Singleton
    {
        string name = highScoreInput.text;
        Scene_Flow.Instance.SetHighScore(m_Points, name);
        GameReset();                            
    }
    private void GameReset()                        // enables game over text & space bar to main menu, calls to refresh high scores list 
    {
        highScoreEntryBox.SetActive(false);         // disables new high entry box 
        m_ResetKey = true;
        GameOverText.SetActive(true);
        PrintHighScores();
    }
    private void PrintHighScores()                  // refresh high scrore list 
    {
        HighScoreText.text = "";                    // these clean out old text 
        DifficultyText.text = "";

        TopScoreText.text = "Top Score: " + Scene_Flow.Instance.highScores[0].score;    // Prints top score to game box

        foreach (Scores entry in Scene_Flow.Instance.highScores)    // iterates through high score list if list > 0
        {
            if (entry.score > 0)
            {
                HighScoreText.text += entry.score + "               " + entry.name + "\n\n";        // prints stats for each struct in the list
                DifficultyText.text += "<color=blue>" + entry.difficulty + "</color>\n\n";
            }
        }
    }
}
