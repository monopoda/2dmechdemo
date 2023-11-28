using UnityEngine;

public class VehicleColor : MonoBehaviour
{
    public Color color1;
    public Color color2;

    public SpriteRenderer[] color1sprites;
    public SpriteRenderer[] color2sprites;

    void Start()
    {
        SetColors();
    }

    private void SetColors()
    {
        foreach (var sprite in color1sprites)
        {
            sprite.color = color1;
        }
        foreach (var sprite in color2sprites)
        {
            sprite.color = color2;
        }
    }
}
