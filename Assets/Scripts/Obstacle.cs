using UnityEngine;

public interface IPlayerMovement
{
    void Die();
}
public class Obstacle : MonoBehaviour
{
    public IPlayerMovement playerMovement;
    private const string PLAYER_TAG = "Player";
    private void Awake()
    {

        GameObject playerMovementGO = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        if (!playerMovementGO)
        {
            Debug.Log("Aucun Player trouvé dans la scène");
            enabled = false;
            return;
        }
        playerMovement = playerMovementGO.GetComponent<IPlayerMovement>();

        if (playerMovement == null)
        {
            Debug.Log("Le Player n'a pas de script implémentant IPlayerMovement");
            enabled = false;
            return;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckCollision(collision.gameObject);
    }

    public void CheckCollision(GameObject hitObject)
    {
        if (hitObject.CompareTag(PLAYER_TAG))
        {
            if (playerMovement != null) playerMovement.Die();
        }
    }
}
