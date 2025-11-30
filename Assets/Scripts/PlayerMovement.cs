using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPlayerMovement
{

    public bool alive = true;
    public float speed = 5;
    float horizontalInput;
    private bool isOnGround;
    public float speedIncreasePerPoint = 0.05f;
    [SerializeField] float horizontalMultiplier;
    [SerializeField] float jumpForce = 400.0f;
    [SerializeField] LayerMask groundMask;

    private GameManager gameManager;
    public GameObject groundTile;
    private BoxCollider groundCollider;
    private float groundSize;

    public AudioClip crashSound;
    private AudioSource playerAudio;
    private AudioSource backgroundAudio;

    public ParticleSystem explosionParticle;
    public ParticleSystem twinkleEffect;
    public ParticleSystem dust;

    private Rigidbody rb;
    private Animator playerAnim;

    private const string GAMEMANAGER_NAME = "GameManager";
    private const string MAIN_CAMERA_NAME = "Main Camera";
    private const string GROUND_TAG = "Ground";
    private const string COIN_TAG = "Coin";
    private const string DEATH_ANIMATION_NAME = "Death_b";
    private const string DEATH_TYPE_NAME = "DeathType_int";
    private const string HORIZONTAL_AXIS_NAME = "Horizontal";

    private void Awake()
    {
        playerAudio = GetComponent<AudioSource>();
        playerAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (playerAudio == null)
        {
            Debug.Log($"Aucune composante AudioSource associé à l'objet {name}");
            enabled = false;
            return;
        }
        if (playerAnim == null)
        {
            Debug.Log($"Aucune composante Animator associé à l'objet {name}");
            enabled = false;
            return;
        }
        if (rb == null)
        {
            Debug.Log($"Aucune composante de RigidBody associé à l'objet {name}");
            enabled = false;
            return;
        }

        GameObject gameManagerGO = GameObject.Find(GAMEMANAGER_NAME);
        if (gameManagerGO == null)
        {
            Debug.Log($"Aucun objet au nom de {GAMEMANAGER_NAME} ");
            enabled = false;
            return;
        }
        gameManager = gameManagerGO.GetComponent<GameManager>();
        if (gameManager == null)
        {
            Debug.Log($"Aucune composante GameManager associée à l'objet {GAMEMANAGER_NAME} ");
            enabled = false;
            return;
        }

        GameObject mainCamera = GameObject.Find(MAIN_CAMERA_NAME);
        if (gameManagerGO == null)
        {
            Debug.Log($"Aucun objet au nom de {MAIN_CAMERA_NAME} ");
            enabled = false;
            return;
        }
        backgroundAudio = mainCamera.GetComponent<AudioSource>();
        if (gameManager == null)
        {
            Debug.Log($"Aucune composante AudioSource associée à l'objet {MAIN_CAMERA_NAME} ");
            enabled = false;
            return;
        }

        groundCollider = groundTile.GetComponent<BoxCollider>();
        if (groundCollider == null)
        {
            Debug.Log($"Aucune composante BoxCollider associée à l'objet {groundTile.name} ");
            enabled = false;
            return;
        }
        groundSize = groundCollider.size.x / 2;
    }
    private void Update()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL_AXIS_NAME);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

    }

    void FixedUpdate()
    {
        if (!alive) return;

        // Traiter le mouvement, que l'on vérifie avant de l'effectuer. Cela nous permet de ne pas dépasser les limites du sol.
        Vector3 forwardMove = transform.forward * Time.fixedDeltaTime * speed;
        Vector3 horizontalMove = transform.right * horizontalInput * speed * Time.fixedDeltaTime * horizontalMultiplier;
        Vector3 nextPosition = rb.position + forwardMove + horizontalMove;

        if (nextPosition.x <= -groundSize)
        {
            nextPosition.x = -groundSize;
        }
        else if (nextPosition.x >= groundSize)
        {
            nextPosition.x = groundSize;
        }
        rb.MovePosition(nextPosition);

        if (isOnGround)
            dust.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(GROUND_TAG))
        {
            isOnGround = true;
        }
        if (other.gameObject.CompareTag(COIN_TAG))
        {
            twinkleEffect.Play();
        }
    }

    public void Die()
    {
        if (!alive) return;
        alive = false;

        playerAnim.SetBool(DEATH_ANIMATION_NAME, true);
        playerAnim.SetInteger(DEATH_TYPE_NAME, 1);
        explosionParticle.Play();
        playerAudio.PlayOneShot(crashSound, 1.0f);
        gameManager.GameOver();
        backgroundAudio.Stop();
        dust.Stop();
    }

    void Jump()
    {
        float height = GetComponent<Collider>().bounds.size.y;
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, (height / 2) + 0.1f, groundMask);

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isOnGround = false;
    }
}
