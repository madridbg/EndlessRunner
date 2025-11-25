using UnityEngine;

public class Obstacle : MonoBehaviour
{
    PlayerMovement playerMovement;
    private void Awake()
    {

        GameObject playerMovementGO = GameObject.FindGameObjectWithTag("Player");
        if (!playerMovementGO)
        {
            Debug.Log("Aucun Player trouvé dans la scène");
            enabled = false;
            return;
        }
        playerMovement = playerMovementGO.GetComponent<PlayerMovement>();
        if (!playerMovement)
        {
            Debug.Log("Aucun script playerMOvement associé avec le Player");
            enabled = false;
            return;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            playerMovement.Die();
        }
    }
}
