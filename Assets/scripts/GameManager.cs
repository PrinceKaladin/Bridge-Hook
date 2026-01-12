using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager I { get; private set; }

    [Header("UI")]
    public Text scoreText;


    [Header("Score")]
    public int score = 0;

    const string SCORE_KEY = "score";
    const string BEST_KEY = "bestscore";

    public int BestScore { get; private set; }

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;

        // можно не делать DontDestroyOnLoad, если это только игровая сцена
        // DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        BestScore = PlayerPrefs.GetInt(BEST_KEY, 0);
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        if (PlayerPrefs.GetInt("sound", 1) == 1) {
            this.GetComponent<AudioSource>().Play();
        }
        score += amount;
        PlayerPrefs.SetInt(SCORE_KEY, score);

        if (score > BestScore)
        {
            BestScore = score;
            PlayerPrefs.SetInt(BEST_KEY, BestScore);
        }

        PlayerPrefs.Save();
        UpdateUI();
    }

    public void ResetScore()
    {
        score = 0;
        PlayerPrefs.SetInt(SCORE_KEY, score);
        PlayerPrefs.Save();
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText) scoreText.text = $"Score: {score}";

    }
}
