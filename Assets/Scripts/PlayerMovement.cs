using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public bool alive = true;
    public float speed = 5;
    public Rigidbody rb;
    float horizontalInput;
    public float horizontalMultiplier;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive) return;

        transform.Translate(Vector3.forward * Time.fixedDeltaTime * speed);
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * speed * Time.fixedDeltaTime * horizontalMultiplier);

        if (transform.position.y < -5)
        {
            Die();
        }

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
}
