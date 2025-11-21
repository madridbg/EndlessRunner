using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int score;
    public static GameManager inst;

    public TextMeshProUGUI scoreText;


    public void IncrementScore()
    {
        score++;
        scoreText.text = "Score: " + score;
    }

    private void Awake()
    {
        inst = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
