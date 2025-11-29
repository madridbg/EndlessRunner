using Mono.Cecil;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPlayerMovement
{

    public bool alive = true;
    public float speed = 5;
    private Rigidbody rb;
    private Animator playerAnim;
    float horizontalInput;
    [SerializeField] float horizontalMultiplier;
    public float speedIncreasePerPoint = 0.5f;
    private GameManager gameManager;
    public ParticleSystem explosionParticle;
    [SerializeField] float jumpForce = 400.0f;
    [SerializeField] LayerMask groundMask;
    public AudioClip crashSound;
    private AudioSource playerAudio;
    private AudioSource backgroundAudio;
    public GameObject groundTile;
    private void Awake()
    {
        playerAudio = GetComponent<AudioSource>();
        playerAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        backgroundAudio = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        
        if (!rb)
        {
            Debug.Log("Aucune composante de RigidBody associé à l'objet Player");
            enabled = false;
            return;
        }
        if (!playerAnim)
        {
            Debug.Log("Aucune composante de Aninmator dans le player");
            enabled = false;
            return;
        }

    }
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (transform.position.y < -5)
        {
            Die();
        }
    }
    void FixedUpdate()
    {
        if (!alive) return;

        float groundSize = groundTile.GetComponent<BoxCollider>().size.x/2;
        Vector3 forwardMove = transform.forward * Time.fixedDeltaTime * speed;
        if ((rb.position.x >= groundSize && horizontalInput > 0) 
            || (rb.position.x <= -groundSize && horizontalInput < 0))
        {
            horizontalInput = 0;
        }
        Vector3 horizontalMove = transform.right * horizontalInput * speed * Time.fixedDeltaTime * horizontalMultiplier;

        rb.MovePosition(rb.position + forwardMove + horizontalMove);

    }

    public void Die()
    {
        if (!alive) return;
        alive = false;

        playerAnim.SetBool("Death_b", true);
        playerAnim.SetInteger("DeathType_int", 1);
        explosionParticle.Play();
        playerAudio.PlayOneShot(crashSound, 1.0f);
        gameManager.GameOver();
        backgroundAudio.Stop();
    }

    void Jump()
    {
        float height = GetComponent<Collider>().bounds.size.y;
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, (height / 2) + 0.1f, groundMask);

        rb.AddForce(Vector3.up * jumpForce);
    }
}
