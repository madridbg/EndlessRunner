using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int scoreCoins;
    public static GameManager inst;
    public TextMeshProUGUI scoreText;
    private PlayerMovement pm;
    public AudioClip milestoneSound;
    private AudioSource gameManagerAudio;
    private int lastMilestoneScore = 0;

    public bool isGameActive;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    private float score;
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] PlayerMovement playerMovement;

    void Start()
    {
        isGameActive = true;
        pm = GameObject.Find("Player").GetComponent<PlayerMovement>();
        gameManagerAudio = GetComponent<AudioSource>();


        score = 0;
        lastMilestoneScore = 0;
    }
    void Update()
    {
        if (isGameActive)
        {
            score += Time.deltaTime * 5;

            //update le score de lecran
            scoreText.text = "Score: " + Mathf.FloorToInt(score);

            CheckMilestone();
        }

    }


    public void IncrementScore()
    {
        scoreCoins++;
        coinsText.text = "Pièces: " + scoreCoins;
        playerMovement.speed += playerMovement.speedIncreasePerPoint;
    }

    private void Awake()
    {
        inst = this;
    }

    void CheckMilestone()
    {
        int currentScoreInt = Mathf.FloorToInt(score);

        int currentMilestone = (currentScoreInt / 100) * 100;

        if (currentMilestone > lastMilestoneScore && currentMilestone >= 100)
        {
            if (gameManagerAudio != null && milestoneSound != null)
            {
                gameManagerAudio.PlayOneShot(milestoneSound);
            }

            lastMilestoneScore = currentMilestone;

        }
    }

    public void GameOver()
    {
        isGameActive = false;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
