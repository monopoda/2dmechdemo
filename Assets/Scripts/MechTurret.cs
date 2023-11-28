using UnityEngine;
using UnityEngine.Android;

public class MechTurret : MonoBehaviour
{
    private PlayerControls playerControls;

    public TurretLocation turretPosition;
    public float bulletInterval;
    public float heatUpSpeed;
    public float coolDownSpeed;

    public ParticleSystem ps;

    private void Start()
    {
        playerControls = FindObjectOfType<PlayerControls>();
    }

    private void Update()
    {
        FireTurret();
    }

    private void FireTurret()
    {
        var psemission = ps.emission;

        if (turretPosition == TurretLocation.Left)
        {
            if (Input.GetKey(playerControls.leftWeapon))
            {
                psemission.rateOverTime = bulletInterval;
            }
            else
            {
                psemission.rateOverTime = 0;
            }
        }
        else if (turretPosition == TurretLocation.Right)
        {
            if (Input.GetKey(playerControls.rightWeapon))
            {
                psemission.rateOverTime = bulletInterval;
            }
            else
            {
                psemission.rateOverTime = 0;
            }
        }
    }
}

public enum TurretLocation
{
    Left,
    Right,
    Auxiliary
}