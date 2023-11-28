using UnityEngine;

public class MechControl : MonoBehaviour
{
    private PlayerControls playerControls;
    private Rigidbody2D rb;

    public int currentPlayer = -1; // -1 means computer controlled, 0 means keyboard, 1+ is the controller ID connected (if there is one)

    public Transform legs;
    public Transform legL;
    public Transform legR;
    public Transform footL;
    public Transform footR;
    public Transform nextFootLocationL;
    public Transform nextFootLocationR;

    public float maxSpeed;
    public float moveForce;
    public float maxFootDistance;
    public float maxFootLocationDistance;
    public float footSpeed;

    private bool moving;
    private float prevRotation;
    private float footResetTimer;

    private void Start()
    {
        playerControls = FindObjectOfType<PlayerControls>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        maxSpeed = moveForce * 2;
        maxFootDistance = moveForce * 0.6f;
        maxFootLocationDistance = moveForce * 2f;
        footSpeed = moveForce * 4.5f;



        if (Input.GetKey(playerControls.up) || Input.GetKey(playerControls.down) || Input.GetKey(playerControls.left) || Input.GetKey(playerControls.right))
            moving = true;
        else moving = false;

        

        legL.localPosition = footL.localPosition * 0.5f;
        legR.localPosition = footR.localPosition * 0.5f;

        ProceduralAnimation();
        FootMovement();
        FootReset();


    }
    private void FixedUpdate()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        transform.up = mousePos - transform.position;
        legs.GetChild(0).up = Vector2.up;
        legs.GetChild(1).up = Vector2.up;
        
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        MovementControls();
    }

    private void MovementControls()
    {
        if (Input.GetKey(playerControls.up))
        {
            rb.AddForce(Vector2.up * moveForce, ForceMode2D.Force);
        }
        if (Input.GetKey(playerControls.down))
        {
            rb.AddForce(Vector2.down * moveForce, ForceMode2D.Force);
        }
        if (Input.GetKey(playerControls.left))
        {
            rb.AddForce(Vector2.left * moveForce, ForceMode2D.Force);
        }
        if (Input.GetKey(playerControls.right))
        {
            rb.AddForce(Vector2.right * moveForce, ForceMode2D.Force);
        }
        if (!moving && rb.velocity.magnitude > 0)
        {
            rb.velocity = Vector2.zero;
        }
    }
    private void ProceduralAnimation()
    {
        Vector3 offset = new Vector3(rb.velocity.x, rb.velocity.y, 0) * 2;
        offset = Vector3.ClampMagnitude(offset, maxFootLocationDistance); // used for offset of legs, so they can stay relative to where their legs are

        Vector3 offsetStep = new Vector3(rb.velocity.x, rb.velocity.y, 0) * 0.025f;

        if (Vector2.Distance(footL.position, legL.position + offsetStep) > maxFootDistance && Vector2.Distance(footR.position, nextFootLocationR.position) < 0.005f && footL.position == nextFootLocationL.position)
        {
            nextFootLocationL.position = legs.GetChild(0).position + offset;
            print("Moving left foot location to " + (legs.GetChild(0).position + offset));
        }
        if (Vector2.Distance(footR.position, legR.position + offsetStep) > maxFootDistance && Vector2.Distance(footL.position, nextFootLocationL.position) < 0.005f && footR.position == nextFootLocationR.position)
        {
            nextFootLocationR.position = legs.GetChild(1).position + offset;
            print("Moving right foot location to " + (legs.GetChild(1).position + offset));
        }
    }
    private void FootMovement()
    {
        if (rb.velocity.magnitude < 0.001f && Vector2.Distance(footL.position, legL.position) < 0.1f)
        {
            footL.position = nextFootLocationL.position;
        }
        if (rb.velocity.magnitude < 0.001f && Vector2.Distance(footR.position, legR.position) < 0.1f)
        {
            footR.position = nextFootLocationR.position;
        }

        footL.transform.position = Vector2.MoveTowards(footL.transform.position, nextFootLocationL.position, footSpeed * Time.deltaTime);
        footR.transform.position = Vector2.MoveTowards(footR.transform.position, nextFootLocationR.position, footSpeed * Time.deltaTime);
    }
    private void FootReset()
    {
        if (!moving)
        {
            if (transform.rotation.eulerAngles.z != prevRotation)
            {
                footResetTimer = 5;
                prevRotation = transform.rotation.eulerAngles.z;
            }
            else if (footResetTimer > 0)
            {
                footResetTimer -= Time.deltaTime;
            }
            else
            {
                if (Vector2.Distance(footR.position, nextFootLocationR.position) < 0.001f)
                {
                    nextFootLocationL.position = legs.GetChild(0).position;
                }
                if (Vector2.Distance(footL.position, nextFootLocationL.position) < 0.001f)
                {
                    nextFootLocationR.position = legs.GetChild(1).position;
                }

                footL.transform.position = Vector2.MoveTowards(footL.transform.position, nextFootLocationL.position, footSpeed * Time.deltaTime * 0.4f);
                footR.transform.position = Vector2.MoveTowards(footR.transform.position, nextFootLocationR.position, footSpeed * Time.deltaTime * 0.4f);
            }
        }
    }
}
