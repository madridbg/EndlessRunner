using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int scoreCoins;
    public static GameManager inst;
    public TextMeshProUGUI scoreText;
    private PlayerMovement pm;
    public AudioClip milestoneSound;
    private AudioSource gameManagerAudio;
    private int lastMilestoneScore = 0;


    private float score;
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] PlayerMovement playerMovement;

    void Start()
    {
        pm = GameObject.Find("Player").GetComponent<PlayerMovement>();
        gameManagerAudio = GetComponent<AudioSource>();


        score = 0;
        lastMilestoneScore = 0;
    }
    void Update()
    {
        if (!pm.gameOver)
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
        coinsText.text = "Score: " + scoreCoins;
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
}
