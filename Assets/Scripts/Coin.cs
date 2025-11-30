using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] float turnSpeed = 90f;
    private AudioSource playerAudio;
    public AudioClip pickupSound;

    const string obstacleTag = "Obstacle";
    const string playerTag = "Player";
    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player == null)
        {
            Debug.Log($"Aucun GameObject Player trouvé dans la scène");
            enabled = false;
            return;
        }
        playerAudio = player.GetComponent<AudioSource>();
        if (playerAudio == null)
        {
            Debug.Log($"Aucune composante AudioSource associée à l'objet {playerTag}");
            enabled = false;
            return;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(obstacleTag))
        {
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag(playerTag))
        {
            GameManager.inst.IncrementScore();
            playerAudio.PlayOneShot(pickupSound, 1.0f);
            Destroy(gameObject);
            return;
        }

    }

    void Update()
    {
        transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
    }
}
