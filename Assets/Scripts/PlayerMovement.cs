using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public bool alive = true;
    public float speed = 5;
    private Rigidbody rb;
    float horizontalInput;
    [SerializeField] float horizontalMultiplier;
    public float speedIncreasePerPoint = 0.5f;

    [SerializeField] float jumpForce = 400.0f;
    [SerializeField] LayerMask groundMask;
    private void Awake()
    {
       
        rb = GetComponent<Rigidbody>();
        if (!rb)
        {
            Debug.Log("Aucune composante de RigidBody associé à l'objet Player");
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

        Vector3 forwardMove = transform.forward * Time.fixedDeltaTime * speed;
        Vector3 horizontalMove = transform.right * horizontalInput * speed * Time.fixedDeltaTime * horizontalMultiplier;

        rb.MovePosition(rb.position + forwardMove + horizontalMove);

    }

    public void Die()
    {
        alive = false;
        Invoke(nameof(Restart), 2.0f);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Jump()
    {
        float height = GetComponent<Collider>().bounds.size.y;
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, (height / 2) + 0.1f, groundMask);

        rb.AddForce(Vector3.up * jumpForce);
    }
}
