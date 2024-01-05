using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed;
    public float maxX;
    public float minX;

    public float smoothSpeedY;
    public float maxY;
    public float minY;
    public float offsetY;

    Vector3 lastPlayerPosition;
    void Start()
    {
        lastPlayerPosition = player.position;
    }

    void Update()
    {
        float targetX = Mathf.Clamp(player.position.x, minX, maxX);
        float smoothedX = Mathf.Lerp(transform.position.x, targetX, smoothSpeed);

        float targetY = Mathf.Clamp(player.position.y, minY, maxY);
        float smoothedY = Mathf.Lerp(transform.position.y, targetY, smoothSpeedY);

        transform.position = new Vector3(smoothedX, smoothedY+offsetY, transform.position.z);

        lastPlayerPosition = player.position;
    }
}
