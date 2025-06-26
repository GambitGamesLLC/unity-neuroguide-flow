using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform player;
    public float yAxis;
    public float zAxis;

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, yAxis, zAxis);
    }
}