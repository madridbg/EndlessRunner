using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

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
        transform.Translate(Vector3.forward * Time.fixedDeltaTime * speed);
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * speed * Time.fixedDeltaTime * horizontalMultiplier);

    }
}
