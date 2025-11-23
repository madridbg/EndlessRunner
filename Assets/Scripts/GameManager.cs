using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    int score;
    public static GameManager inst;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] PlayerMovement playerMovement;


    public void IncrementScore()
    {
        score++;
        scoreText.text = "Score: " + score;
        playerMovement.speed += playerMovement.speedIncreasePerPoint;
    }

    private void Awake()
    {
        inst = this;
    }
}
