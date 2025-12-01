using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{

    public void StartGame()
    {
        // Charger la scène de jeu.
        SceneManager.LoadScene("EndlessRunner");
    }
}
