using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public TextMeshProUGUI HighScoreText;
    public TextMeshProUGUI DifficultyText;
    public GameObject GameOverText;
    public GameObject HighScoreTitle;
    public GameObject highScoreEntryBox;
    public TMP_InputField highScoreInput;

    private float ballForce = 2f;

    public int m_Points;

    public bool m_Started = false;
    public bool m_GameOver = false;
    private bool m_ResetKey = false;

    private void OnEnable()
    {
        const float step = 0.6f;
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
        if (Scene_Flow.Instance.currentDifficulty == Difficulty.Hard) ballForce *= 3;
        PrintHighScores();
        HighScoreTitle.SetActive(true);
    }
    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HighScoreTitle.SetActive(false);
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * ballForce, ForceMode.VelocityChange); 
            }
        }
        else if (m_GameOver && m_ResetKey)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Scene_Flow.Instance.RestartMenu();
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score: {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;

        HighScoreTitle.SetActive(true);

        if (m_Points > Scene_Flow.Instance.highScores[Scene_Flow.Instance.highScores.Count - 1].score) highScoreEntryBox.SetActive(true);
        
        else GameReset();
    }
    public void RecordScore()
    {
        string name = highScoreInput.text;
        Scene_Flow.Instance.SetHighScore(m_Points, name);
        GameReset();
        PrintHighScores();
    }
    private void GameReset()
    {
        highScoreEntryBox.SetActive(false);
        m_ResetKey = true;
        GameOverText.SetActive(true);
        PrintHighScores();
    }
    private void PrintHighScores()
    {
        HighScoreText.text = "";
        DifficultyText.text = "";
        foreach (Scores entry in Scene_Flow.Instance.highScores)
        {
            if (entry.score > 0)
            {
                HighScoreText.text += entry.score + "               " + entry.name + "\n\n";
                DifficultyText.text += "<color=blue>" + entry.difficulty + "</color>\n\n";
            }
                

        }
    }
}
