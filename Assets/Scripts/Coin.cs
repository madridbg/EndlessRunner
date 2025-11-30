using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] float turnSpeed = 90f;
    private AudioSource playerAudio;
    public AudioClip pickupSound;

    const string OBSTACLE_TAG = "Obstacle";
    const string PLAYER_TAG = "Player";
    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        if (player == null)
        {
            Debug.Log($"Aucun GameObject Player trouvé dans la scène");
            enabled = false;
            return;
        }
        playerAudio = player.GetComponent<AudioSource>();
        if (playerAudio == null)
        {
            Debug.Log($"Aucune composante AudioSource associée à l'objet {PLAYER_TAG}");
            enabled = false;
            return;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(OBSTACLE_TAG))
        {
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag(PLAYER_TAG))
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
