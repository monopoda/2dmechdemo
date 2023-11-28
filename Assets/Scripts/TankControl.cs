using UnityEngine;

public class TankControl : MonoBehaviour
{
    private PlayerControls playerControls;
    private Rigidbody2D rb;

    public float maxSpeed;
    public float moveForce;
    public float turningRate;

    private bool moving;
    private bool turning;

    private void Start()
    {
        playerControls = FindObjectOfType<PlayerControls>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (Input.GetKey(playerControls.up) || Input.GetKey(playerControls.down))
            moving = true;
        else moving = false;
        if (Input.GetKey(playerControls.left) || Input.GetKey(playerControls.right))
            turning = true;
        else turning = false;
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        if (rb.angularVelocity > 60 || rb.angularVelocity < -60)
        {
            rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -60, 60);
        }
        MovementControls();
    }
    private void MovementControls()
    {
        if (Input.GetKey(playerControls.up))
        {
            rb.AddForce(transform.up * moveForce, ForceMode2D.Force);
        }
        if (Input.GetKey(playerControls.down))
        {
            rb.AddForce(transform.up * moveForce * -0.6f, ForceMode2D.Force);
        }
        if (Input.GetKey(playerControls.left))
        {
            rb.AddTorque(moveForce * Mathf.Deg2Rad * 10, ForceMode2D.Force);
        }
        if (Input.GetKey(playerControls.right))
        {
            rb.AddTorque(moveForce * Mathf.Deg2Rad * -10, ForceMode2D.Force);
        }
        if (!turning && rb.angularVelocity > 0 || !turning && rb.angularVelocity < 0)
        {
            rb.angularDrag = 10;
        }
        else
        {
            rb.angularDrag = 0;
        }


        Vector2 dragVel = new Vector2(-transform.InverseTransformDirection(rb.velocity).x, 0);
        rb.AddForce(dragVel, ForceMode2D.Force);
    }
}
