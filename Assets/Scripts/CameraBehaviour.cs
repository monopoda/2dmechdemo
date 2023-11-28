using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private PlayerControls playerControls;
    private Transform mech;
    public Texture2D cursorTexture;

    public float parallaxIntensity;

    public int cameraMode;
    public float xOffset = 0f;
    public float yOffset = 0f;
    public int closest = 0;
    public int farthest = 0;

    private Camera cam;

    private void Start()
    {
        playerControls = FindObjectOfType<PlayerControls>();
        cam = GetComponent<Camera>();
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        foreach (MechControl mc in FindObjectsOfType<MechControl>())
        {
            if (mc.currentPlayer == 0)
            {
                mech = mc.transform;
            }
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, farthest, closest));

        if (Time.timeScale == 1 || cameraMode == 3)
        {
            if (Input.mouseScrollDelta.y < 0 && cam.transform.position.z > farthest)
            {
                cam.transform.position -= new Vector3(0, 0, 30);
            }
            else if (Input.mouseScrollDelta.y > 0 && cam.transform.position.z < closest)
            {
                cam.transform.position += new Vector3(0, 0, 30);
            }
        }
        if (cameraMode == 3)
        {
            
        }
    }

    private void FixedUpdate()
    {
        if (mech != null)
        {

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;


            transform.position = Vector3.Lerp(transform.position, new Vector3(mech.position.x + (mousePos.x - mech.position.x) * 0.25f, mech.position.y + (mousePos.y - mech.position.y) * 0.25f, -10), 5 * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position, new Vector3(mech.position.x + (mousePos.x - mech.position.x) * 0.25f, mech.position.y + (mousePos.y - mech.position.y) * 0.25f, -10), 5 * Time.deltaTime);
        }
    }
}
