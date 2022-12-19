using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundResizer : MonoBehaviour
{
    [SerializeField] float verticalSize;
    [SerializeField] float horizontalSize;
    [SerializeField] bool isRotated90Deg;

    void Awake()
    {
        float vertCameraSize = 2 * Camera.main.orthographicSize;
        float horizCameraSize = vertCameraSize * Camera.main.aspect;

        Vector3 scale = Vector3.one;

        scale.x = horizCameraSize / horizontalSize;
        scale.y = vertCameraSize / verticalSize;

        if (isRotated90Deg)
        {
            float t = scale.x;
            scale.x = scale.y;
            scale.y = t;
        }

        transform.localScale = scale;
    }

}
