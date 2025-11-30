using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{

    public void StartGame()
    {
        // Charger la scène de jeu.
        Debug.LogError("DifficultyButton : aucune composante Button associée au bouton.");
        SceneManager.LoadScene("EndlessRunner");
    }
}
