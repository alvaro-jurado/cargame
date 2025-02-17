using UnityEngine;

public class RoadHeightAdjuster : MonoBehaviour
{
    public float heightOffset = 0.01f;

    void Start()
    {
        Vector3 position = transform.position;
        position.y += heightOffset;
        transform.position = position;
    }
}
