using Unity.VisualScripting;
using UnityEngine;


public class Hook_Movement : MonoBehaviour
{
    [SerializeField] float jumpForce = 0.1f;

    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Move the player
        if (Input.GetKey(KeyCode.Space))
            Move();
    }

    private void Move()
    {
        // Move the player
        rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
    }

}